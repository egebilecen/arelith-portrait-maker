using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace arelith_portrait_maker
{
    public partial class Main : Form
    {
        private bool is_cropping   = false;
        private bool is_mouse_down = false;

        private string crop_area_str;
        private Point  crop_area_pos;
        private Size   crop_area_size;
        private Color  crop_border_color = Color.FromArgb(0, 255, 0);
        private int    crop_area_step_size = 5;

        private string portrait_path;
        private Bitmap original_image; // will always hold original RESIZED image
        private Bitmap canvas; // any changes will happen on this canvas
        private Bitmap canvas_scaled = null; // will hold scaled image

        private List<KeyValuePair<string, Bitmap>> crop_list = new List<KeyValuePair<string, Bitmap>>();

        private string template_folder = @".\template";
        private string output_folder   = @".\output";
        private string output_prefix   = "po_";

        public static class Portrait_Dimension
        {
            //width, height, negative value
            //https://steamcommunity.com/sharedfiles/filedetails/?id=1344346521
            static public int[] huge   = { 256, 400, 112 };
            static public int[] large  = { 128, 200,  56 };
            static public int[] medium = { 64,  100,  28 };
            static public int[] small  = { 32,   50,  14 };
            static public int[] tiny   = { 16,   25,   7 };
        }

        public Main()
        {
            if(!Directory.Exists(template_folder))
            {
                MessageBox.Show("Couldn't find \""+template_folder.Replace(".\\", "")+"\" folder. Be sure to \""+template_folder.Replace(".\\", "")+"\" folder is located in the same directory with application.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);

                return;
            }
            else
            {
                if(!File.Exists(template_folder+@"\template_h.png") 
                || !File.Exists(template_folder+@"\template_l.png")
                || !File.Exists(template_folder+@"\template_m.png")
                || !File.Exists(template_folder+@"\template_s.png")
                || !File.Exists(template_folder+@"\template_t.png"))
                {
                    MessageBox.Show("Couldn't find template images. Be sure to \""+template_folder.Replace(".\\", "")+"\" folder contains the 5 template images.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);

                    return;
                }
            }

            // INIT
            this.KeyPreview = true;
            InitializeComponent();

            this.crop_area_step_size = Int32.Parse(this.input_stepsize.Text);
        }

        //https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect  = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private string get_file_extension(string file_path)
        {
            var split = file_path.Split('\\');

            return split[split.Length - 1].Split('.')[1];
        }

        private string get_file_name(string file_path)
        {
            var split = file_path.Split('\\');

            return split[split.Length - 1].Split('.')[0];
        }

        private Bitmap get_bitmap_from_crop_list(string key)
        {
            for(var i=0; i < this.crop_list.Count; i++)
            {
                if(this.crop_list[i].Key == key) return this.crop_list[i].Value;
            }

            return null;
        }

        private bool add_bitmap_to_crop_list(string key, Bitmap bitmap)
        {
            if(get_bitmap_from_crop_list(key) == null)
            {
                this.crop_list.Add(new KeyValuePair<string, Bitmap>(key, bitmap));
            }

            return false;
        }

        private bool delete_bitmap_from_crop_list(string key)
        {
            for(var i=0; i < this.crop_list.Count; i++)
            {
                if(this.crop_list[i].Key == key)
                {
                    this.crop_list.RemoveAt(i);

                    return true;
                }
            }

            return false;
        }

        private void show_crop_section()
        {
            this.btn_crop_l.Visible = true;
            this.btn_crop_m.Visible = true;
            this.btn_crop_s.Visible = true;
            this.btn_crop_t.Visible = true;
        }

        private void hide_crop_section()
        {
            this.btn_crop_l.Visible = false;
            this.btn_crop_m.Visible = false;
            this.btn_crop_s.Visible = false;
            this.btn_crop_t.Visible = false;

            hide_scale_section();
        }

        private void show_scale_section()
        {
            this.slider_scale.Visible  = true;
            this.lbl_scale.Visible     = true;
            this.lbl_scale_val.Visible = true;
        }

        private void hide_scale_section()
        {
            this.slider_scale.Visible  = false;
            this.lbl_scale.Visible     = false;
            this.lbl_scale_val.Visible = false;
        }

        private void show_crop_step_size_section()
        {
            this.lbl_step_size.Visible  = true;
            this.input_stepsize.Visible = true;
            this.lbl_step_size_info.Visible = true;
        }

        private void hide_crop_step_size_section()
        {
            this.lbl_step_size.Visible  = false;
            this.input_stepsize.Visible = false;
            this.lbl_step_size_info.Visible = false;
        }

        private void clear_all_buttons()
        {
            this.picbox.BackgroundImage = null;
            this.btn_fit_portrait.Enabled = false;
            this.btn_generate.Enabled = false;
            this.btn_end_crop.Visible = false;

            this.crop_list.Clear();

            hide_crop_section();
        }

        private void free_bitmap(ref Bitmap bmp)
        {
            if(bmp == null) return;

            bmp.Dispose();
            bmp = null;
        }

        private void event_clipping_begin()
        {
            this.btn_load_img.Enabled = false;
            this.btn_fit_portrait.Enabled   = false;
            this.btn_generate.Enabled = false;
            this.btn_end_crop.Visible = true;

            show_scale_section();
            show_crop_step_size_section();
        }

        private void event_clipping_end()
        {
            this.btn_load_img.Enabled = true;
            this.btn_fit_portrait.Enabled   = true;
            this.btn_generate.Enabled = true;
            this.btn_end_crop.Visible = false;

            hide_scale_section();
            hide_crop_step_size_section();

            this.lbl_scale_val.Text = "0.0";
            this.slider_scale.Value = 0;
        }

        private void btn_load_img_Click(object sender, EventArgs e)
        {
            OpenFileDialog file_dialog = new OpenFileDialog();

            file_dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            file_dialog.Title = "Please select a portrait";

            if(file_dialog.ShowDialog() == DialogResult.OK)
            {
                if(file_dialog.FileName != String.Empty)
                {
                    this.portrait_path  = file_dialog.FileName;
                    this.original_image = new Bitmap(portrait_path);

                    if(get_file_name(this.portrait_path).Length > 10)
                    {
                        MessageBox.Show("Selected portrait's filename is exceeding 10 characters! Changing your portrait's filename to have max. 10 characters is highly suggested before continuing to using tool.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    /*if(canvas.Width < Portrait_Dimension.huge[0])
                    {
                        MessageBox.Show("Selected portrait's width must be at least "+Portrait_Dimension.huge[0].ToString()+"px!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;    
                    }
                    else if(canvas.Height < Portrait_Dimension.huge[1])
                    {
                        MessageBox.Show("Selected portrait's height must be at least "+Portrait_Dimension.huge[1].ToString()+"px!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }*/

                    // Remove transparency
                    Bitmap temp_canvas = (Bitmap) this.original_image.Clone();

                    using (Graphics g = Graphics.FromImage(this.original_image))
                    {
                        g.Clear(Color.FromArgb(254, 254, 254));
                        g.DrawImage(temp_canvas, 0, 0);
                    }

                    free_bitmap(ref temp_canvas);

                    clear_all_buttons();
                    btn_fit_portrait_Click(sender, null);
                    
                    this.picbox.BackgroundImage = this.original_image;
                    this.btn_fit_portrait.Enabled = true;
                }
            }
        }

        private void btn_fit_portrait_Click(object sender, EventArgs e)
        {
            this.original_image = ResizeImage(this.original_image, Portrait_Dimension.huge[0], Portrait_Dimension.huge[1]);
            this.canvas = (Bitmap) this.original_image.Clone();

            this.picbox.BackgroundImage  = this.original_image;

            show_crop_section();

            this.btn_fit_portrait.Enabled = false;
            this.btn_generate.Enabled     = true;
        }

        private void btn_generate_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(output_folder)) Directory.CreateDirectory(output_folder);

            string portrait_name = get_file_name(this.portrait_path).ToLower();

            try
            {
                //Huge portrait template
                Bitmap template_h = new Bitmap(template_folder+@"\template_h.png");

                using (Graphics g = Graphics.FromImage(template_h))
                {
                    g.DrawImage(this.original_image, 
                                new Rectangle(0, 0, template_h.Width, template_h.Height - Portrait_Dimension.huge[2]), 
                                0, 0, 
                                this.original_image.Width, this.original_image.Height, 
                                GraphicsUnit.Pixel);
                }

                //Large portrait template
                Bitmap template_l = new Bitmap(template_folder+@"\template_l.png");

                using (Graphics g = Graphics.FromImage(template_l))
                {
                    if (get_bitmap_from_crop_list("large") == null)
                    {
                        this.canvas = ResizeImage(this.original_image, Portrait_Dimension.large[0], Portrait_Dimension.large[1]);

                        g.DrawImage(this.canvas,
                                    new Rectangle(0, 0, template_l.Width, template_l.Height - Portrait_Dimension.large[2]),
                                    0, 0,
                                    this.canvas.Width, this.canvas.Height,
                                    GraphicsUnit.Pixel);
                    }
                    else
                    {
                        Bitmap cropped_image = get_bitmap_from_crop_list("large");

                        g.DrawImage(cropped_image,
                                    new Rectangle(0, 0, template_l.Width, template_l.Height - Portrait_Dimension.large[2]),
                                    0, 0,
                                    cropped_image.Width, cropped_image.Height,
                                    GraphicsUnit.Pixel);
                    }
                }

                //Medium portrait template
                Bitmap template_m = new Bitmap(template_folder+@"\template_m.png");

                using (Graphics g = Graphics.FromImage(template_m))
                {
                    if (get_bitmap_from_crop_list("medium") == null)
                    {
                        this.canvas = ResizeImage(this.original_image, Portrait_Dimension.medium[0], Portrait_Dimension.medium[1]);

                        g.DrawImage(this.canvas,
                                    new Rectangle(0, 0, template_m.Width, template_m.Height - Portrait_Dimension.medium[2]),
                                    0, 0,
                                    this.canvas.Width, this.canvas.Height,
                                    GraphicsUnit.Pixel);
                    }
                    else
                    {
                        Bitmap cropped_image = get_bitmap_from_crop_list("medium");

                        g.DrawImage(cropped_image,
                                    new Rectangle(0, 0, template_m.Width, template_m.Height - Portrait_Dimension.medium[2]),
                                    0, 0,
                                    cropped_image.Width, cropped_image.Height,
                                    GraphicsUnit.Pixel);
                    }
                }

                //Small portrait template
                Bitmap template_s = new Bitmap(template_folder+@"\template_s.png");

                using (Graphics g = Graphics.FromImage(template_s))
                {
                    if (get_bitmap_from_crop_list("small") == null)
                    {
                        this.canvas = ResizeImage(this.original_image, Portrait_Dimension.small[0], Portrait_Dimension.small[1]);

                        g.DrawImage(this.canvas,
                                    new Rectangle(0, 0, template_s.Width, template_s.Height - Portrait_Dimension.small[2]),
                                    0, 0,
                                    this.canvas.Width, this.canvas.Height,
                                    GraphicsUnit.Pixel);
                    }
                    else
                    {
                        Bitmap cropped_image = get_bitmap_from_crop_list("small");

                        g.DrawImage(cropped_image,
                                    new Rectangle(0, 0, template_s.Width, template_s.Height - Portrait_Dimension.small[2]),
                                    0, 0,
                                    cropped_image.Width, cropped_image.Height,
                                    GraphicsUnit.Pixel);
                    }
                }

                //Tiny portrait template
                Bitmap template_t = new Bitmap(template_folder+@"\template_t.png");

                using (Graphics g = Graphics.FromImage(template_t))
                {
                    if (get_bitmap_from_crop_list("tiny") == null)
                    {
                        this.canvas = ResizeImage(this.original_image, Portrait_Dimension.tiny[0], Portrait_Dimension.tiny[1]);

                        g.DrawImage(this.canvas,
                                    new Rectangle(0, 0, template_t.Width, template_t.Height - Portrait_Dimension.tiny[2]),
                                    0, 0,
                                    this.canvas.Width, this.canvas.Height,
                                    GraphicsUnit.Pixel);
                    }
                    else
                    {
                        Bitmap cropped_image = get_bitmap_from_crop_list("tiny");

                        g.DrawImage(cropped_image,
                                    new Rectangle(0, 0, template_t.Width, template_t.Height - Portrait_Dimension.tiny[2]),
                                    0, 0,
                                    cropped_image.Width, cropped_image.Height,
                                    GraphicsUnit.Pixel);
                    }
                }

                template_h.Save(output_folder+@"\" + output_prefix + portrait_name + "_h.png", ImageFormat.Png);
                template_l.Save(output_folder+@"\" + output_prefix + portrait_name + "_l.png", ImageFormat.Png);
                template_m.Save(output_folder+@"\" + output_prefix + portrait_name + "_m.png", ImageFormat.Png);
                template_s.Save(output_folder+@"\" + output_prefix + portrait_name + "_s.png", ImageFormat.Png);
                template_t.Save(output_folder+@"\" + output_prefix + portrait_name + "_t.png", ImageFormat.Png);

                clear_all_buttons();

                MessageBox.Show("Portraits are successfully generated. Please check \""+output_folder.Replace(".\\", "")+"\" folder.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(output_folder);

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while generating portraits. (Exception: "+ex.Message+")", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            clear_all_buttons();

            return;
        }

        private void btn_crop_l_Click(object sender, EventArgs e)
        {
            if(this.is_cropping)
            {
                MessageBox.Show("Please end your current cropping before starting new one.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            event_clipping_begin();

            this.crop_area_str = "large";
            this.crop_area_pos = new Point(
                (this.original_image.Width  / 2) - (Portrait_Dimension.large[0] / 2), 
                (this.original_image.Height / 2) - (Portrait_Dimension.large[1] / 2)
            );
            this.crop_area_size = new Size(Portrait_Dimension.large[0], Portrait_Dimension.large[1]);

            using(Graphics g = Graphics.FromImage(this.canvas))
            {
                g.DrawRectangle(new Pen(crop_border_color, 2), new Rectangle(
                    this.crop_area_pos,
                    this.crop_area_size
                ));
            }

            this.picbox.BackgroundImage = this.canvas;

            this.is_cropping = true;
        }

        private void btn_crop_m_Click(object sender, EventArgs e)
        {
            if(this.is_cropping)
            {
                MessageBox.Show("Please end your current cropping before starting new one.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            event_clipping_begin();

            this.crop_area_str = "medium";
            this.crop_area_pos = new Point(
                (this.original_image.Width / 2)  - (Portrait_Dimension.medium[0] / 2), 
                (this.original_image.Height / 2) - (Portrait_Dimension.medium[1] / 2)
            );
            this.crop_area_size = new Size(Portrait_Dimension.medium[0], Portrait_Dimension.medium[1]);

            using(Graphics g = Graphics.FromImage(this.canvas))
            {
                g.DrawRectangle(new Pen(crop_border_color, 2), new Rectangle(
                    this.crop_area_pos,
                    this.crop_area_size
                ));
            }

            this.picbox.BackgroundImage = this.canvas;

            this.is_cropping = true;
        }

        private void btn_crop_s_Click(object sender, EventArgs e)
        {
            if(this.is_cropping)
            {
                MessageBox.Show("Please end your current cropping before starting new one.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            event_clipping_begin();

            this.crop_area_str = "small";
            this.crop_area_pos = new Point(
                (this.original_image.Width / 2)  - (Portrait_Dimension.small[0] / 2), 
                (this.original_image.Height / 2) - (Portrait_Dimension.small[1] / 2)
            );
            this.crop_area_size = new Size(Portrait_Dimension.small[0], Portrait_Dimension.small[1]);

            using(Graphics g = Graphics.FromImage(this.canvas))
            {
                g.DrawRectangle(new Pen(crop_border_color, 2), new Rectangle(
                    this.crop_area_pos,
                    this.crop_area_size
                ));
            }

            this.picbox.BackgroundImage = this.canvas;

            this.is_cropping = true;
        }

        private void btn_crop_t_Click(object sender, EventArgs e)
        {
            if(this.is_cropping)
            {
                MessageBox.Show("Please end your current cropping before starting new one.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            event_clipping_begin();

            this.crop_area_str = "tiny";
            this.crop_area_pos = new Point(
                (this.original_image.Width / 2)  - (Portrait_Dimension.tiny[0] / 2), 
                (this.original_image.Height / 2) - (Portrait_Dimension.tiny[1] / 2)
            );
            this.crop_area_size = new Size(Portrait_Dimension.tiny[0], Portrait_Dimension.tiny[1]);

            using(Graphics g = Graphics.FromImage(this.canvas))
            {
                g.DrawRectangle(new Pen(crop_border_color, 2), new Rectangle(
                    this.crop_area_pos,
                    this.crop_area_size
                ));
            }

            this.picbox.BackgroundImage = this.canvas;

            this.is_cropping = true;
        }

        private void btn_end_crop_Click(object sender, EventArgs e)
        {
            DialogResult dialog_res = MessageBox.Show("Do you want to end the cropping?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            if(dialog_res == DialogResult.No) return;

            this.is_cropping = false;

            if(dialog_res != DialogResult.Cancel)
            {

                if(this.canvas_scaled == null)
                    this.canvas = (Bitmap) this.original_image.Clone();
                else
                    this.canvas = this.canvas_scaled; //no need to clone for keeping original scaled image,
                                                      //will deleted regardless

                if(get_bitmap_from_crop_list(this.crop_area_str) != null)
                    delete_bitmap_from_crop_list(this.crop_area_str);

                Bitmap cropped_area = new Bitmap(this.crop_area_size.Width, this.crop_area_size.Height);

                using (Graphics g = Graphics.FromImage(cropped_area))
                {
                    g.DrawImage(this.canvas, new Rectangle(0, 0, cropped_area.Width, cropped_area.Height), 
                                this.crop_area_pos.X, this.crop_area_pos.Y, 
                                this.crop_area_size.Width, this.crop_area_size.Height, 
                                GraphicsUnit.Pixel);
                }

                add_bitmap_to_crop_list(this.crop_area_str, cropped_area);
            }

            free_bitmap(ref this.canvas_scaled);

            this.canvas = (Bitmap) this.original_image.Clone();
            
            this.picbox.BackgroundImage = this.original_image;

            event_clipping_end();
        }

        private void picbox_MouseDown(object sender, MouseEventArgs e)
        {
            this.is_mouse_down = true;
        }

        private void picbox_MouseUp(object sender, MouseEventArgs e)
        {
            this.is_mouse_down = false;
        }

        private void picbox_MouseMove(object sender, MouseEventArgs e)
        {
            if(this.is_cropping && this.is_mouse_down)
            {
                free_bitmap(ref this.canvas);

                if(this.canvas_scaled == null)
                    this.canvas = (Bitmap) this.original_image.Clone();
                else
                    this.canvas = (Bitmap) this.canvas_scaled.Clone();

                int point_x = 0;
                int point_y = 0;

                if(e.Location.X - (this.crop_area_size.Width / 2) < 0) point_x = 0;
                else if(e.Location.X + (this.crop_area_size.Width / 2) > this.canvas.Width) 
                    point_x = this.canvas.Width - this.crop_area_size.Width;
                else 
                    point_x = e.Location.X - (this.crop_area_size.Width / 2);

                if(e.Location.Y - (this.crop_area_size.Height / 2) < 0) point_y = 0;
                else if(e.Location.Y + (this.crop_area_size.Height / 2) > this.canvas.Height) 
                    point_y = this.canvas.Height - this.crop_area_size.Height;
                else 
                    point_y = e.Location.Y - (this.crop_area_size.Height / 2);
                
                this.crop_area_pos.X = point_x;
                this.crop_area_pos.Y = point_y;

                using (Graphics g = Graphics.FromImage(this.canvas))
                {
                    g.DrawRectangle(new Pen(crop_border_color, 2), new Rectangle(
                        this.crop_area_pos,
                        this.crop_area_size
                    ));
                }

                this.picbox.BackgroundImage = this.canvas;
            }
        }

        private void slider_scale_Scroll(object sender, EventArgs e)
        {
            float slider_val = (float) this.slider_scale.Value / 10;

            this.lbl_scale_val.Text = ((this.slider_scale.Value > 0) ? "+" : "") + slider_val.ToString();
            
            free_bitmap(ref this.canvas);
            free_bitmap(ref this.canvas_scaled);

            if(slider_val >= -1f && slider_val <= 1f)
            {
                this.lbl_scale_val.Text = "0.0";

                this.canvas = (Bitmap) this.original_image.Clone();

                using(Graphics g = Graphics.FromImage(this.canvas))
                {
                    g.DrawRectangle(new Pen(crop_border_color, 2), new Rectangle(
                        new Point(7, 7),
                        this.crop_area_size
                    ));
                }

                this.picbox.BackgroundImage = this.canvas;

                return;
            }

            float scaled_img_w = this.original_image.Width;
            float scaled_img_h = this.original_image.Height;

            if(slider_val > 0)
            {
                scaled_img_w *= slider_val;
                scaled_img_h *= slider_val;
            }
            else
            {
                slider_val *= -1;

                scaled_img_w /= slider_val;
                scaled_img_h /= slider_val;
            }

            this.canvas_scaled = ResizeImage(this.original_image, (int) Math.Ceiling(scaled_img_w), (int) Math.Ceiling(scaled_img_h));
            this.canvas        = (Bitmap) this.canvas_scaled.Clone();

            using(Graphics g = Graphics.FromImage(this.canvas))
            {
                g.DrawRectangle(new Pen(crop_border_color, 2), new Rectangle(
                    new Point(7, 7),
                    this.crop_area_size
                ));
            }

            this.picbox.BackgroundImage = this.canvas;
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if(this.is_cropping && !this.is_mouse_down)
            {
                free_bitmap(ref this.canvas);

                if(this.canvas_scaled == null)
                    this.canvas = (Bitmap) this.original_image.Clone();
                else
                    this.canvas = (Bitmap) this.canvas_scaled.Clone();

                switch(e.KeyCode)
                {
                    case Keys.W:
                        if(this.crop_area_pos.Y - crop_area_step_size >= 0)
                            this.crop_area_pos.Y -= crop_area_step_size;
                        else
                            this.crop_area_pos.Y = 0;
                    break;

                    case Keys.S:
                        if(this.crop_area_pos.Y + crop_area_step_size <= (this.canvas.Height - this.crop_area_size.Height))
                            this.crop_area_pos.Y += crop_area_step_size;
                        else
                            this.crop_area_pos.Y = this.canvas.Height - this.crop_area_size.Height;
                    break;

                    case Keys.A:
                        if(this.crop_area_pos.X - crop_area_step_size >= 0)
                            this.crop_area_pos.X -= crop_area_step_size;
                        else
                            this.crop_area_pos.X = 0;
                    break;

                    case Keys.D:
                        if(this.crop_area_pos.X + crop_area_step_size <= (this.canvas.Width - this.crop_area_size.Width))
                            this.crop_area_pos.X += crop_area_step_size;
                        else
                            this.crop_area_pos.X = this.canvas.Width - this.crop_area_size.Width;
                    break;

                    case Keys.Enter:
                        if(this.input_stepsize.ContainsFocus)
                        {
                            try
                            {
                                this.ActiveControl = null;
                                this.crop_area_step_size = Int32.Parse(this.input_stepsize.Text);
                            }
                            catch(Exception)
                            {
                                this.input_stepsize.Text = this.crop_area_step_size.ToString();
                            }
                        }
                    break;
                }

                using (Graphics g = Graphics.FromImage(this.canvas))
                {
                    g.DrawRectangle(new Pen(crop_border_color, 2), new Rectangle(
                        this.crop_area_pos,
                        this.crop_area_size
                    ));
                }

                this.picbox.BackgroundImage = this.canvas;
            }
        }
    }
}
