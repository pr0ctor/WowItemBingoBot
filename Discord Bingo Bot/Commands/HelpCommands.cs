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
    [Command("help")]
    [Description("Information regarding the different commmands")]
    internal class HelpCommands
    {

        [Command("info")]
        [Description("General information regarding the bot and commands")]
#if !DEBUG
        [DiscordRoleAttribute("TeamGreenRole")]
#endif
        public async ValueTask GeneralInformation(SlashCommandContext context)
        {
            await context.DeferResponseAsync(ephemeral: true);

            var finalMessage = new DiscordMessageBuilder().WithContent(Messages.HelpInformationMessage);

            await context.EditResponseAsync(finalMessage);
        }

        [Command("namespreadsheet")]
        [Description("General information regarding the bot and commands")]
#if !DEBUG
        [DiscordRoleAttribute("TeamGreenRole")]
#endif
        public async ValueTask GenralInformation(SlashCommandContext context)
        {
            await context.DeferResponseAsync(ephemeral: true);

            var finalMessage = new DiscordMessageBuilder().WithContent(Messages.HelpNameSpreadsheetMessage(EnvironmentVariables.NameSpreadsheetLink));

            await context.EditResponseAsync(finalMessage);
        }
    }
}
