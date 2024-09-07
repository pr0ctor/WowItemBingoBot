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

            if (discordUser.CheckUserForRoleById(attribute.RoleId))
            {
                return ValueTask.FromResult<string?>(null);
            }
            else
            {
                return ValueTask.FromResult<string?>(Messages.InvalidDiscordRoleAssigned);
            }
        }
    }
}
