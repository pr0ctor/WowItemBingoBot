using DiscordBingoBot.AutoCompleteProviders;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Commands.Converters;
using DiscordBingoBot.ParameterChecks;
using DSharpPlus.Entities;
using DiscordBingoBot.Model;
using DiscordBingoBot.OCR;
using System.Net;
using DiscordBingoBot.BingoEngine;
using DiscordBingoBot.ImageGen;
using DiscordBingoBot.ChoiceProviders;
using DiscordBingoBot.ContextChecks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DiscordBingoBot.Commands
{
    internal class SubmitCommand
    {
        [Command("submit")]
        [Description("Submit an item for your Bingo Card")]
#if !DEBUG
        [DiscordRoleAttribute("TeamGreenRole")]
#endif
        public async ValueTask SubmitItem(
            SlashCommandContext context,
            [Description("The name of the wow character. If this is you, please enter \"Me\". Must be 2->12 letters."),
                SlashAutoCompleteProvider<WoWCharacterNameAutoCompleteProvider>,
                WoWCharacterName] string characterName,
            [Description("Name of the item to submit for checking."),
                SlashAutoCompleteProvider<GearNameAutoCompleteProvider>,
                GearName] string itemName,
            [ParameterAttribute("chat_image"),
                Description("Image of chat that shows that someone who is in your group and guild was awarded this item."),
                FilenameFiletype(".png,.jpg,.jpeg")] DiscordAttachment imageAttachment,
            [ParameterAttribute("show_card"),
                Description("Do you want to see the new card (if not a bingo)?"),
                SlashChoiceProvider<YesNoChoiceProvider>] int showCard 
        )
        {

            await context.DeferResponseAsync(ephemeral: false);

            var fileUrl = imageAttachment.Url;
            var discordUser = new User(context.Member);
            //var gearItem = EnvironmentVariables.GearItemCacheByItemName[itemName];
            var gearItem = EnvironmentVariables.GearItemCacheByItemId[int.Parse(itemName)];
            var characterIsCurrentUser = (characterName.Trim().Equals("Me", StringComparison.OrdinalIgnoreCase))
                ? true
                : false;

            // check if registered

            var isRegistered = await Database.CheckUserRegistration(discordUser);

            if(!isRegistered)
            {
                await context.EditResponseAsync(Messages.UserNotRegistered);
                return;
            }

            var wowCharacterId = 1;
            if(!characterIsCurrentUser)
            {
                wowCharacterId = await Database.RegisterOrRetrieveWoWCharacter(characterName);
            }

            // get bingo card and submissions

            var bingoCard = await Database.GetActiveBingoCardForUser(discordUser);

            var listOfValidGearItems = bingoCard.Layout.Split(",");

            var submittedItemIsOnCard = listOfValidGearItems.Contains(gearItem.ItemId.ToString());

            if (!submittedItemIsOnCard)
            {
                await context.EditResponseAsync(Messages.SubmittedItemIsNotOnCard);
                return;
            }

            var currentSubmissions = await Database.RetrieveSubmissionsForActiveBingoCard(discordUser, bingoCard);

            // check to see if this item has already been submitted

            if (currentSubmissions.Contains(gearItem.ItemId))
            {
                await context.EditResponseAsync(Messages.ItemAlreadySubmitted);
                return;
            }

            // download image from the message

            //using var httpClient = new HttpClient();

            //using var filedataStream = await httpClient.GetStreamAsync(fileUrl);
            //var filedataStream = await httpClient.GetByteArrayAsync(fileUrl);

            //var modifiedFile = ProcessUploadedImage(filedataStream);

            // parse uploaded image

            //var imageText = OcrWorker.ReadTextFromImage(modifiedFile);

            //var foundItemAward = OcrWorker.FoundMatchingGearAquisition(imageText, (characterIsCurrentUser) ? "You" : characterName, gearItem);

            /*if(!foundItemAward)
            {
                await context.EditResponseAsync(Messages.ItemNotFoundInScreenshot);
                return;
            }*/

            // updated board state

            await Database.CreateBingoCardSubmission(discordUser, bingoCard, wowCharacterId, gearItem, fileUrl ?? "");

            // evaluate win condition

            var completedCard = await BingoHelpers.ValidateBingoCard(discordUser, bingoCard);

            FileStream bingoCardData;
            string messageData = "";
            if(completedCard)
            {
                // create completion and update existing card

                await Database.CompleteBingoCard(discordUser, bingoCard);

                //generate new card and return to the user
                var data = await BingoHelpers.GenerateNewBingoCard(discordUser);
                bingoCardData = new(data.cardUri, FileMode.Open);

                messageData = Messages.SuccessfullyCompleteABingoCard;
                showCard = 1;
            }
            else
            {
                var cardLocation = await ImageGen.ImageGen.UpdateExistingBingoCardWithEntry(bingoCard, gearItem);
                bingoCardData = await ImageGen.ImageGen.GetExistingBingoCard(bingoCard);
                messageData = Messages.SuccessfulSubmission;
            }

            // send response back with messages and/or with new bingo card
            var finalMessage = (showCard == 1) 
                ? new DiscordMessageBuilder().WithContent(messageData).AddFile(bingoCardData)
                : new DiscordMessageBuilder().WithContent(messageData);

            await context.EditResponseAsync(finalMessage);
            bingoCardData.Dispose();
        }


        private byte[] ProcessUploadedImage(byte[] image)
        {

            var ratio = 300 / 72;
            using var picture = Image.Load(image);

            picture.Mutate(o => o.Resize(picture.Width * ratio, picture.Height * ratio, KnownResamplers.Lanczos8));

            using var newstream = new MemoryStream();
            picture.SaveAsPng(newstream);

            return newstream.ToArray();
        }
    }
}
