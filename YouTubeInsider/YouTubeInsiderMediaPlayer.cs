using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MouseKeyboardActivityMonitor;

namespace YouTubeInsider
{
    public partial class YouTubeInsiderMediaPlayer : Form
    {
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        private string videoId;
        private readonly KeyboardHookListener m_KeyboardHookManager;
        public bool playState = false;
        public bool YTState = false;
        public bool YTFocus = false;

        public YouTubeInsiderMediaPlayer()
        {
            InitializeComponent();
        }

        public YouTubeInsiderMediaPlayer(string videoId)
        {
            this.videoId = videoId;
            InitializeComponent();
            this.YTplayer.Movie = "https://www.youtube.com/v/" + videoId + "?autoplay=1&enablejsapi=1&fs=1&loop=1&modestbranding=1&rel=0&showinfo=0&autohide=1&color=white&iv_load_policy=3&version=3&playerapiid=ytplayer";
            //?version =3&enablejsapi=1
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
            //Console.Write("YTplayer_FlashCall: raw: "+e.request.ToString()+"\r\n");
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
            YTplayer_CallFlash("playVideo()");
            //YTplayer_CallFlash("setOptions('cc','reload','true')");
            playState = true;
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

        private void convertButton_Click(object sender, EventArgs e)
        {
            mediaPause();
            captureTime.Text = YTplayer_CallFlash("getCurrentTime()");

            string screenShotImage = "screen.jpeg";
            Graphics g = YTplayer.CreateGraphics();
            Bitmap bmp = new Bitmap(YTplayer.Size.Width, YTplayer.Size.Height, g);
            Graphics memoryGraphics = Graphics.FromImage(bmp);
            IntPtr dc = memoryGraphics.GetHdc();
            bool success = PrintWindow(YTplayer.Handle, dc, 0);
            memoryGraphics.ReleaseHdc(dc);
            //bmp.Save("screen.jpeg", ImageFormat.Jpeg);
            bmp.Save(screenShotImage, bmp.RawFormat);
            bmp.Dispose();

            TesseractTranslation.translate(screenShotImage);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            mediaPlay();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            mediaPause();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            mediaStop();
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
