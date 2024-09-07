using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.ChoiceProviders
{
    public class YesNoChoiceProvider : IChoiceProvider
    {
        private static readonly IReadOnlyDictionary<string, object> yesNoChoice = new Dictionary<string, object>
        {
            ["Yes"] = 1,
            ["No"] = 0
        };

        public ValueTask<IReadOnlyDictionary<string, object>> ProvideAsync(CommandParameter parameter) => ValueTask.FromResult(yesNoChoice);
    }
}
