using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Model
{
    internal static class Queries
    {

        #region Select Queries

        public static readonly string SelectActiveBingoCardForUser = "select * from bingocards where completed = 0 and cardowner = @discordid";

        public static readonly string SelectAllWoWGearItems = "select id, itemid, itemname, itemimageurl, itemimagename from wowgear";

        public static readonly string SelectWoWCharacterByName = "select * from wowcharacters where charactername like @charactername";

        public static readonly string SelectAllWoWCharacters = "select charactername from wowcharacters";

        // yes it might be slow or inefficient, but theres only like <250 items
        public static readonly string SelectUsersBasedOnDiscordUserId = "select snowflake from discordusers where snowflake = @discordId limit 1";

        public static readonly string SelectAllSubmissionsForBingoCard = "select g.itemid as itemid from wowgear g, bingocardsubmissions bs, bingocards bc where bs.gearitem = g.id and bs.bingocard = bc.id and bc.id = @bingocardid and bs.discorduser = @discordId and bc.completed = 0";

        public static string Select24RandomGearItems(int numberOfItems) => $"select id, itemid, itemname, itemimageurl, itemimagename from wowgear order by rand() limit {numberOfItems}";

        #endregion

        #region Create Queries

        public static readonly string CreateNewDiscordUser = "insert into discordusers(snowflake, username) values( @snowflake , @username )";

        public static readonly string CreateNewWowCharacter = "insert into wowcharacters(charactername) values( @characterName )";

        public static readonly string CreateNewBingoCard = "insert into bingocards(sessioncode, layout, boardstatecode, cardowner) values( @sessioncode , @layout , @boardcode , @discordId )";

        public static readonly string CreateNewBingoCardSubmission = "insert into bingocardsubmissions(discorduser, wowcharacter, gearitem, bingocard, screenshoturl) values( @discordId , @wowcharacterId , @gearitemId , @bingocardId , @screenshotUrl )";
        
        public static readonly string CreateNewBingoCardCompletion = "insert into bingocardcompletions(discorduser, bingocard) values( @discordId , @bingocardId )";

        #endregion

        #region Update Queries

        public static readonly string UpdateBingoCardToCompletion = "update bingocards set completed = true where id = @bingocardId";

        #endregion

        #region Leaderboard Queries

        public static readonly string SelectTotalCountOfUserCompletions = "select count(id) as totalcompletions from bingocardcompletions where discorduser = @discordId";

        public static readonly string SelectTotalCountOfUserSubmissions = "select count(id) as totalsubmissions from bingocardsubmissions where discorduser = @discordId";

        public static readonly string SelectTotalCountOfUserSubmissionsForSpecificCard = "select count(id) as totalsubmissions from bingocardsubmissions where discorduser = @discordId and bingocard = @bingocardId";

        public static readonly string SelectGlobalLeaderBoardTop10Completions = @"
            select 
                d.username as username, 
                c.total_completions as totalcompletions, 
                s.total_submissions as totalsubmissions,
                rank() over (order by c.total_completions desc) as overallrank
            from discordusers d
            join (
                select discorduser, count(id) as total_completions
                from bingocardcompletions
                group by discorduser
            ) c on c.discorduser = d.snowflake
            join (
                select discorduser, count(id) as total_submissions
                from bingocardsubmissions
                group by discorduser
            ) s on s.discorduser = d.snowflake
            order by c.total_completions desc
            limit 10
";

        #endregion

    }
}
