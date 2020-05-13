namespace arelith_portrait_maker
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.picbox = new System.Windows.Forms.PictureBox();
            this.btn_load_img = new System.Windows.Forms.Button();
            this.btn_generate = new System.Windows.Forms.Button();
            this.btn_fit_portrait = new System.Windows.Forms.Button();
            this.btn_crop_l = new System.Windows.Forms.Button();
            this.btn_crop_m = new System.Windows.Forms.Button();
            this.btn_crop_s = new System.Windows.Forms.Button();
            this.btn_crop_t = new System.Windows.Forms.Button();
            this.btn_end_crop = new System.Windows.Forms.Button();
            this.lbl_debug = new System.Windows.Forms.Label();
            this.slider_scale = new System.Windows.Forms.TrackBar();
            this.lbl_scale = new System.Windows.Forms.Label();
            this.lbl_scale_val = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slider_scale)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(522, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arelith Portrait Maker by Zaphiel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picbox
            // 
            this.picbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picbox.Location = new System.Drawing.Point(153, 43);
            this.picbox.Name = "picbox";
            this.picbox.Size = new System.Drawing.Size(256, 400);
            this.picbox.TabIndex = 1;
            this.picbox.TabStop = false;
            this.picbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picbox_MouseDown);
            this.picbox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picbox_MouseMove);
            this.picbox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picbox_MouseUp);
            // 
            // btn_load_img
            // 
            this.btn_load_img.Location = new System.Drawing.Point(17, 43);
            this.btn_load_img.Name = "btn_load_img";
            this.btn_load_img.Size = new System.Drawing.Size(130, 40);
            this.btn_load_img.TabIndex = 2;
            this.btn_load_img.Text = "Load Image";
            this.btn_load_img.UseVisualStyleBackColor = true;
            this.btn_load_img.Click += new System.EventHandler(this.btn_load_img_Click);
            // 
            // btn_generate
            // 
            this.btn_generate.Enabled = false;
            this.btn_generate.Location = new System.Drawing.Point(17, 135);
            this.btn_generate.Name = "btn_generate";
            this.btn_generate.Size = new System.Drawing.Size(130, 40);
            this.btn_generate.TabIndex = 3;
            this.btn_generate.Text = "Generate";
            this.btn_generate.UseVisualStyleBackColor = true;
            this.btn_generate.Click += new System.EventHandler(this.btn_generate_Click);
            // 
            // btn_resize
            // 
            this.btn_fit_portrait.Enabled = false;
            this.btn_fit_portrait.Location = new System.Drawing.Point(17, 89);
            this.btn_fit_portrait.Name = "btn_resize";
            this.btn_fit_portrait.Size = new System.Drawing.Size(130, 40);
            this.btn_fit_portrait.TabIndex = 4;
            this.btn_fit_portrait.Text = "Fit Portrait";
            this.btn_fit_portrait.UseVisualStyleBackColor = true;
            this.btn_fit_portrait.Click += new System.EventHandler(this.btn_fit_portrait_Click);
            // 
            // btn_crop_l
            // 
            this.btn_crop_l.Location = new System.Drawing.Point(415, 43);
            this.btn_crop_l.Name = "btn_crop_l";
            this.btn_crop_l.Size = new System.Drawing.Size(130, 40);
            this.btn_crop_l.TabIndex = 5;
            this.btn_crop_l.Text = "Crop (L)";
            this.btn_crop_l.UseVisualStyleBackColor = true;
            this.btn_crop_l.Visible = false;
            this.btn_crop_l.Click += new System.EventHandler(this.btn_crop_l_Click);
            // 
            // btn_crop_m
            // 
            this.btn_crop_m.Location = new System.Drawing.Point(415, 89);
            this.btn_crop_m.Name = "btn_crop_m";
            this.btn_crop_m.Size = new System.Drawing.Size(130, 40);
            this.btn_crop_m.TabIndex = 6;
            this.btn_crop_m.Text = "Crop (M)";
            this.btn_crop_m.UseVisualStyleBackColor = true;
            this.btn_crop_m.Visible = false;
            this.btn_crop_m.Click += new System.EventHandler(this.btn_crop_m_Click);
            // 
            // btn_crop_s
            // 
            this.btn_crop_s.Location = new System.Drawing.Point(415, 135);
            this.btn_crop_s.Name = "btn_crop_s";
            this.btn_crop_s.Size = new System.Drawing.Size(130, 40);
            this.btn_crop_s.TabIndex = 7;
            this.btn_crop_s.Text = "Crop (S)";
            this.btn_crop_s.UseVisualStyleBackColor = true;
            this.btn_crop_s.Visible = false;
            this.btn_crop_s.Click += new System.EventHandler(this.btn_crop_s_Click);
            // 
            // btn_crop_t
            // 
            this.btn_crop_t.Location = new System.Drawing.Point(415, 181);
            this.btn_crop_t.Name = "btn_crop_t";
            this.btn_crop_t.Size = new System.Drawing.Size(130, 40);
            this.btn_crop_t.TabIndex = 8;
            this.btn_crop_t.Text = "Crop (T)";
            this.btn_crop_t.UseVisualStyleBackColor = true;
            this.btn_crop_t.Visible = false;
            this.btn_crop_t.Click += new System.EventHandler(this.btn_crop_t_Click);
            // 
            // btn_end_crop
            // 
            this.btn_end_crop.Location = new System.Drawing.Point(415, 227);
            this.btn_end_crop.Name = "btn_end_crop";
            this.btn_end_crop.Size = new System.Drawing.Size(130, 40);
            this.btn_end_crop.TabIndex = 9;
            this.btn_end_crop.Text = "End Cropping";
            this.btn_end_crop.UseVisualStyleBackColor = true;
            this.btn_end_crop.Visible = false;
            this.btn_end_crop.Click += new System.EventHandler(this.btn_end_crop_Click);
            // 
            // lbl_debug
            // 
            this.lbl_debug.Location = new System.Drawing.Point(17, 446);
            this.lbl_debug.Name = "lbl_debug";
            this.lbl_debug.Size = new System.Drawing.Size(529, 17);
            this.lbl_debug.TabIndex = 10;
            this.lbl_debug.Text = "Debug label";
            this.lbl_debug.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_debug.Visible = false;
            // 
            // slider_scale
            // 
            this.slider_scale.AutoSize = false;
            this.slider_scale.LargeChange = 1;
            this.slider_scale.Location = new System.Drawing.Point(415, 274);
            this.slider_scale.Maximum = 0;
            this.slider_scale.Minimum = -100;
            this.slider_scale.Name = "slider_scale";
            this.slider_scale.Size = new System.Drawing.Size(130, 38);
            this.slider_scale.TabIndex = 11;
            this.slider_scale.Visible = false;
            this.slider_scale.Scroll += new System.EventHandler(this.slider_scale_Scroll);
            // 
            // lbl_scale
            // 
            this.lbl_scale.BackColor = System.Drawing.Color.Transparent;
            this.lbl_scale.Location = new System.Drawing.Point(441, 315);
            this.lbl_scale.Name = "lbl_scale";
            this.lbl_scale.Size = new System.Drawing.Size(47, 23);
            this.lbl_scale.TabIndex = 12;
            this.lbl_scale.Text = "Scale:";
            this.lbl_scale.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbl_scale.Visible = false;
            // 
            // lbl_scale_val
            // 
            this.lbl_scale_val.AutoSize = true;
            this.lbl_scale_val.Location = new System.Drawing.Point(484, 315);
            this.lbl_scale_val.Name = "lbl_scale_val";
            this.lbl_scale_val.Size = new System.Drawing.Size(28, 17);
            this.lbl_scale_val.TabIndex = 11;
            this.lbl_scale_val.Text = "0.0";
            this.lbl_scale_val.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbl_scale_val.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 472);
            this.Controls.Add(this.lbl_scale_val);
            this.Controls.Add(this.lbl_scale);
            this.Controls.Add(this.slider_scale);
            this.Controls.Add(this.lbl_debug);
            this.Controls.Add(this.btn_end_crop);
            this.Controls.Add(this.btn_crop_t);
            this.Controls.Add(this.btn_crop_s);
            this.Controls.Add(this.btn_crop_m);
            this.Controls.Add(this.btn_crop_l);
            this.Controls.Add(this.btn_fit_portrait);
            this.Controls.Add(this.btn_generate);
            this.Controls.Add(this.btn_load_img);
            this.Controls.Add(this.picbox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Arelith Portrait Maker";
            this.TransparencyKey = System.Drawing.Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slider_scale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picbox;
        private System.Windows.Forms.Button btn_load_img;
        private System.Windows.Forms.Button btn_generate;
        private System.Windows.Forms.Button btn_fit_portrait;
        private System.Windows.Forms.Button btn_crop_l;
        private System.Windows.Forms.Button btn_crop_m;
        private System.Windows.Forms.Button btn_crop_s;
        private System.Windows.Forms.Button btn_crop_t;
        private System.Windows.Forms.Button btn_end_crop;
        private System.Windows.Forms.Label lbl_debug;
        private System.Windows.Forms.TrackBar slider_scale;
        private System.Windows.Forms.Label lbl_scale;
        private System.Windows.Forms.Label lbl_scale_val;
    }
}

