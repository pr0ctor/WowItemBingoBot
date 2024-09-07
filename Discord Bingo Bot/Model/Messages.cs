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
        public static readonly string SuccessfulSubmission = "Congrats, this is a new addition to your Bingo Card! Your progress has been recorded:";

        public static readonly string SuccessfulCardRetrieval = "Here is your Bingo Card:";

        public static string SuccessfulWowCharacterNameRegistration(string wowCharacterName, string discordName) => $"The given Character Name '{wowCharacterName}' has been registered by {discordName}.";

        #endregion

        #region Leaderboard Messages

        public static string TotalUserSubmissions(int userSubmissions) => $"You have submitted  { userSubmissions }  item{ ((userSubmissions == 1) ? "" : "s") }.";
        
        public static string TotalUserCompletions(int userCompletions) => $"You have completed  { userCompletions }  card{ ((userCompletions == 1) ? "" : "s") }.";

        public static string TotalUserSubmissionsForCard(int userSubmissions) => $"You have submitted  {userSubmissions}  item{((userSubmissions == 1) ? "" : "s")} for your current Bingo Card.";

        #endregion
    }
}
