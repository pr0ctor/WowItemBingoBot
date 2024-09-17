using DiscordBingoBot.BingoEngine;
using DiscordBingoBot.Model;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Reflection;
using System.Numerics;

namespace DiscordBingoBot.ImageGen
{
    internal static class ImageGen
    {

        public static async Task<FileStream> GetExistingBingoCard(BingoCard bingoCard)
        {
            var file = new FileStream($@"{EnvironmentVariables.BaseDirectoryPath}/images/computedcards/{bingoCard.Owner}/{bingoCard.SessionCode}.jpg", FileMode.Open);

            return file;
        }

        public static async Task<byte[]> GetExistingBingoCardAsByteArray(BingoCard bingoCard)
        {
            using var file = new FileStream($@"{EnvironmentVariables.BaseDirectoryPath}/images/computedcards/{bingoCard.Owner}/{bingoCard.SessionCode}.jpg", FileMode.Open);
            var streamLength = Convert.ToInt32(file.Length);
            var fileData = new byte[streamLength];
            file.Read(fileData, 0, streamLength);
            return fileData;

        }

        public static async Task<string> UpdateExistingBingoCardWithEntry(BingoCard bingoCard, GearItem submission)
        {
            var file = await GetExistingBingoCardAsByteArray(bingoCard);

            var layoutItems = bingoCard.Layout.Split(",").ToList();
            var submissionIndex = layoutItems.FindIndex(0, i => i == submission.ItemId.ToString());

            if(submissionIndex > 12) submissionIndex++;

            var submissionHorizontalOffset = submissionIndex / 5;
            var submissionVerticalOffset = submissionIndex % 5;

            var bigxPoint = BingoHelpers.bigxReference.GetNewPoint(submissionHorizontalOffset, submissionVerticalOffset);

            using var baseImage = Image.Load(file);
            using var bigxImage = Image.Load(new ReadOnlySpan<byte>(EnvironmentVariables.BigXData));

            baseImage.Mutate(o => o.DrawImage(bigxImage, new Point(bigxPoint.X, bigxPoint.Y), 1f));

            var location = $@"{EnvironmentVariables.BaseDirectoryPath}/images/computedcards/{bingoCard.Owner}/{bingoCard.SessionCode}.jpg";

            await baseImage.SaveAsJpegAsync(location);

            return location;
        }

        public static async Task<string> GenerateNewBingoCardImage(BingoCard bingoCard, List<string> itemNames, List<byte[]> imageIcons, byte[] bingoCardBase)
        {
            var iconPoints = BingoHelpers.iconReference.GetSquareOfPoints(5, 5, true);
            var textPoints = BingoHelpers.textReference.GetSquareOfPoints(5, 5, true);

            //var textFont = SystemFonts.CreateFont("Impact", 20f);
            var textFont = EnvironmentVariables.BingoTextFont.CreateFont(15.5f);

            using Image baseImage = Image.Load(new ReadOnlySpan<byte> (bingoCardBase));
            var iconCounter = 0;
            
            foreach(var row in iconPoints)
            {
                foreach(var cell in row)
                {
                    if (cell.X == -1 && cell.Y == -1) continue;
                    using Image icon = Image.Load(new ReadOnlySpan<byte>(imageIcons[iconCounter]));
                    baseImage.Mutate(o => o.DrawImage(icon, new Point(cell.X, cell.Y), 1.0f));
                    iconCounter++;
                }
            }

            var textCounter = 0;

            foreach(var row in textPoints)
            {
                foreach(var cell in row)
                {
                    if (cell.X == -1 && cell.Y == -1) continue;

                    RichTextOptions textOptions = new(textFont)
                    {
                        Origin = new Vector2(cell.X, cell.Y),
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        WrappingLength = 140
                    };

                    baseImage.Mutate(o => o.DrawText(textOptions, itemNames[textCounter], Brushes.Solid(Color.White), Pens.Solid(Color.Black, 1f)));
                    //baseImage.Mutate(o => o.DrawText(textOptions, itemNames[textCounter], Brushes.Solid(Color.Purple)/*, Pens.Solid(Color.Black, 2)*/));

                    textCounter++;
                }
            }
            RichTextOptions userInfoOptions = new RichTextOptions(textFont)
            {
                Origin = new Vector2(BingoHelpers.UserInformationPoint.X, BingoHelpers.UserInformationPoint.Y),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };
            baseImage.Mutate(o => o.DrawText(userInfoOptions, $"{bingoCard.Owner}-{bingoCard.SessionCode}", Color.Black));

            var finalPath = $@"{EnvironmentVariables.BaseDirectoryPath}/images/computedcards/{bingoCard.Owner}/{bingoCard.SessionCode}.jpg";

            Directory.CreateDirectory($@"{EnvironmentVariables.BaseDirectoryPath}/images/computedcards/{bingoCard.Owner}/");

            await baseImage.SaveAsJpegAsync(finalPath);

            return finalPath;
        }

    }
}
