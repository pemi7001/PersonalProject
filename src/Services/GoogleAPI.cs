using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BrownCow;
using BrownCowBattalionDiscordBot.Responses;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using WebApiThrottle;
using System.Web.Http;

namespace SheetsQuickstart
{
    public class ReadEditPlayerDkpSheets
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets};    
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static DkpResponse UpdateEntry(Player player)
        {
            UserCredential credential;
            using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                String spreadsheetId = "1DyjmqXtcmRmyy3mxXSER04BRhbhVpzwe9KJv4INFnTM";               
                DkpResponse response = new DkpResponse();

                string range = $"sheet1!D{player.ID + 1}";
                ValueRange valueRange = new ValueRange();

                var objectList = new List<object>() { $"{player.dkp}" };
                valueRange.Values = new List<IList<object>> { objectList };

                var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                var updateResponse = updateRequest.Execute();
                response.message = $"{player.name} now has {player.dkp} dkp";
                return response;
            }
        }
        
        public static DkpResponse Generatelist(SocketGuild guild)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MessageHandlers.Add(new ThrottlingHandler()
            {
                Policy = new ThrottlePolicy(perSecond: 1, perMinute: 30, perHour: 500, perDay: 2000)
                {
                    IpThrottling = true,
                    ClientThrottling = true,
                    EndpointThrottling = true
                },
                Repository = new CacheRepository()
            });

            SheetsService service= new SheetsService();
            DkpResponse dkpResponse = new DkpResponse();
            UserCredential credential;
            using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                // Create Google Sheets API service.
                
                service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                for (int i = 1; i < guild.Users.Count;)
                {
                    foreach (SocketGuildUser user in guild.Users)
                    {
                        Player player = new Player();
                        player.name = user.Mention;
                        player.ID = i;
                        i++;
                        player.CharacterName = user.Username;
                        player.discordnick = user.Nickname;
                       
                            System.Threading.Thread.Sleep(2000);
                        
                        CreateEntry(player);
                    }
                }
                dkpResponse.message = "List Generated.";
                return dkpResponse;
            }
            
        }
        public static void CreateEntry(Player player)
        {
            UserCredential credential;
            SheetsService service = new SheetsService();
            ValueRange valueRange = new ValueRange();
            String spreadsheetId = "1DyjmqXtcmRmyy3mxXSER04BRhbhVpzwe9KJv4INFnTM";
            String range = $"sheet1!A{player.ID + 1}:e";
            using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                // Create Google Sheets API service.

                service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                var objectList = new List<object>() { $"{player.ID}", $"{player.name}", $"{player.CharacterName}", $"{player.dkp}",$"{player.discordnick}" };
                valueRange.Values = new List<IList<object>> { objectList };

                var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                var appendRepsonse = appendRequest.Execute();
            }
        }
        public static DkpResponse Read()
        {
            DkpResponse dkpResponse = new DkpResponse();
            UserCredential credential;
            using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                String spreadsheetId = "1DyjmqXtcmRmyy3mxXSER04BRhbhVpzwe9KJv4INFnTM";
                String range = "sheet1!A2:F";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        service.Spreadsheets.Values.Get(spreadsheetId, range);

                
                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;
                if (values != null && values.Count > 0)
                {
                    List<Player> players = new List<Player>();
                    Console.WriteLine("number, discord mention, Discord Name,DKP,character");
                    foreach (var row in values)
                    {
                        Player player = new Player();
                        string y = (string)row[3];
                        int.TryParse(y, out int x);
                        player.dkp = x;
                        y = (string)row[0];
                        int.TryParse(y, out x);
                        player.ID = x;
                        player.name = (string)row[1];
                        player.CharacterName = (string)row[2];
                       
                        

                        if (player.CharacterName != null || player.CharacterName != "" && player.isinguild == "y")
                        {
                            players.Add(player);
                        }
                        
                        
                    }
                    dkpResponse.players = players;
                    dkpResponse.message = "List Generated";
                    return dkpResponse;
                }
                else
                {
                    dkpResponse.players = null;
                    dkpResponse.message = "no data found";
                    return dkpResponse;
                }
            }
        }
        public static void addnicknames(Player player)
        {
            UserCredential credential;
            SheetsService service = new SheetsService();
            ValueRange valueRange = new ValueRange();
            String spreadsheetId = "1DyjmqXtcmRmyy3mxXSER04BRhbhVpzwe9KJv4INFnTM";
            String range = $"sheet1!E{player.ID + 1}";
            using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "reliable-realm-246215-862fda588c64.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                // Create Google Sheets API service.

                service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                var objectList = new List<object>() {$"{player.discordnick}" };
                valueRange.Values = new List<IList<object>> { objectList };

                var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                var appendRepsonse = appendRequest.Execute();
            }
        }    
    }
}   
