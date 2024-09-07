using DiscordBingoBot.Model;
using DiscordBingoBot.ContextChecks;
using DSharpPlus.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBingoBot.ParameterChecks;
using System.ComponentModel;
using DSharpPlus.Commands.Processors.SlashCommands;
using DiscordBingoBot.BingoEngine;
using DSharpPlus.Entities;

namespace DiscordBingoBot.Commands
{
    [Command("register")]
    [Description("Register to join WoW Gear Bingo")]
    internal class RegisterCommands
    {
        [Command("character")]
        [Description("Register a character in order to be eligible for credit towards their item drops.")]
#if !DEBUG
        [DiscordRoleAttribute("TeamGreenRole")]
#endif
        public async ValueTask RegisterCharacter(
            SlashCommandContext context,
            [Description("The name of the wow character. Must be 2->12 letters."),
                WoWCharacterName] string characterName
        )
        {

            await context.DeferResponseAsync(ephemeral: true);

            var discordUser = new User(context.Member);

            var isRegistered = await Database.RegisterNewWoWCharacter(discordUser, characterName);

            await context.EditResponseAsync( (isRegistered) 
                ? Messages.SuccessfulWowCharacterNameRegistration(characterName, discordUser.UserName)
                : Messages.WowCharacterAlreadyRegistered
            );
        }

        [Command("me")]
        [Description("Register to join WoW Gear Bingo")]
#if !DEBUG
        [DiscordRoleAttribute("TeamGreenRole")]
#endif
        public async ValueTask RegisterMe(SlashCommandContext context)
        {
            await context.DeferResponseAsync(ephemeral: false);

            var discordUser = new User(context.Member);

            var isRegistered = await Database.CheckUserRegistration(discordUser);

            if(isRegistered)
            {
                await context.EditResponseAsync(Messages.UserAlreadyRegistered);
                return;
            }

            await Database.RegisterNewDiscordUser(discordUser);

            var data = await BingoHelpers.GenerateNewBingoCard(discordUser);

            using var cardFileString = new FileStream(data.cardUri, FileMode.Open);

            var finalMessage = new DiscordMessageBuilder().WithContent(Messages.SuccessfulDiscordUserRegistration).AddFile(cardFileString);

            await context.EditResponseAsync(finalMessage);
        }
    }
}
