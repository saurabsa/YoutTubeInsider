using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeInsider
{
    public partial class YouTubeInsiderMediaPlayer : Form
    {
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        private string videoId;
        private string videoName;
        public bool playState = false;
        public bool YTState = false;
        public bool YTFocus = false;

        Boolean bHaveMouse;
        Point ptOriginal = new Point();
        Point ptLast = new Point();
        Rectangle rectCropArea;

        public YouTubeInsiderMediaPlayer()
        {
            InitializeComponent();
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            this.screenShotPicture.Visible = false;
            this.bHaveMouse = false;
            this.cropBtn.Enabled = false;
            this.decodeBtn.Enabled = false;
            this.langBox.SelectedIndex = 0;
        }

        public async System.Threading.Tasks.Task Run()
        {
            // This OAuth 2.0 access scope allows for full read/write access to the
            // authenticated user's account and requires requests to use an SSL connection.
            /*UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] { YouTubeService.Scope.YoutubeForceSsl },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }*/

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAJze2R6uaM9kA_U8YnwVzJWo_hX3O2O3s",
                ApplicationName = this.GetType().ToString()
            });
            /*var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });*/
            
            // Call the YouTube Data API's captions.list method to
            // retrieve video caption tracks.
            var captionListRequest = youtubeService.Captions.List("snippet", videoId);
            CaptionListResponse captionListResponse = await captionListRequest.ExecuteAsync();

            // Print information from the API response.
            foreach (var caption in captionListResponse.Items)
            {
                Console.WriteLine(caption.Id);
                Console.WriteLine(caption.Kind);
                Console.WriteLine(caption.Snippet.Name);
                Console.WriteLine(caption.Snippet.Language);
            }
        }

        public YouTubeInsiderMediaPlayer(string videoId, string videoName)
        {
            this.videoId = videoId;
            this.videoName = videoName;
            InitializeComponent();
            InitializePlayer();

            //Run().Wait();
            this.YTplayer.Movie = "https://www.youtube.com/v/" + videoId + "?autoplay=1&enablejsapi=1&fs=1&loop=1&modestbranding=1&rel=0&showinfo=0&autohide=1&color=white&iv_load_policy=3&version=3&playerapiid=ytplayer";
            //?version =3&enablejsapi=1
            //?autoplay=1&enablejsapi=1&fs=1&loop=1&modestbranding=1&rel=0&showinfo=0&autohide=1&color=white&iv_load_policy=3&version=3&playerapiid=ytplayer
            //http://www.youtube.com/v/BboXNHDjhAM?autoplay=1&enablejsapi=1&fs=1&loop=1&modestbranding=1&rel=0&showinfo=0&autohide=1&color=white&iv_load_policy=3&theme=light&version=3
        }

        private static List<string> parseDelimitedString(string arguments, char delim = ',')
        {
            bool inQuotes = false;
            bool inNonQuotes = false;
            int whiteSpaceCount = 0;

            List<string> strings = new List<string>();

            StringBuilder sb = new StringBuilder();
            foreach (char c in arguments)
            {
                if (c == '\'' || c == '"')
                {
                    if (!inQuotes)
                        inQuotes = true;
                    else
                        inQuotes = false;

                    whiteSpaceCount = 0;
                }
                else if (c == delim)
                {
                    if (!inQuotes)
                    {
                        if (whiteSpaceCount > 0 && inQuotes)
                        {
                            sb.Remove(sb.Length - whiteSpaceCount, whiteSpaceCount);
                            inNonQuotes = false;
                        }
                        strings.Add(sb.Replace("'", string.Empty).Replace("\"", string.Empty).ToString());
                        sb.Remove(0, sb.Length);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    whiteSpaceCount = 0;
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (inNonQuotes || inQuotes)
                    {
                        sb.Append(c);
                        whiteSpaceCount++;
                    }
                }
                else
                {
                    if (!inQuotes) inNonQuotes = true;
                    sb.Append(c);
                    whiteSpaceCount = 0;
                }
            }
            strings.Add(sb.Replace("'", string.Empty).Replace("\"", string.Empty).ToString());
            return strings;
        }

        private string YTplayer_CallFlash(string ytFunction)
        {
            string flashXMLrequest = "";
            string response = "";
            string flashFunction = "";
            List<string> flashFunctionArgs = new List<string>();

            Regex func2xml = new Regex(@"([a-z][a-z0-9]*)(\(([^)]*)\))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match fmatch = func2xml.Match(ytFunction);

            if (fmatch.Captures.Count != 1)
            {
                Console.Write("bad function request string");
                return "";
            }

            flashFunction = fmatch.Groups[1].Value.ToString();
            flashXMLrequest = "<invoke name=\"" + flashFunction + "\" returntype=\"xml\">";
            if (fmatch.Groups[3].Value.Length > 0)
            {
                flashFunctionArgs = parseDelimitedString(fmatch.Groups[3].Value);
                if (flashFunctionArgs.Count > 0)
                {
                    flashXMLrequest += "<arguments><string>";
                    flashXMLrequest += string.Join("</string><string>", flashFunctionArgs);
                    flashXMLrequest += "</string></arguments>";
                }
            }
            flashXMLrequest += "</invoke>";

            try
            {
                Console.Write("YTplayer_CallFlash: \"" + flashXMLrequest + "\"\r\n");
                response = this.YTplayer.CallFunction(flashXMLrequest);
                Console.Write("YTplayer_CallFlash_response: \"" + response + "\"\r\n");
            }
            catch
            {
                Console.Write("YTplayer_CallFlash: error \"" + flashXMLrequest + "\"\r\n");
            }

            return response;
        }

        private void YTStateError(string error)
        {
            Console.Write("YTplayer_error: " + error + "\r\n");
        }

        private void YTStateChange(string YTplayState)
        {
            switch (int.Parse(YTplayState))
            {
                case -1: playState = false; break; //not started yet
                case 1: playState = true; break; //playing
                case 2: playState = false; break; //paused
                //case 3: ; break; //buffering
                case 0: playState = false;
                        YTplayer_CallFlash("seekTo(0)");
                    break; //ended
            }
        }

        private void YTplayer_Enter(object sender, EventArgs e)
        {
            YTFocus = true;
            //if(globalHotkeysToolStripMenuItem.Checked) deactivateGlobalHotkey(true);
        }

        private void YTplayer_Leave(object sender, EventArgs e)
        {
            YTFocus = false;
            //if (globalHotkeysToolStripMenuItem.Checked) activateGlobalHotkey(true);
        }

        private void YTready(string playerID)
        {
            YTState = true;

            //start eventHandlers
            YTplayer_CallFlash("addEventListener(\"onStateChange\",\"YTStateChange\")");
            YTplayer_CallFlash("addEventListener(\"onError\",\"YTError\")");
        }

        private void YTplayer_FSCommand(object sender, AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEvent e)
        {
            Console.Write("YTplayer_FSCommand: " + e.command.ToString() + "(" + e.args.ToString() + ")" + "\r\n");
        }

        private void YTplayer_FlashCall(object sender, AxShockwaveFlashObjects._IShockwaveFlashEvents_FlashCallEvent e)
        {
            Console.Write("YTplayer_FlashCall: raw: "+e.request.ToString()+"\r\n");
            // message is in xml format so we need to parse it
            XmlDocument document = new XmlDocument();
            document.LoadXml(e.request);
            // get attributes to see which command flash is trying to call
            XmlAttributeCollection attributes = document.FirstChild.Attributes;
            String command = attributes.Item(0).InnerText;
            // get parameters
            XmlNodeList list = document.GetElementsByTagName("arguments");
            List<string> listS = new List<string>();
            foreach (XmlNode l in list)
            {
                listS.Add(l.InnerText);
            }
            //Console.Write("YTplayer_FlashCall: \"" + command.ToString() + "(" + string.Join(",", listS) + ")\r\n");
            // Interpret command
            switch (command)
            {
                case "onYouTubePlayerReady": YTready(listS[0]); break;
                case "YTStateChange": YTStateChange(listS[0]); break;
                case "YTError": YTStateError(listS[0]); break;
                case "document.location.href.toString": this.YTplayer.SetReturnValue("<string>http://www.youtube.com/watch?v=" + this.videoId + "</string>"); break;
                default: Console.Write("YTplayer_FlashCall: (unknownCommand)\r\n"); break;
            }
        }

        private void mediaPlay()
        {
            Console.Write(string.Format("Media: Play\n"));
            //YTplayer_CallFlash("setOptions('cc','reload','true')");
            YTplayer_CallFlash("playVideo()");
            playState = true;

            this.cropBtn.Enabled = false;
            this.decodeBtn.Enabled = false;
        }

        private void mediaPause()
        {
            Console.Write(string.Format("Media: Pause\n"));
            YTplayer_CallFlash("pauseVideo()");
            playState = false;
        }

        private void mediaStop()
        {
            Console.Write(string.Format("Media: Stop\n"));
            YTplayer_CallFlash("stopVideo()");
            playState = false;

            this.cropBtn.Enabled = false;
            this.decodeBtn.Enabled = false;
        }

        private void mediaPlayPause()
        {
            if (playState == true)
            {
                mediaPause();
            }
            else
            {
                mediaPlay();
            }
        }

        private void mediaRestart()
        {
            Console.Write(string.Format("Media: Restart\n"));
        }

        private Image CaptureScreen()
        {
            Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            Bitmap target = new Bitmap(screenSize.Width, screenSize.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(screenSize.Width, screenSize.Height));
            }
            return target;
        }

        private void captureButton_Click(object sender, EventArgs e)
        {
            mediaPause();
            captureTime.Text = "";
            meanConfidence.Text = "";
            // slurp the xml into an XmlDocument
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(YTplayer_CallFlash("getCurrentTime()"));
            XmlNode current = xml.SelectSingleNode(@"/number");
            double result = Convert.ToDouble(current.FirstChild.InnerText) / 100.00;
            captureTime.Text = result.ToString();

            Graphics g = YTplayer.CreateGraphics();
            resetRectangle();
            //g.Clear(Color.White);
            Bitmap bmp = new Bitmap(YTplayer.Size.Width, YTplayer.Size.Height, g);
            Graphics memoryGraphics = Graphics.FromImage(bmp);
            IntPtr dc = memoryGraphics.GetHdc();
            bool success = PrintWindow(YTplayer.Handle, dc, 0);
            memoryGraphics.ReleaseHdc(dc);
            enableScreenShotPicture(bmp);
            //TesseractTranslation.translate(screenShotImage);
        }

        private void enableScreenShotPicture(Bitmap bmp)
        {
            this.screenShotPicture.Image = bmp;
            this.screenShotPicture.Visible = true;
            this.screenShotPicture.BringToFront();

            this.cropBtn.Enabled = true;
            this.decodeBtn.Enabled = true;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            mediaPlay();
            captureTime.Text = "";
            meanConfidence.Text = "";
            this.screenShotPicture.Visible = false;
            this.screenShotPicture.SendToBack();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            mediaPause();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            mediaStop();
        }

        private async void decodeBtn_Click(object sender, EventArgs e)
        {
            if (rectCropArea.Width == 0 || rectCropArea.Height == 0) return;
            Bitmap targetImage = crop();
            meanConfidence.Text = "Decoding ...";
            string screenShotImage = Constants.ScreenShotImagePath;

            targetImage.Save(screenShotImage);

            targetImage.Dispose();

            string textPath = Constants.TextImagePath;
            textPath = await OCRService.translate(screenShotImage, meanConfidence, videoName, langBox);

            try
            {
                var dte = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.14.0");
                if (dte == null)
                {
                    Type type = Type.GetTypeFromProgID("VisualStudio.DTE.14.0");
                    dte = (EnvDTE80.DTE2)Activator.CreateInstance(type);
                    dte.MainWindow.Visible = true;
                }
                dte.Documents.Open(textPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cropBtn_Click(object sender, EventArgs e)
        {
            if (rectCropArea.Width == 0 || rectCropArea.Height == 0) return;
            crop();
        }

        private Bitmap crop()
        {
            TargetPicBox.Refresh();
            //Prepare a new Bitmap on which the cropped image will be drawn
            Bitmap sourceBitmap = new Bitmap(screenShotPicture.Image, screenShotPicture.Width, screenShotPicture.Height);
            Graphics g = TargetPicBox.CreateGraphics();

            Bitmap newBitmap = new Bitmap(TargetPicBox.Width, TargetPicBox.Height);

            int x = 0, y = 0, width = 0, height = 0;
            if(rectCropArea.Width >= TargetPicBox.Width)
            {
                x = 0;
                width = TargetPicBox.Width;
            }
            else
            {
                x = (TargetPicBox.Width - rectCropArea.Width)/2;
                width = rectCropArea.Width;
            }

            if (rectCropArea.Height >= TargetPicBox.Height)
            {
                y = 0;
                height = TargetPicBox.Height;
            }
            else
            {
                y = (TargetPicBox.Height - rectCropArea.Height) / 2;
                height = rectCropArea.Height;
            }

            //Draw the image on the Graphics object with the new dimesions
            g.DrawImage(sourceBitmap, new Rectangle(x, y, width, height),
                rectCropArea, GraphicsUnit.Pixel);

            System.Drawing.Imaging.PixelFormat format = sourceBitmap.PixelFormat;
            Bitmap bmp = sourceBitmap.Clone(rectCropArea, format);
            //bmp.Save("tst.jpeg", bmp.RawFormat);

            //Good practice to dispose the System.Drawing objects when not in use.
            sourceBitmap.Dispose();

            return bmp;
        }

        private void screenShotPicture_MouseDown(object sender, MouseEventArgs e)
        {
            // Make a note that we "have the mouse".
            bHaveMouse = true;

            // Store the "starting point" for this rubber-band rectangle.
            ptOriginal.X = e.X;
            ptOriginal.Y = e.Y;

            // Special value lets us know that no previous
            // rectangle needs to be erased.

            ptLast.X = -1;
            ptLast.Y = -1;

            rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
        }

        private void screenShotPicture_MouseUp(object sender, MouseEventArgs e)
        {
            // Set internal flag to know we no longer "have the mouse".
            bHaveMouse = false;

            // If we have drawn previously, draw again in that spot
            // to remove the lines.
            if (ptLast.X != -1)
            {
                Point ptCurrent = new Point(e.X, e.Y);
            }
            else
            {
                Point ptCurrent = new Point(e.X, e.Y);

                if (ptOriginal.X == ptCurrent.X && ptOriginal.Y == ptCurrent.Y)
                {
                    resetRectangle();
                }
            }

            // Set flags to know that there is no "previous" line to reverse.
            ptLast.X = -1;
            ptLast.Y = -1;
            ptOriginal.X = -1;
            ptOriginal.Y = -1;
        }

        private void resetRectangle()
        {
            rectCropArea.Width = 0;
            rectCropArea.Height = 0;

            rectCropArea.X = ptOriginal.X;
            rectCropArea.Y = ptOriginal.Y;
            screenShotPicture.Refresh();
        }

        private void screenShotPicture_MouseMove(object sender, MouseEventArgs e)
        {
            Point ptCurrent = new Point(e.X, e.Y);

            // If we "have the mouse", then we draw our lines.
            if (bHaveMouse)
            {
                // If we have drawn previously, draw again in
                // that spot to remove the lines.

                // Update last point.
                ptLast = ptCurrent;

                // Draw new lines.

                // e.X - rectCropArea.X;
                // normal
                if (e.X > ptOriginal.X && e.Y > ptOriginal.Y)
                {
                    rectCropArea.Width = e.X - ptOriginal.X;

                    // e.Y - rectCropArea.Height;
                    rectCropArea.Height = e.Y - ptOriginal.Y;
                }
                else if (e.X < ptOriginal.X && e.Y > ptOriginal.Y)
                {
                    rectCropArea.Width = ptOriginal.X - e.X;
                    rectCropArea.Height = e.Y - ptOriginal.Y;
                    rectCropArea.X = e.X;
                    rectCropArea.Y = ptOriginal.Y;
                }
                else if (e.X > ptOriginal.X && e.Y < ptOriginal.Y)
                {
                    rectCropArea.Width = e.X - ptOriginal.X;
                    rectCropArea.Height = ptOriginal.Y - e.Y;

                    rectCropArea.X = ptOriginal.X;
                    rectCropArea.Y = e.Y;
                }
                else
                {
                    rectCropArea.Width = ptOriginal.X - e.X;

                    // e.Y - rectCropArea.Height;
                    rectCropArea.Height = ptOriginal.Y - e.Y;
                    rectCropArea.X = e.X;
                    rectCropArea.Y = e.Y;
                }
                screenShotPicture.Refresh();
            }
        }

        private void screenShotPicture_Paint(object sender, PaintEventArgs e)
        {
            Pen drawLine = new Pen(Color.Red);
            drawLine.DashStyle = DashStyle.Dash;
            e.Graphics.DrawRectangle(drawLine, rectCropArea);
        }

        private void YouTubeInsiderMediaPlayer_Load(object sender, EventArgs e)
        {

        }
    }

    public static class BindingListExtensions
    {
        public static void RemoveAll<T>(this BindingList<T> list, Func<T, bool> predicate)
        {
            // first check predicates -- uses System.Linq
            // could collapse into the foreach, but still must use 
            // ToList() or ToArray() to avoid deferred execution
            var toRemove = list.Where(predicate).ToList();

            // then loop and remove after
            foreach (var item in toRemove)
            {
                list.Remove(item);
            }
        }
    }
}
