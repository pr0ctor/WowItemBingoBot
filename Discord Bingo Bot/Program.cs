// See https://aka.ms/new-console-template for more information
using DiscordBingoBot.ContextChecks;
using DiscordBingoBot.Helpers;
using DiscordBingoBot.Model;
using DiscordBingoBot.ParameterChecks;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;


await Helpers.PopulateBackgroundData();

//await Helpers.DownloadGearItemImages(EnvironmentVariables.GearItemCache);

DiscordClientBuilder clientBuilder = DiscordClientBuilder.CreateDefault(
    EnvironmentVariables.DiscordAppToken,
#if DEBUG
    DiscordIntents.AllUnprivileged | SlashCommandProcessor.RequiredIntents
#else
    SlashCommandProcessor.RequiredIntents
#endif
);

#if DEBUG
clientBuilder.SetLogLevel(LogLevel.Debug);
#else
clientBuilder.SetLogLevel(LogLevel.Debug);
#endif

clientBuilder.UseCommands
(
    extension =>
    {
        // Register Context Checks
        extension.AddCheck<DiscordRoleCheck>();

        // Register Parameter Checks
        extension.AddParameterCheck<GearNameCheck>();
        extension.AddParameterCheck<FilenameFiletypeCheck>();
        extension.AddParameterCheck<WowChararcterNameCheck>();

        // Register Commands
        extension.AddCommands(Helpers.GenerateCommandRegistrationList());
        
        SlashCommandProcessor slashCommandProcessor = new();
        extension.AddProcessors(slashCommandProcessor);
    }
);

var discordClient = clientBuilder.Build();

DiscordActivity status = new("WoW Item Bingo", DiscordActivityType.Competing);

await discordClient.ConnectAsync(status, DiscordUserStatus.Online);

await Task.Delay(-1);