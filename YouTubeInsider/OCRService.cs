using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouTubeInsider
{
    class OCRService
    {
        private static uint textFileCount = 1;

        internal static async Task<string> translate(string screenShotImage, Label meanConfidence, string videoName, ComboBox langBox)
        {
            string textPath = Constants.TextImagePath;
            List<string> textWords = new List<string>();

            bool autoDetect = false;
            string lang = "";

            if (langBox.SelectedIndex == 0)
            {
                autoDetect = true;
            }
            else
            {
                if (langBox.SelectedIndex == 1)
                {
                    lang = "cs";
                }
                else if (langBox.SelectedIndex == 2)
                {
                    lang = "java";
                }
                else if (langBox.SelectedIndex == 3)
                {
                    lang = "cpp";
                }
                else if (langBox.SelectedIndex == 4)
                {
                    lang = "c";
                }
                else if (langBox.SelectedIndex == 5)
                {
                    lang = "py";
                }
            }

            string tessText = TesseractTranslation.translate(screenShotImage, textWords, videoName);
            string msOcrText = await MSOCRPage.translate(screenShotImage, textWords, videoName);

            string linesToWrite = LanguageService.scoreOCR(tessText, msOcrText, textWords, autoDetect, videoName, ref lang, meanConfidence);

            if (langBox.SelectedIndex == 0)
            {
                if (lang.Equals("cs"))
                {
                    langBox.SelectedIndex = 1;
                }
                else if (lang.Equals("java"))
                {
                    langBox.SelectedIndex = 2;
                }
                else if (lang.Equals("cpp"))
                {
                    langBox.SelectedIndex = 3;
                }
                else if (lang.Equals("c"))
                {
                    langBox.SelectedIndex = 4;
                }
                else if (lang.Equals("py"))
                {
                    langBox.SelectedIndex = 5;
                }
            }

            textPath = textPath + textFileCount++.ToString() + "." + lang;
            System.IO.File.WriteAllText(textPath, linesToWrite);

            meanConfidence.Text = meanConfidence.Text + " ... Done";

            return textPath;
        }
    }
}
