using DiscordBingoBot.Model;
using DSharpPlus.Commands.ContextChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.ContextChecks
{
    public class DiscordRoleAttribute : ContextCheckAttribute
    {
        public string RoleId { get; init; }
        public DiscordRoleAttribute(string roleEnvVar) => RoleId = EnvironmentVariables.MapNamesToValues(roleEnvVar) ?? "";
    }
}
