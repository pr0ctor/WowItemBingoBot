using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace DiscordBingoBot.Model
{
    internal static class Database
    {
        private static MySqlConnection Connect()
        {
            var connection = new MySqlConnection(EnvironmentVariables.DatabaseConnectionString);
            connection.Open();
            return connection;
        }

        private static void Disconnect(MySqlConnection connection)
        {
            connection.Close();
        }

        public static async Task PopulateGearCache()
        {
            using var connection = Connect();

            try
            {
                EnvironmentVariables.GearItemCache = await GetAllWoWGear(connection);
                EnvironmentVariables.GearItemCacheByItemId = EnvironmentVariables.GearItemCache
                    .Select(g => new { id = g.ItemId, gearItem = g })
                    .ToDictionary(x =>  x.id, x => x.gearItem);
                EnvironmentVariables.GearItemCacheByItemName = EnvironmentVariables.GearItemCache
                    .Select(g => new { name = g.ItemName, gearItem = g })
                    .ToDictionary(x => x.name, x => x.gearItem);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }
        }

        public static async Task<List<GearItem>> GetRandomGearItems()
        {
            using var connection = Connect();

            try
            {
                return await GetNRandomGearItems(connection, EnvironmentVariables.NumberOfRandomItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return new();
        }

        public static async Task<bool> CheckUserRegistration(User user)
        {
            using var connection = Connect();

            try
            {
                return await IsUserRegistered(connection, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return false;
        }

        public static async Task<bool> RegisterNewDiscordUser(User user)
        {
            using var connection = Connect();

            try
            {
                // Check if they are already registered

                var isRegistered = await IsUserRegistered(connection, user);

                if (!isRegistered)
                {
                    await RegisterUser(connection, user);

                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return false;
        }

        public static async Task<bool> RegisterNewWoWCharacter(User user, string characterName)
        {
            using var connection = Connect();

            try
            {

                // Check if they are already registered

                var isRegistered = await IsUserRegistered(connection, user);

                if(!isRegistered)
                {
                    await RegisterUser(connection, user);
                }

                var isWoWCharacterCreated = await IsWoWCharacterRegistered(connection, characterName);

                if(isWoWCharacterCreated.hasRows)
                {
                    return false; // wow character has already been added
                }

                return await CreateNewWoWCharacter(connection, characterName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return false;
        }

        public static async Task<int> RegisterOrRetrieveWoWCharacter(string characterName)
        {
            using var connection = Connect();

            try
            {

                var isWoWCharacterCreated = await IsWoWCharacterRegistered(connection, characterName);

                if (isWoWCharacterCreated.hasRows)
                {
                    return isWoWCharacterCreated.wowCharID;
                }

                await CreateNewWoWCharacter(connection, characterName);

                isWoWCharacterCreated = await IsWoWCharacterRegistered(connection, characterName);

                return isWoWCharacterCreated.wowCharID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return -1;
        }

        public static async Task<List<string>> GetAllRegisteredWoWCharacters()
        {
            using var connection = Connect();

            try
            {
                return await GetAllWoWCharacters(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return new();
        }

        public static async Task<BingoCard> GetActiveBingoCardForUser(User user)
        {
            using var connection = Connect();

            try
            {
                return await GetActiveBingoCard(connection, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return new(0, "", "", "", false, 0);
        }

        public static async Task CreateNewBingoCard(BingoCard bingoCard)
        {
            using var connection = Connect();
            try
            {
                await CreateNewBingoCard(connection, bingoCard);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }
        }

        public static async Task CreateBingoCardSubmission(User user, BingoCard bingoCard, int wowCharacterId, GearItem gearItem, string submissionUrl)
        {
            using var connection = Connect();
            try
            {
                await CreateNewBingoCardSubmission(connection, user, bingoCard, wowCharacterId, gearItem, submissionUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }
        }

        public static async Task<List<int>> RetrieveSubmissionsForActiveBingoCard(User user, BingoCard bingoCard)
        {
            using var connection = Connect();

            try
            {
                return await GetSubmissionsForActiveBingoCard(connection, user, bingoCard);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return new();
        }

        public static async Task<List<LeaderboardResult>> GetGlobalLeaderBoard()
        {
            using var connection = Connect();

            try
            {
                return await GetGlobalLeaderboardItems(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return new();
        }

        public static async Task<int> GetTotalSubmissionsForUser(User user)
        {
            using var connection = Connect();

            try
            {
                // Check if they are already registered

                var isRegistered = await IsUserRegistered(connection, user);

                if (!isRegistered)
                {
                    return 0;
                }

                return await GetTotalUserSubmissions(connection, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return 0;
        }

        public static async Task<int> GetTotalSubmissionsForUserByCard(User user, BingoCard bingoCard)
        {
            using var connection = Connect();

            try
            {
                // Check if they are already registered

                var isRegistered = await IsUserRegistered(connection, user);

                if (!isRegistered)
                {
                    return 0;
                }

                return await GetTotalUserSubmissionsForCard(connection, user, bingoCard);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return 0;
        }

        public static async Task<int> GetTotalCompletionsForUser(User user)
        {
            using var connection = Connect();

            try
            {
                // Check if they are already registered

                var isRegistered = await IsUserRegistered(connection, user);

                if (!isRegistered)
                {
                    return 0;
                }

                return await GetTotalUserCompletions(connection, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }

            return 0;
        }

        public static async Task CompleteBingoCard(User user, BingoCard bingoCard)
        {
            using var connection = Connect();

            try
            {
                await CreateNewBingoCardCompletion(connection, user, bingoCard);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(connection);
            }
        }

        private static async Task<bool> IsUserRegistered(MySqlConnection connection, User user)
        {
            using var existingUsers = new MySqlCommand
            {
                CommandText = Queries.SelectUsersBasedOnDiscordUserId,
                Connection = connection
            };

            existingUsers.Prepare();

            existingUsers.Parameters.AddWithValue("@discordId", user.UserSnowflake);

            using var results = await existingUsers.ExecuteReaderAsync();

            return results.HasRows;
        }
        
        private static async Task<(bool hasRows, int wowCharID)> IsWoWCharacterRegistered(MySqlConnection connection, string characterName)
        {
            using var existingUsers = new MySqlCommand
            {
                CommandText = Queries.SelectWoWCharacterByName,
                Connection = connection
            };

            existingUsers.Prepare();

            existingUsers.Parameters.AddWithValue("@charactername", characterName);

            using var results = await existingUsers.ExecuteReaderAsync();
            var recordId = -1;

            if(results.HasRows)
            {
                results.Read();

                recordId = results.GetInt32(results.GetOrdinal("id"));
            }

            return (
                hasRows: results.HasRows,
                wowCharID: recordId
            );
        }

        private static async Task RegisterUser(MySqlConnection connection, User user)
        {
            using var createUser = new MySqlCommand
            {
                CommandText = Queries.CreateNewDiscordUser,
                Connection = connection
            };

            createUser.Prepare();

            createUser.Parameters.AddWithValue("@snowflake", user.UserSnowflake);
            createUser.Parameters.AddWithValue("@username", user.UserName);

            await createUser.ExecuteNonQueryAsync();
        }

        private static async Task<bool> CreateNewWoWCharacter(MySqlConnection connection, string characterName)
        {
            using var createWowCharacter = new MySqlCommand
            {
                CommandText = Queries.CreateNewWowCharacter,
                Connection = connection
            };

            createWowCharacter.Prepare();

            createWowCharacter.Parameters.AddWithValue("@characterName", characterName);

            await createWowCharacter.ExecuteNonQueryAsync();

            EnvironmentVariables.WoWCharacterNameCache.Add(characterName);

            return true;
        }

        private static async Task<List<string>> GetAllWoWCharacters(MySqlConnection connection)
        {
            using var selectWoWCharacters = new MySqlCommand
            {
                CommandText = Queries.SelectAllWoWCharacters,
                Connection = connection
            };

            selectWoWCharacters.Prepare();

            using var results = await selectWoWCharacters.ExecuteReaderAsync();

            var nameList = new List<string>();

            while(results.Read())
            {
                nameList.Add(results.GetString(results.GetOrdinal("charactername")));
            }

            return nameList;
        }

        private static async Task CreateNewBingoCard(MySqlConnection connection, BingoCard bingoCard)
        {
            using var createBingoCompletion = new MySqlCommand
            {
                CommandText = Queries.CreateNewBingoCard,
                Connection = connection
            };

            createBingoCompletion.Prepare();

            createBingoCompletion.Parameters.AddWithValue("@discordId",  bingoCard.Owner);
            createBingoCompletion.Parameters.AddWithValue("@sessioncode", bingoCard.SessionCode);
            createBingoCompletion.Parameters.AddWithValue("@boardcode", bingoCard.BoardStateCode);
            createBingoCompletion.Parameters.AddWithValue("@layout", bingoCard.Layout);

            await createBingoCompletion.ExecuteNonQueryAsync();
        }

        private static async Task CreateNewBingoCardSubmission(MySqlConnection connection, User user, BingoCard bingoCard, int wowCharacterId, GearItem gearItem, string submissionUrl)
        {
            using var createBingoSubmission = new MySqlCommand
            {
                CommandText = Queries.CreateNewBingoCardSubmission,
                Connection = connection
            };

            createBingoSubmission.Prepare();

            createBingoSubmission.Parameters.AddWithValue("@discordId", user.UserSnowflake);
            createBingoSubmission.Parameters.AddWithValue("@wowcharacterId", wowCharacterId);
            createBingoSubmission.Parameters.AddWithValue("@gearitemId", gearItem.Id);
            createBingoSubmission.Parameters.AddWithValue("@bingocardId", bingoCard.Id);
            createBingoSubmission.Parameters.AddWithValue("@screenshotUrl", submissionUrl);

            await createBingoSubmission.ExecuteNonQueryAsync();
        }

        private static async Task CreateNewBingoCardCompletion(MySqlConnection connection, User user, BingoCard bingoCard)
        {
            using var createBingoCompletion = new MySqlCommand
            {
                CommandText = Queries.CreateNewBingoCardCompletion,
                Connection = connection
            };

            createBingoCompletion.Prepare();

            createBingoCompletion.Parameters.AddWithValue("@discordId", user.UserSnowflake);
            createBingoCompletion.Parameters.AddWithValue("@bingocardId", bingoCard.Id);

            await createBingoCompletion.ExecuteNonQueryAsync();

            using var updateBingoCardForCompletion = new MySqlCommand
            {
                CommandText = Queries.UpdateBingoCardToCompletion,
                Connection = connection
            };

            updateBingoCardForCompletion.Prepare();

            updateBingoCardForCompletion.Parameters.AddWithValue("@bingocardId", bingoCard.Id);

            await updateBingoCardForCompletion.ExecuteNonQueryAsync();
        }

        private static async Task<List<GearItem>> GetAllWoWGear(MySqlConnection connection)
        {
            using var allGear = new MySqlCommand
            {
                CommandText = Queries.SelectAllWoWGearItems,
                Connection = connection
            };

            allGear.Prepare();

            using var results = await allGear.ExecuteReaderAsync();
            var gearList = new List<GearItem>();

            while(results.Read())
            {
                gearList.Add(
                    new(
                        results.GetInt32(results.GetOrdinal("id")),
                        results.GetInt32(results.GetOrdinal("itemid")),
                        results.GetString(results.GetOrdinal("itemname")),
                        results.GetString(results.GetOrdinal("itemimageurl")),
                        results.GetString(results.GetOrdinal("itemimagename"))
                    )
                );
            }

            return gearList;
        }

        private static async Task<List<GearItem>> GetNRandomGearItems(MySqlConnection connection, int numberOfItems)
        {
            using var nRandomItems = new MySqlCommand
            {
                CommandText = Queries.Select24RandomGearItems(numberOfItems),
                Connection = connection
            };

            nRandomItems.Prepare();

            using var results = await nRandomItems.ExecuteReaderAsync();
            var gearList = new List<GearItem>();

            while (results.Read())
            {
                gearList.Add(
                    new(
                        results.GetInt32(results.GetOrdinal("id")),
                        results.GetInt32(results.GetOrdinal("itemid")),
                        results.GetString(results.GetOrdinal("itemname")),
                        results.GetString(results.GetOrdinal("itemimageurl")),
                        results.GetString(results.GetOrdinal("itemimagename"))
                    )
                );
            }

            return gearList;
        }

        private static async Task<BingoCard> GetActiveBingoCard(MySqlConnection connection, User user)
        {
            using var bingoCard = new MySqlCommand
            {
                CommandText = Queries.SelectActiveBingoCardForUser,
                Connection = connection
            };

            bingoCard.Prepare();

            bingoCard.Parameters.AddWithValue("@discordId", user.UserSnowflake);

            using var results = await bingoCard.ExecuteReaderAsync();

            results.Read();

            return new(
                results.GetInt32(results.GetOrdinal("id")),
                results.GetString(results.GetOrdinal("sessioncode")),
                results.GetString(results.GetOrdinal("layout")),
                results.GetString(results.GetOrdinal("boardstatecode")),
                results.GetBoolean(results.GetOrdinal("completed")),
                results.GetUInt64(results.GetOrdinal("cardowner"))
            );
        }

        private static async Task<List<int>> GetSubmissionsForActiveBingoCard(MySqlConnection connection, User user, BingoCard bingoCard)
        {
            using var activeSubmissions = new MySqlCommand
            {
                CommandText = Queries.SelectAllSubmissionsForBingoCard,
                Connection = connection
            };

            activeSubmissions.Prepare();

            activeSubmissions.Parameters.AddWithValue("@discordId", user.UserSnowflake);
            activeSubmissions.Parameters.AddWithValue("@bingocardid", bingoCard.Id);

            using var results = await activeSubmissions.ExecuteReaderAsync();
            var gearItemIdList = new List<int>();

            while (results.Read())
            {
                gearItemIdList.Add(results.GetInt32(results.GetOrdinal("itemid")));
            }

            return gearItemIdList;
        }

        private static async Task<List<LeaderboardResult>> GetGlobalLeaderboardItems(MySqlConnection connection)
        {
            using var leaderboard = new MySqlCommand
            {
                CommandText = Queries.SelectGlobalLeaderBoardTop10Completions,
                Connection = connection
            };

            leaderboard.Prepare();

            using var results = await leaderboard.ExecuteReaderAsync();
            var resultsList = new List<LeaderboardResult>();
            while (results.Read())
            {
                resultsList.Add(new(
                    results.GetInt32(results.GetOrdinal("overallrank")),
                    results.GetString(results.GetOrdinal("username")),
                    results.GetInt32(results.GetOrdinal("totalcompletions")),
                    results.GetInt32(results.GetOrdinal("totalsubmissions"))
                ));
            }

            return resultsList;
        }

        private static async Task<int> GetTotalUserCompletions(MySqlConnection connection, User user)
        {
            using var userCompletions = new MySqlCommand
            {
                CommandText = Queries.SelectTotalCountOfUserCompletions,
                Connection = connection
            };

            userCompletions.Prepare();

            userCompletions.Parameters.AddWithValue("@discordId", user.UserSnowflake);

            using var results = await userCompletions.ExecuteReaderAsync();

            results.Read();

            return results.GetInt32(results.GetOrdinal("totalcompletions"));
        }

        private static async Task<int> GetTotalUserSubmissions(MySqlConnection connection, User user)
        {
            using var userSubmissions = new MySqlCommand
            {
                CommandText = Queries.SelectTotalCountOfUserSubmissions,
                Connection = connection
            };

            userSubmissions.Prepare();

            userSubmissions.Parameters.AddWithValue("@discordId", user.UserSnowflake);


            using var results = await userSubmissions.ExecuteReaderAsync();

            results.Read();

            return results.GetInt32(results.GetOrdinal("totalsubmissions"));
        }

        private static async Task<int> GetTotalUserSubmissionsForCard(MySqlConnection connection, User user, BingoCard bingoCard)
        {
            using var userSubmissionsForCard = new MySqlCommand
            {
                CommandText = Queries.SelectTotalCountOfUserSubmissionsForSpecificCard,
                Connection = connection
            };

            userSubmissionsForCard.Prepare();

            userSubmissionsForCard.Parameters.AddWithValue("@discordId", user.UserSnowflake);
            userSubmissionsForCard.Parameters.AddWithValue("@bingocardId", bingoCard.Id);

            using var results = await userSubmissionsForCard.ExecuteReaderAsync();

            results.Read();

            return results.GetInt32(results.GetOrdinal("totalsubmissions"));
        }

    }
}
