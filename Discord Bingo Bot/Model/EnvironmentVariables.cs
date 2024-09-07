using BingoEngine.Core;
using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Model
{
    internal static class EnvironmentVariables
    {
        public static readonly int NumberOfRandomItems = 24;

        public const string DatabaseConnectionStringName = "DatabaseConnectionString";
        public const string AdminDiscordRoleName = "AdminDiscordRole";
        public const string DiscordAppIdName = "DiscordAppId";
        public const string DiscordAppTokenName = "DiscordAppToken";
        public const string TeamGreenRoleName = "TeamGreenRole";
        public const string TeamGreenLeadsRoleName = "TeamGreenLeadsRole";

        public static string DatabaseConnectionString { get; set; }
        public static string AdminDiscordRole { get; set; }
        public static string DiscordAppId { get; set; }
        public static string DiscordAppToken { get; set; }
        public static string TeamGreenRole { get; set; }
        public static string TeamGreenLeadsRole { get; set; }

        public static string BaseDirectoryPath { get; set; }

        public static IBingoPattern BingoPattern { get; set; }

        public static readonly Random random = new Random();

        public static List<GearItem> GearItemCache { get; set; } = new();
        public static Dictionary<int, GearItem> GearItemCacheByItemId { get; set; } = new();
        public static Dictionary<string, GearItem> GearItemCacheByItemName { get; set; } = new();
        public static Dictionary<string, byte[]> GeatItemCacheForImageData { get; set; } = new();
        public static Dictionary<string, byte[]> BingoCardBaseImageCache { get; set; } = new();
        public static List<string> WoWCharacterNameCache { get; set; } = new();

        public static FontFamily BingoTextFont { get; set; }

        public static byte[] BigXData { get; set; }

        public static string MapNamesToValues(string envVarName)
        {
            return envVarName switch
            {
                DatabaseConnectionStringName => DatabaseConnectionString,
                AdminDiscordRoleName => AdminDiscordRole,
                DiscordAppIdName => DiscordAppId,
                DiscordAppTokenName => DiscordAppToken,
                TeamGreenRoleName => TeamGreenRole,
                TeamGreenLeadsRoleName => TeamGreenLeadsRole,
                _ => ""
            };
        }
    }

}
