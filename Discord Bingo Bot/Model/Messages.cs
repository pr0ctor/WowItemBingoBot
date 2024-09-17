using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Model
{
    internal static class Messages
    {
        #region Error Messages

        public static readonly string ItemNotFoundInScreenshot = "The given character or item award was not found in the submitted screenshot.";
        public static readonly string InvalidStringParameter = "The provided value was not a string.";
        public static readonly string InvalidDiscordAttachmentParameter = "The provided value was not a discord attachment.";
        public static readonly string InvalidDiscordRoleAssigned = "Cannot preform this operation. Insufficient permissions.";
        public static readonly string WowCharacterAlreadyRegistered = "This WoW Character has already been registered";
        public static readonly string UserAlreadyRegistered = "You are already registered.";
        public static readonly string UserNotRegistered = "You are not registered. You must first register in order to submit.";
        public static readonly string SubmittedItemIsNotOnCard = "That item is not on your current card.";

        public static string IncorrectGearNameEntry(string gearName) => $"The given Item Name '{gearName}' is not a valid selection.";
        public static string IncorrectWowCharacterNameLength(string wowCharacterName) => $"The given Character Name '{wowCharacterName}' is {((wowCharacterName.Length > 12) ? "too long" : "too short")}. Please enter a valid name.";
        public static string IncorrectWowCharacterNameSymbols(string wowCharacterName) => $"The given Character Name '{wowCharacterName}' contains invalid characters. Please enter a valid name.";

        public static string InvalidFileTypeUploaded(string uploadedFileName, List<string> acceptedTypes) => $"The uploaded file '{uploadedFileName}' cannot be accepted. Only files of types: '{string.Join(", ", acceptedTypes)}' can be accepted.";

        #endregion


        #region Success Messages

        public static readonly string SuccessfulDiscordUserRegistration = "You have been registered! Your first bingo card has been generated:";

        public static readonly string ItemAlreadySubmitted = "You have already successfully submitted this item.";

        public static readonly string SuccessfullyCompleteABingoCard = "Congrats, you have completed the previous Bingo Card! A new one has been generated for you:";
        public static readonly string SuccessfulSubmission = "Congrats, this is a new addition to your Bingo Card! Your progress has been recorded.";

        public static readonly string SuccessfulCardRetrieval = "Here is your Bingo Card:";

        public static readonly string NoRecordedRankings = "No recorded rankings yet.";

        public static string SuccessfulWowCharacterNameRegistration(string wowCharacterName, string discordName) => $"The given Character Name '{wowCharacterName}' has been registered by {discordName}.";

        #endregion

        #region Leaderboard Messages

        public static string TotalUserSubmissions(int userSubmissions) => $"You have submitted  { userSubmissions }  item{ ((userSubmissions == 1) ? "" : "s") }.";
        
        public static string TotalUserCompletions(int userCompletions) => $"You have completed  { userCompletions }  card{ ((userCompletions == 1) ? "" : "s") }.";

        public static string TotalUserSubmissionsForCard(int userSubmissions) => $"You have submitted  {userSubmissions}  item{((userSubmissions == 1) ? "" : "s")} for your current Bingo Card.";

        #endregion

        #region Help Messages

        public static readonly string HelpInformationMessage = $@"The bot has serveral commands:
## Register Commands

These commands allow for users to regsiter for the game and load names for World of Warcraft characters to be selected from.
- `/register me` : This command registers you in order to join the bingo game. Required to be run to join and generates a new bingo card on registration. Names must be between 2-12 characters long and cannot include numbers, special characters, etc.
- `/register character` : This command logs a character name for a World of Warcraft character. This is used for auto-completion search and verification.

## Submit Command

The submit command allows you to submit an item to be checked to mark an item off on just your bingo card. The command requires serveral things in order to be correctly processed:
`/submit`
- `character_name` : The name of World of Warcraft character that received the item. If that character was you, use `Me` instead.
- `item_name` : The name of the item that was received. The name must match the name exactly and can be chosen from the auto-complete list.
- `chat_image` : The image of the in-game chat that shows that the item was awarded to a player. Don't submit a screenshot of your entire screen or screenshots containing sensitive data.
- `show_card` : Whether or not to receive an updated image of the card after a successful submission.

## View Commands

These commands allow for various things to be viewed.
- `/view bingocard` : Retreives an image of the current state of your card.
- `/view leaderboard` : Retrieves the current state of the overall leaderboard for the current session.
- `/view mystats` : Retreives the various statistics for you for the current session.

## Help Commands

- `/help info` : Info regarding the different commands.
- `/help namespreadsheet` : Returns the link to the spreadsheet that contains a list of World of Warcraft characters.

";

        public static string HelpNameSpreadsheetMessage(string spreadsheetUrl) => $@"Here is the link to the name spreadsheet: {spreadsheetUrl}";

#       endregion
    }
}
