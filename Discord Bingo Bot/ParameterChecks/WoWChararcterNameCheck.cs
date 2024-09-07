using DiscordBingoBot.Model;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks.ParameterChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBingoBot.ParameterChecks
{
    public sealed class WowChararcterNameCheck : IParameterCheck<WoWCharacterNameAttribute>
    {
        public ValueTask<string?> ExecuteCheckAsync(WoWCharacterNameAttribute attribute, ParameterCheckInfo info, CommandContext context)
        {
            if(info.Value is not string)
            {
                return ValueTask.FromResult<string?>(Messages.InvalidStringParameter);
            }

            var wowCharacterName = ((string)info.Value ?? "").Trim();

            // Check for existance and value
            if (string.IsNullOrEmpty(wowCharacterName)) return ValueTask.FromResult<string?>(Messages.IncorrectWowCharacterNameLength(wowCharacterName));

            // Value should only be between 2->12 characters
            if (wowCharacterName.Length > 12 || wowCharacterName.Length < 2) return ValueTask.FromResult<string?>(Messages.IncorrectWowCharacterNameLength(wowCharacterName));

            // Check for invalid characters
            var checkMatches = Regex.Match(wowCharacterName, Helpers.Helpers.validWowCharacterNameRegex);
            if (!checkMatches.Success) return ValueTask.FromResult<string?>(Messages.IncorrectWowCharacterNameSymbols(wowCharacterName));

            return ValueTask.FromResult<string?>(null);
        }
    }
}
