using DiscordBingoBot.BingoEngine;
using DiscordBingoBot.Commands;
using DiscordBingoBot.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBingoBot.Helpers
{
    public static class Helpers
    {
        // No special characters, numbers, or whitespace but still allow characters with diacritics like ñ or å
        //public static readonly string validWowCharacterNameRegex = @"[\d\s$&+,:;=?@#|'<>.^*()%!-]+";
        public static readonly string validWowCharacterNameRegex = @"\p{L}+";

        public static async Task PopulateBackgroundData()
        {
            BuildEnvironmentVariables();

            BingoHelpers.InitializeBingoEngine();

            await Database.PopulateGearCache();

            EnvironmentVariables.WoWCharacterNameCache = await Database.GetAllRegisteredWoWCharacters();

            await PopulateImageCaches();
        }

        public static void BuildEnvironmentVariables()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile($"settings.development.json")
#else
                .AddJsonFile($"settings.production.json")
#endif
                .Build();

            EnvironmentVariables.DatabaseConnectionString = configBuilder[EnvironmentVariables.DatabaseConnectionStringName] ?? "";
            EnvironmentVariables.AdminDiscordRole = configBuilder[EnvironmentVariables.AdminDiscordRoleName] ?? "";
            EnvironmentVariables.DiscordAppId = configBuilder[EnvironmentVariables.DiscordAppIdName] ?? "";
            EnvironmentVariables.DiscordAppToken = configBuilder[EnvironmentVariables.DiscordAppTokenName] ?? "";
            EnvironmentVariables.TeamGreenRole = configBuilder[EnvironmentVariables.TeamGreenRoleName] ?? "";
            EnvironmentVariables.TeamGreenTrialRole = configBuilder[EnvironmentVariables.TeamGreenTrialRoleName] ?? "";
            EnvironmentVariables.TeamGreenLeadsRole = configBuilder[EnvironmentVariables.TeamGreenLeadsRoleName] ?? "";
            EnvironmentVariables.NameSpreadsheetLink = configBuilder[EnvironmentVariables.NameSpreadsheetLinkName] ?? "";

            EnvironmentVariables.BaseDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

            var fonts = new SixLabors.Fonts.FontCollection();
            //EnvironmentVariables.BingoTextFont = fonts.Add($@"{EnvironmentVariables.BaseDirectoryPath}/font/RobotoMono-VariableFont_wght.ttf");
            EnvironmentVariables.BingoTextFont = fonts.Add($@"{EnvironmentVariables.BaseDirectoryPath}/font/RubikMonoOne-Regular.ttf");
        }

        public static IEnumerable<Type> GenerateCommandRegistrationList()
        {
            return [
                typeof(RegisterCommands),
                typeof(ViewCommands),
                typeof(SubmitCommand),
                typeof(HelpCommands)
            ];
        }

        private static async Task PopulateImageCaches()
        {
            var gearIconImages = new Dictionary<string, byte[]>();
            foreach(var item in EnvironmentVariables.GearItemCache)
            {
                if (gearIconImages.ContainsKey(item.ItemImageName)) continue;

                var fileBytes = File.ReadAllBytes(EnvironmentVariables.BaseDirectoryPath + $@"/images/icons/{item.ItemImageName}");
                gearIconImages.Add(item.ItemImageName, fileBytes);
                //Thread.Sleep(100);
            }
            EnvironmentVariables.GeatItemCacheForImageData = gearIconImages;

            var bingoCardImages = new Dictionary<string, byte[]>();
            var bingoCardTemplateCount = Directory.GetFiles(EnvironmentVariables.BaseDirectoryPath + $@"/images/bingocards/").Count();
            foreach (var number in Enumerable.Range(0, bingoCardTemplateCount-1))
            {
                var fileBytes = File.ReadAllBytes(EnvironmentVariables.BaseDirectoryPath + $@"/images/bingocards/{number}.jpg");

                bingoCardImages.Add($"{number}.jpg", fileBytes);
            }
            EnvironmentVariables.BingoCardBaseImageCache = bingoCardImages;

            var bigXBytes = File.ReadAllBytes(EnvironmentVariables.BaseDirectoryPath + $@"/images/misc/bigx.png");
            EnvironmentVariables.BigXData = bigXBytes;
        }


#if DEBUG
        // await Helpers.DownloadGearItemImages(EnvironmentVariables.GearItemCache);
        public static async Task DownloadGearItemImages(List<GearItem> items)
        {
            using var client = new HttpClient();
            
            foreach (GearItem item in items)
            {
                var location = EnvironmentVariables.BaseDirectoryPath + $@"/images/icons/{item.ItemImageName}";

                if (File.Exists(location)) continue;

                Console.WriteLine($"Getting image for {item.ItemName} -> {item.ItemImageName}");
                using var filestream = File.Create(location);
                using var response = await client.GetAsync(item.ItemImageUrl);
                await response.Content.CopyToAsync(filestream);

                Thread.Sleep(2000);
            }
        }
#endif
    }
}
