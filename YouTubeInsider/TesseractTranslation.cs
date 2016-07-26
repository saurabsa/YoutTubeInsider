﻿using System;
using System.Diagnostics;
using Tesseract;

namespace YouTubeInsider
{
    class TesseractTranslation
    {
        public static void translate(string imagePath, string textPath)
        {
            string linesToWrite = "";
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            linesToWrite += "Mean confidence: " + page.GetMeanConfidence() + "\n";
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
                            linesToWrite += "Text (GetText): \r\n\n" + text;
                            Console.WriteLine("Text (GetText): \r\n{0}", text);
                            linesToWrite += "Text (iterator):\n";
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
                                                    linesToWrite += "<BLOCK>\n";
                                                    Console.WriteLine("<BLOCK>");
                                                }

                                                linesToWrite += iter.GetText(PageIteratorLevel.Word) + ".";
                                                Console.Write(iter.GetText(PageIteratorLevel.Word));
                                                Console.Write(" ");

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                    linesToWrite += "\n";
                                                    Console.WriteLine();
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                linesToWrite += "\n";
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                        }
                    }
                }
                System.IO.File.WriteAllText(textPath, linesToWrite);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
            //Console.Write("Press any key to continue . . . ");
            //Console.ReadKey(true);
        }
    }
}
