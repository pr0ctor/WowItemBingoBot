using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Model
{
    internal class LeaderboardResult(int overallRank, string username, int totalCompletions, int totalsubmissions)
    {
        public int OverallRank { get; set; } = overallRank;
        public string UserName { get; set; } = username;
        public int TotalCompletions { get; set; } = totalCompletions;
        public int TotalSubmissions { get; set; } = totalsubmissions;

        public override string ToString()
        {
            return $"{OverallRank}.\t{UserName}\t{TotalCompletions}\t{TotalSubmissions}";
        }

    }
}
