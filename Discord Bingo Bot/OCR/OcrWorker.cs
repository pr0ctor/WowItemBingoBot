using DiscordBingoBot.Algorithms;
using DiscordBingoBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace DiscordBingoBot.OCR
{
    internal static class OcrWorker
    {

        private static int LevenshteinMaxErrors = 6;

        public static string ReadTextFromImage(byte[] imageStream)
        {
            var engine = new TesseractEngine(EnvironmentVariables.BaseDirectoryPath + $@"/tessdata/", "eng");

            var image = Pix.LoadFromMemory(imageStream);

            var engineResult = engine.Process(image);
#if DEBUG
            Console.WriteLine(engineResult.GetText());
#endif

            return engineResult.GetText();
        }


        /*public static string ReadTextFromImage(Stream imageStream)
        {
            //var ocr = new IronTesseract();

            ///using var ocrInput = new OcrInput();
            ocrInput.LoadImage(imageStream);
            //ocrInput.LoadPdf("document.pdf");

            // Optionally Apply Filters if needed:
            // ocrInput.Deskew();  // use only if image not straight
            // ocrInput.DeNoise(); // use only if image contains digital noise

            var ocrResult = ocr.Read(ocrInput);
            Console.WriteLine(ocrResult.Text);
            return ocrResult.Text;
        }*/

        public static bool FoundMatchingGearAquisition(string sourceText, string characterName, GearItem item)
        {
            var itemNameRegex = $@"{item.ItemName.Replace(" ", $@"\s+")}";

            var messageRegex = new Regex(@$"{characterName}[\p{{L}}\d-]* receives* loot: [\s\[\]l]*{itemNameRegex}[\]l].*");

            //var match = Regex.Match(sourceText, messageRegex);
            var match = messageRegex.Match(sourceText.Trim());

            if (match.Success)
            {
                return true;
            }

            var perfectCase = @$"{characterName} receive{((characterName.Equals("Me")) ? "" : "s" )} loot: [{item.ItemName}].";

            var condensedSource = sourceText.Replace("\r", "").Replace("\n", " ").Trim();

            var levenshteinDistance = LevenshteinDistance.Calculate(perfectCase, condensedSource);

            var lengthDifference = Math.Abs(perfectCase.Length - condensedSource.Length);

            var allowableErrors = lengthDifference + LevenshteinMaxErrors;

            if (levenshteinDistance <= allowableErrors)
            {
                return true;
            }

            return false;
        }
    }
}
