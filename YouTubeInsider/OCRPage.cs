using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace YouTubeInsider
{
    class OCRPage
    {
        private static uint textFileCount = 1;

        /// <summary>
        /// Uploads the image to Project Oxford and performs OCR
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <param name="language">The language code to recognize for</param>
        /// <returns></returns>
        private static async Task<OcrResults> UploadAndRecognizeImage(string imageFilePath, string language)
        {
            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient("130199dd1a3145839d19c05916fb3412");
            Console.WriteLine("VisionServiceClient is created");

            using (Stream imageFileStream = File.OpenRead(imageFilePath))
            {
                //
                // Upload an image and perform OCR
                //
                Console.WriteLine("Calling VisionServiceClient.RecognizeTextAsync()...");
                OcrResults ocrResult = await VisionServiceClient.RecognizeTextAsync(imageFileStream, language);
                return ocrResult;
            }
        }

        public static async Task<string> translate(string imagePath, string textPath, Label meanConfidence, string videoName)
        {
            string languageCode = "en";
            string meanConfidenceValue = "";
            List<string> textWords = new List<string>();

            //
            // Either upload an image, or supply a url
            //
            OcrResults ocrResult = await UploadAndRecognizeImage(imagePath, languageCode);

            StringBuilder stringBuilder = new StringBuilder();

            if (ocrResult != null && ocrResult.Regions != null)
            {
                //stringBuilder.Append("Text: ");
                //stringBuilder.AppendLine();
                foreach (var item in ocrResult.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            textWords.Add(word.Text);
                            stringBuilder.Append(" ");
                        }

                        stringBuilder.AppendLine();
                    }

                    stringBuilder.AppendLine();
                }
            }

            Console.WriteLine(stringBuilder.ToString());
            textPath = textPath + textFileCount++.ToString() + "." + LanguageService.analyzeType(textWords, videoName);

            System.IO.File.WriteAllText(textPath, stringBuilder.ToString());
            meanConfidence.Text = meanConfidenceValue + " ... Completed";

            return textPath;
        }

        protected void LogOcrResults(OcrResults results)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (results != null && results.Regions != null)
            {
                stringBuilder.Append("Text: ");
                stringBuilder.AppendLine();
                foreach (var item in results.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            stringBuilder.Append(" ");
                        }

                        stringBuilder.AppendLine();
                    }

                    stringBuilder.AppendLine();
                }
            }

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}
