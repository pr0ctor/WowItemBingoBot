using DiscordBingoBot.ContextChecks;
using DiscordBingoBot.Model;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Commands
{
    [Command("view")]
    [Description("View various things for WoW Gear Bingo")]
    internal class ViewCommands
    {

        [Command("bingocard")]
        [Description("Retieves your current Bingo Card and progress")]
#if !DEBUG
        [DiscordRoleAttribute("TeamGreenRole")]
#endif
        public async ValueTask ViewBingoCard(SlashCommandContext context)
        {
            await context.DeferResponseAsync(ephemeral: true);

            var discordUser = new User(context.Member);

            var isRegistered = await Database.CheckUserRegistration(discordUser);

            if (!isRegistered)
            {
                await context.EditResponseAsync(Messages.UserNotRegistered);
                return;
            }

            var bingoCard = await Database.GetActiveBingoCardForUser(discordUser);

            using var bingoCardData = await ImageGen.ImageGen.GetExistingBingoCard(bingoCard);

            var finalMessage = new DiscordMessageBuilder().WithContent(Messages.SuccessfulCardRetrieval).AddFile((FileStream)bingoCardData);

            await context.EditResponseAsync(finalMessage);
        }

        [Command("leaderboard")]
        [Description("View the leaderboard for Bingo Card Completion")]
#if !DEBUG
        [DiscordRoleAttribute("TeamGreenRole")]
#endif
        public async ValueTask ViewLeaderboard(SlashCommandContext context)
        {
            await context.DeferResponseAsync(ephemeral: true);

            var leaderboardResults = await Database.GetGlobalLeaderBoard();

            var tableHeader = "\t\tTop 10 Bingoers\n\nRank\tName\tCompletions\tSubmissions\n\n";

            var parsedString = (leaderboardResults.Count() > 0) 
                ? string.Join("\n", leaderboardResults.Select(t => t.ToString()))
                : Messages.NoRecordedRankings;

            var finalMessage = new DiscordMessageBuilder().WithContent(tableHeader + parsedString);

            await context.RespondAsync(finalMessage);
        }

        [Command("mystats")]
        [Description("View your stats")]
#if !DEBUG
        [DiscordRoleAttribute("TeamGreenRole")]
#endif
        public async ValueTask ViewMyStats(SlashCommandContext context)
        {
            await context.DeferResponseAsync(ephemeral: true);

            var discordUser = new User(context.Member);

            var isRegistered = await Database.CheckUserRegistration(discordUser);

            if(!isRegistered)
            {
                await context.EditResponseAsync(Messages.UserNotRegistered);
                return;
            }

            var currentCard = await Database.GetActiveBingoCardForUser(discordUser);

            var totalCompletions = await Database.GetTotalCompletionsForUser(discordUser);
            var totalSubmissions = await Database.GetTotalSubmissionsForUser(discordUser);
            var totalSubmissionsForCard = await Database.GetTotalSubmissionsForUserByCard(discordUser, currentCard);

            var messageText = $"\n\t\t{discordUser.DispalyName}'s Statistics\n\n{Messages.TotalUserCompletions(totalCompletions)}\n{Messages.TotalUserSubmissions(totalSubmissions)}\n{Messages.TotalUserSubmissionsForCard(totalSubmissionsForCard)}";

            var finalMessage = new DiscordMessageBuilder().WithContent(messageText);

            await context.RespondAsync(finalMessage);
        }
    }
}
