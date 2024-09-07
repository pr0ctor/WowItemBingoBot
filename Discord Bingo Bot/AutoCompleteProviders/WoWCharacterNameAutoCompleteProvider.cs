using DiscordBingoBot.Model;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.AutoCompleteProviders
{
    public class WoWCharacterNameAutoCompleteProvider : IAutoCompleteProvider
    {
        public ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext context)
        {
            // Can only show 25 items at a time
            var items = (IReadOnlyDictionary<string, object>)EnvironmentVariables.WoWCharacterNameCache
                .Where(g => g.StartsWith(context.UserInput, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(g => g, g => (object)g.ToString())
                .Take(25)
                .ToDictionary()
                .AsReadOnly();

            return ValueTask.FromResult(items);
        }
    }
}
