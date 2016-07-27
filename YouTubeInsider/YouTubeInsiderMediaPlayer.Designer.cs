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
            this.captureBtn = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.captureTime = new System.Windows.Forms.Label();
            this.screenShotPicture = new System.Windows.Forms.PictureBox();
            this.decodeBtn = new System.Windows.Forms.Button();
            this.cropBtn = new System.Windows.Forms.Button();
            this.TargetPicBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.meanConfidence = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.YTplayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenShotPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetPicBox)).BeginInit();
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
            // captureBtn
            // 
            this.captureBtn.Location = new System.Drawing.Point(833, 12);
            this.captureBtn.Name = "captureBtn";
            this.captureBtn.Size = new System.Drawing.Size(185, 41);
            this.captureBtn.TabIndex = 2;
            this.captureBtn.Text = "Capture";
            this.captureBtn.UseVisualStyleBackColor = true;
            this.captureBtn.Click += new System.EventHandler(this.captureButton_Click);
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(22, 548);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(204, 41);
            this.playButton.TabIndex = 3;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(320, 548);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(204, 41);
            this.pauseButton.TabIndex = 4;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(617, 548);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(204, 41);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // captureTime
            // 
            this.captureTime.AutoSize = true;
            this.captureTime.Location = new System.Drawing.Point(982, 71);
            this.captureTime.Name = "captureTime";
            this.captureTime.Size = new System.Drawing.Size(0, 17);
            this.captureTime.TabIndex = 6;
            // 
            // screenShotPicture
            // 
            this.screenShotPicture.Cursor = System.Windows.Forms.Cursors.Cross;
            this.screenShotPicture.Location = new System.Drawing.Point(22, 12);
            this.screenShotPicture.Name = "screenShotPicture";
            this.screenShotPicture.Size = new System.Drawing.Size(805, 507);
            this.screenShotPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.screenShotPicture.TabIndex = 1;
            this.screenShotPicture.TabStop = false;
            this.screenShotPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.screenShotPicture_Paint);
            this.screenShotPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screenShotPicture_MouseDown);
            this.screenShotPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.screenShotPicture_MouseMove);
            this.screenShotPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.screenShotPicture_MouseUp);
            // 
            // decodeBtn
            // 
            this.decodeBtn.Location = new System.Drawing.Point(833, 478);
            this.decodeBtn.Name = "decodeBtn";
            this.decodeBtn.Size = new System.Drawing.Size(381, 41);
            this.decodeBtn.TabIndex = 7;
            this.decodeBtn.Text = "Crop |-> Decode";
            this.decodeBtn.UseVisualStyleBackColor = true;
            this.decodeBtn.Click += new System.EventHandler(this.decodeBtn_Click);
            // 
            // cropBtn
            // 
            this.cropBtn.Location = new System.Drawing.Point(1043, 12);
            this.cropBtn.Name = "cropBtn";
            this.cropBtn.Size = new System.Drawing.Size(185, 41);
            this.cropBtn.TabIndex = 8;
            this.cropBtn.Text = "Crop";
            this.cropBtn.UseVisualStyleBackColor = true;
            this.cropBtn.Click += new System.EventHandler(this.cropBtn_Click);
            // 
            // TargetPicBox
            // 
            this.TargetPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TargetPicBox.Location = new System.Drawing.Point(833, 115);
            this.TargetPicBox.Name = "TargetPicBox";
            this.TargetPicBox.Size = new System.Drawing.Size(402, 253);
            this.TargetPicBox.TabIndex = 9;
            this.TargetPicBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(833, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Capture Time: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(833, 416);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Mean Confidence: ";
            // 
            // meanConfidence
            // 
            this.meanConfidence.AutoSize = true;
            this.meanConfidence.Location = new System.Drawing.Point(982, 416);
            this.meanConfidence.Name = "meanConfidence";
            this.meanConfidence.Size = new System.Drawing.Size(0, 17);
            this.meanConfidence.TabIndex = 12;
            // 
            // YouTubeInsiderMediaPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 644);
            this.Controls.Add(this.meanConfidence);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TargetPicBox);
            this.Controls.Add(this.cropBtn);
            this.Controls.Add(this.YTplayer);
            this.Controls.Add(this.decodeBtn);
            this.Controls.Add(this.screenShotPicture);
            this.Controls.Add(this.captureTime);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.captureBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "YouTubeInsiderMediaPlayer";
            this.Text = "YouTubeInsiderMediaPlayer";
            this.Load += new System.EventHandler(this.YouTubeInsiderMediaPlayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.YTplayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenShotPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxShockwaveFlashObjects.AxShockwaveFlash YTplayer;
        private System.Windows.Forms.Button captureBtn;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label captureTime;
        private System.Windows.Forms.PictureBox screenShotPicture;
        private System.Windows.Forms.Button decodeBtn;
        private System.Windows.Forms.Button cropBtn;
        private System.Windows.Forms.PictureBox TargetPicBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label meanConfidence;
    }
}