using DiscordBingoBot.Model;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.AutoCompleteProviders
{
    public class GearNameAutoCompleteProvider : IAutoCompleteProvider
    {
        public ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext context)
        {
            // Can only show 25 items at a time
            var items = (IReadOnlyDictionary<string, object>)EnvironmentVariables.GearItemCache
                .Where(g => g.ItemName.Contains(context.UserInput, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(g => g.ItemName, g => (object)g.ItemId.ToString())
                .Take(25)
                .ToDictionary()
                .AsReadOnly();

            return ValueTask.FromResult(items);
        }
    }
}
