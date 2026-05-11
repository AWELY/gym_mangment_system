namespace gym_mangment_system
{
    partial class SplashForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlVideoHost;
        private System.Windows.Forms.PictureBox picLogo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlVideoHost = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlVideoHost
            // 
            this.pnlVideoHost.BackColor = System.Drawing.Color.Black;
            this.pnlVideoHost.Location = new System.Drawing.Point(400, 20);
            this.pnlVideoHost.Name = "pnlVideoHost";
            this.pnlVideoHost.Size = new System.Drawing.Size(300, 400);
            this.pnlVideoHost.TabIndex = 0;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Location = new System.Drawing.Point(220, 85);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(280, 280);
            this.picLogo.TabIndex = 1;
            this.picLogo.TabStop = false;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(720, 450);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.pnlVideoHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashForm";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
