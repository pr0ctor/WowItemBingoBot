using DiscordBingoBot.Model;
using DSharpPlus.Commands.ContextChecks.ParameterChecks;
using DSharpPlus.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBingoBot.ParameterChecks
{
    public class GearNameCheck : IParameterCheck<GearNameAttribute>
    {

        public ValueTask<string?> ExecuteCheckAsync(GearNameAttribute attribute, ParameterCheckInfo info, CommandContext context)
        {
            if (info.Value is not string)
            {
                return ValueTask.FromResult<string?>(Messages.InvalidStringParameter);
            }

            var items = EnvironmentVariables.GearItemCache;

            var selectionName = ((string)info.Value ?? "").Trim();

            var hasFoundSelectionByItemName = items.Exists(g => g.ItemName == selectionName);
            var hasFoundSelectionByItemId = items.Exists(g => g.ItemId.ToString() == selectionName);

            if (!hasFoundSelectionByItemName && !hasFoundSelectionByItemId) return ValueTask.FromResult<string?>(Messages.IncorrectGearNameEntry(selectionName));

            return ValueTask.FromResult<string?>(null);
        }
    }
}
