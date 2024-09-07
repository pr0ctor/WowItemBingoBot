using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Model
{
    internal class User
    {
        public string UserName { get; set; }
        public string DispalyName { get; set; }
        public ulong UserSnowflake { get; set; }
        public List<DiscordRole> UserRoles { get; set; }

        public User(DiscordMember? user)
        {
            if(user is null) throw new ArgumentNullException(nameof(user));

            UserName = user.Username ?? "";
            DispalyName = user.DisplayName ?? "";
            UserSnowflake = user.Id;
            UserRoles = user.Roles.ToList() ?? new();
        }

        public bool CheckUserForRoleById(string roleId)
        {
            if(UserRoles.Count == 0) return false;

            return UserRoles.Exists(r => r.Id.ToString() == roleId);
        }
    }
}
