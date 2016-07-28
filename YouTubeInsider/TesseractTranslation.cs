using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Tesseract;

namespace YouTubeInsider
{
    class TesseractTranslation
    {
        internal static string translate(string imagePath, List<string> textWords, string videoName)
        {
            string linesToWrite = "";
            string meanConfidenceValue = "";
            try
            {
                using (var engine = new TesseractEngine(Constants.TessDataFolderPath, "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            //linesToWrite += "Mean confidence: " + page.GetMeanConfidence() + "\n";
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
                            meanConfidenceValue = page.GetMeanConfidence().ToString();
                            //linesToWrite += "Text (GetText): \r\n\n" + text;
                            linesToWrite = text;
                            Console.WriteLine("Text (GetText): \r\n{0}", text);
                            //linesToWrite += "Text (iterator):\n";
                            Console.WriteLine("Text (iterator):");
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();
                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            do
                                            {
                                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                {
                                                    //linesToWrite += "<BLOCK>\n";
                                                    Console.WriteLine("<BLOCK>");
                                                }

                                                string word = iter.GetText(PageIteratorLevel.Word);
                                                //linesToWrite += iter.GetText(PageIteratorLevel.Word) + ".";
                                                textWords.Add(word);
                                                Console.Write(word);
                                                Console.Write(" ");

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                    //linesToWrite += "\n";
                                                    Console.WriteLine();
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                //linesToWrite += "\n";
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                        }
                    }
                }

                //textPath = textPath + textFileCount++.ToString() + "." + LanguageService.analyzeType(textWords, videoName);
                //System.IO.File.WriteAllText(textPath, linesToWrite);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
                MessageBox.Show(e.ToString());
            }
            return linesToWrite;
        }
    }
}
