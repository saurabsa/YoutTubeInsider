namespace YouTubeInsider
{
    partial class YouTubeInsiderMediaPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YouTubeInsiderMediaPlayer));
            this.YTplayer = new AxShockwaveFlashObjects.AxShockwaveFlash();
            this.button1 = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.captureTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.YTplayer)).BeginInit();
            this.SuspendLayout();
            // 
            // YTplayer
            // 
            this.YTplayer.Enabled = true;
            this.YTplayer.Location = new System.Drawing.Point(22, 12);
            this.YTplayer.Name = "YTplayer";
            this.YTplayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("YTplayer.OcxState")));
            this.YTplayer.Size = new System.Drawing.Size(805, 507);
            this.YTplayer.TabIndex = 0;
            this.YTplayer.FSCommand += new AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEventHandler(this.YTplayer_FSCommand);
            this.YTplayer.FlashCall += new AxShockwaveFlashObjects._IShockwaveFlashEvents_FlashCallEventHandler(this.YTplayer_FlashCall);
            this.YTplayer.Enter += new System.EventHandler(this.YTplayer_Enter);
            this.YTplayer.Leave += new System.EventHandler(this.YTplayer_Leave);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(847, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(201, 69);
            this.button1.TabIndex = 1;
            this.button1.Text = "Capture";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.convertButton_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(22, 548);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(196, 38);
            this.playButton.TabIndex = 2;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(320, 548);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(204, 38);
            this.pauseButton.TabIndex = 3;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(617, 548);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(210, 38);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // captureTime
            // 
            this.captureTime.AutoSize = true;
            this.captureTime.Location = new System.Drawing.Point(844, 114);
            this.captureTime.Name = "captureTime";
            this.captureTime.Size = new System.Drawing.Size(0, 17);
            this.captureTime.TabIndex = 5;
            // 
            // YouTubeInsiderMediaPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 644);
            this.Controls.Add(this.captureTime);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.YTplayer);
            this.Name = "YouTubeInsiderMediaPlayer";
            this.Text = "YouTubeInsiderMediaPlayer";
            ((System.ComponentModel.ISupportInitialize)(this.YTplayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxShockwaveFlashObjects.AxShockwaveFlash YTplayer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label captureTime;
    }
}