using BingoEngine.Core.Patterns;
using BingoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DiscordBingoBot.Model;
using BingoEngine.Core.Boards;
using System.Drawing;

namespace DiscordBingoBot.BingoEngine
{
    internal static class BingoHelpers
    {
        public static Point UserInformationPoint { get; set; }
        public static PointGridLocations iconReference { get; set; }
        public static PointGridLocations textReference { get; set; }
        public static PointGridLocations bigxReference { get; set; }

        public static void InitializeBingoEngine()
        {
            EnvironmentVariables.BingoPattern = new StandardPattern();

            UserInformationPoint = new Point(100, 975);

            iconReference = new PointGridLocations
            {
                startingPoint = new Point( 150 , 160 ),
                horizontalOffset = 160,
                verticalOffset = 160,
            };

            textReference = new PointGridLocations
            {
                startingPoint = new Point( 180 , 260 ),
                horizontalOffset = 160,
                verticalOffset = 160,
            };

            bigxReference = new PointGridLocations
            {
                startingPoint = new Point( 100 , 150 ),
                horizontalOffset = 160,
                verticalOffset = 160,
            };
        }

        public static async Task<(string cardUri, BingoCard BingoCard)> GenerateNewBingoCard(User user)
        {
            // generate session code for new bingo card
            var sessionCode = $"{user.UserSnowflake}{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}";

            // generate new bingo card

            var standardCard = new StandardBoard(sessionCode);
            standardCard.BingoCard.Owner = user.UserSnowflake;

            // save card to database

            await Database.CreateNewBingoCard(standardCard.BingoCard);

            // generate card image

            var rawItemList = standardCard.BingoCard.Layout.Split(',').ToList();

            var gearItems = rawItemList.Select(i => EnvironmentVariables.GearItemCacheByItemId[int.Parse(i)]).ToList();

            var itemNames = gearItems.Select(i => i.ItemName).ToList();
            var itemIcons = gearItems.Select(i => EnvironmentVariables.GeatItemCacheForImageData[i.ItemImageName]).ToList();

            var chosenBingoCardBase = EnvironmentVariables.BingoCardBaseImageCache[$"{EnvironmentVariables.random.Next(0, EnvironmentVariables.BingoCardBaseImageCache.Values.Count)}.jpg"];

            var bingoCardData = await ImageGen.ImageGen.GenerateNewBingoCardImage(standardCard.BingoCard, itemNames, itemIcons, chosenBingoCardBase);

            return (
                cardUri: bingoCardData,
                BingoCard: standardCard.BingoCard
            );
        }

        public static StandardBoard GenerateBingoEngineBoard(BingoCard card)
        {
            return new StandardBoard(card.SessionCode, card.BoardStateCode);
        }

        public static async Task<bool> ValidateBingoCard(User user, BingoCard card)
        {
            var submittedItems = await Database.RetrieveSubmissionsForActiveBingoCard(user, card) ?? new();

            var winCondition = StandardBoardJudge.Evaluate(card.SessionCode, card.BoardStateCode, EnvironmentVariables.BingoPattern, submittedItems.ToArray());

            return winCondition != null;
        }


        public static IBingoPattern[] GetPatterns()
        {
            // dynamically parse all of the classes that implement 
            // the IBingoPattern interface
            var patternInterface = typeof(IBingoPattern);
            var assembly = Assembly.GetAssembly(patternInterface);

            // this shouldn't ever be null, but handled anyway
            if (assembly == null)
            {
                return Array.Empty<IBingoPattern>();
            }

            // exclude the BasePattern, which is an abstract class and cannot be instantiated
            Type[] types = assembly.GetTypes()
                .Where(x => x.GetInterfaces().Contains(patternInterface))
                .Where(x => !x.IsAbstract)
                .ToArray();

            // I don't know how to write this so that we can guarantee to the compiler
            // that the instances won't be null, so I null coalesce to a StandardPattern
            // instance even though I don't think that will ever happen
            var patterns = types.Select(x => Activator.CreateInstance(x) as IBingoPattern ?? new StandardPattern());

            return patterns.ToArray();
        }

    }
}
