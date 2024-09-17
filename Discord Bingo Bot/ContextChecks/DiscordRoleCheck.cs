using DiscordBingoBot.Model;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.ContextChecks
{
    public class DiscordRoleCheck : IContextCheck<DiscordRoleAttribute>
    {
        public ValueTask<string?> ExecuteCheckAsync(DiscordRoleAttribute attribute, CommandContext context)
        {
            var discordUser = new User(context.Member);

            var validRoles = new List<string>
            {
                EnvironmentVariables.TeamGreenRole,
                EnvironmentVariables.TeamGreenTrialRole
            };

            foreach (var role in validRoles)
            {
                if (discordUser.CheckUserForRoleById(role))
                {
                    return ValueTask.FromResult<string?>(null);
                }
            }

            return ValueTask.FromResult<string?>(Messages.InvalidDiscordRoleAssigned);
        }
    }
}
