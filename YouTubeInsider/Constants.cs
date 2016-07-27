using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeInsider
{
    public class Constants
    {
        public static readonly string YouTubeInsiderIconUrl = "http://findicons.com/icon/download/131963/find/128/png";
        public static readonly string TessDataUrl = "https://github.com/tesseract-ocr/tessdata/archive/3.04.00.zip";
        private static readonly string DocumentsFolderPath = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents");
        public static readonly string VisualStudioFolderPath = Path.Combine(DocumentsFolderPath, "Visual Studio 2015");
        public static readonly string TessDataFolderPath = Path.Combine(VisualStudioFolderPath, "tessdata");
        public static readonly string ProjectsFolderPath = Path.Combine(VisualStudioFolderPath, "Projects");
        public static readonly string ScreenShotImagePath = Path.Combine(ProjectsFolderPath, "screenShot.jpeg");
        public static readonly string TextImagePath = Path.Combine(ProjectsFolderPath, "content");
    }
}
