using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Discord.WebSocket;
using BrownCowBattalionDiscordBot.Responses;

namespace BrownCow
{
    public class Player
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int dkp { get; set; }
        public string CharacterName { get; set; }
        public string discordnick { get; set; }
        public string isinguild { get; set; }

        public static string Greeting(string mention, SocketGuildUser user)
        {
            List<Player> playerList = PLayerList.GetPlayerList();

            string message = null;
            var isRegistered = false;
            if (playerList.Count > 0)
            {
                foreach (Player player in playerList)
                {
                    if (player.name == user.Mention)
                    {
                        isRegistered = true;
                        break;
                    }
                }
                if (isRegistered == true)
                {
                    message = $"👋 Hi, {mention}! You are already Registered! say '/moo dkp' to see your dkp";
                }
                else
                {
                    message = $"👋 Hi, {mention}!, you have not registered yet, say '/moostart' to do so!";
                }
            }
            else
            {
                message = $"👋 Hi, {mention}!, you have not registered yet, say '/moostart' to do so!";
            }
            return message;
        }
        public static string Start(SocketGuildUser user)
        {
            return PLayerList.AddPlayer(user.Mention);
        }

        //get
        public static List<Player> GetPlayers()
        {
            PLayerList pLayerList = new PLayerList();
            List<Player> pLayerList2 = new List<Player>();
            pLayerList2 = pLayerList.GetPlayers();
            return pLayerList2;
        }

        public static string GetDkp(SocketGuildUser user)
        {
            int dkp = 0;
            List<Player> playerlist = GetPlayers();
            foreach (Player player in playerlist)
            {
                if (user.Mention == player.name)
                {
                    dkp = player.dkp;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return $"{user.Mention} you have {dkp} DKP!";
        }
        public static string updateDkp(string useranddkp)
        {
            DkpResponse response = new DkpResponse();
            int dkp;
            string[] udkps = useranddkp.Split(new char[] { ' ' }, 2);
            if (Int32.TryParse(udkps[1].ToString(), out dkp))
            {
                string name = udkps[0].ToString();
                List<Player> players = SheetsQuickstart.ReadEditPlayerDkpSheets.Read().players;
                foreach (Player player in players)
                {
                    if (player.name == name)
                    {
                        Int32.TryParse(udkps[1].ToString(), out dkp);
                        player.dkp = player.dkp + dkp;
                        response = SheetsQuickstart.ReadEditPlayerDkpSheets.UpdateEntry(player);
                    }
                }
                return response.message;
            }
            else
            {
                return $"cannot parse {useranddkp[1].ToString()}";
            }
        }
        public static string CompleteList(SocketGuild guild)
        {
            guild.DownloadUsersAsync();
            IReadOnlyCollection<SocketGuildUser> users = guild.Users;
            List<Player> existingplayers = SheetsQuickstart.ReadEditPlayerDkpSheets.Read().players;
            List<SocketGuildUser> newusers = new List<SocketGuildUser>();
            List<Player> all = new List<Player>();

            foreach (SocketGuildUser user in users)
            {
                Player player2 = new Player();
                player2.name = user.Mention;
                player2.ID = existingplayers.Count + 1;
                player2.CharacterName = user.Username;
                player2.discordnick = user.Nickname;

                foreach (Player player in existingplayers)
                {
                    if (player.name == user.Mention)
                    {
                        player2.isinguild = "y";
                    }
                }
                if (player2.isinguild != "y")
                {
                    SheetsQuickstart.ReadEditPlayerDkpSheets.CreateEntry(player2);
                }
            }
            return "Action Completed";
        }
        public static string ForceAddPlayer(SocketGuildUser user)
        {
            Player player = new Player();
            DkpResponse dkpResponse = new DkpResponse();
          
            player.name = user.Mention;
            player.ID = dkpResponse.players.Count + 1;
            player.CharacterName = user.Username;
            player.discordnick = user.Nickname;
            SheetsQuickstart.ReadEditPlayerDkpSheets.CreateEntry(player);
            return PLayerList.AddPlayer(user.Mention);
        }
        public static string spendDkp(string useranddkp)
        {
            DkpResponse response = new DkpResponse();
            int dkp;
            string[] udkps = useranddkp.Split(new char[] { ' ' }, 2);
            if (Int32.TryParse(udkps[1].ToString(), out dkp))
            {
                string name = udkps[0].ToString();
                List<Player> players = SheetsQuickstart.ReadEditPlayerDkpSheets.Read().players;
                foreach (Player player in players)
                {
                    if (player.name == name)
                    {
                        Int32.TryParse(udkps[1].ToString(), out dkp);
                        player.dkp = player.dkp - dkp;
                        response = SheetsQuickstart.ReadEditPlayerDkpSheets.UpdateEntry(player);
                    }
                }
                return $"{dkp} dkp has been spent, {response.message}";
            }
            else
            {
                return $"cannot parse {useranddkp[1].ToString()}";
            }
        }
        public static string AddDkp(SocketGuildChannel channel)
        {
            List<Player> players = SheetsQuickstart.ReadEditPlayerDkpSheets.Read().players;
            List<Player> raiders = new List<Player>();
            List<Player> notregistered = new List<Player>();
            List<Player> awarded = new List<Player>();
            foreach (Player player in players)
            {
                foreach (SocketGuildUser user in channel.Users)
                {
                    if (user.Mention == player.name)
                    {
                        awarded.Add(player);
                    }

                }
            }

            foreach (Player player1 in awarded)
            {
                player1.dkp++;
                SheetsQuickstart.ReadEditPlayerDkpSheets.UpdateEntry(player1);
            }
            return "Woo boss down 1dkp for everyone!!";
        }
        public static string OnTimeDkp(SocketGuildChannel channel)
        {
            List<Player> players = SheetsQuickstart.ReadEditPlayerDkpSheets.Read().players;
            List<Player> raiders = new List<Player>();
            List<Player> notregistered = new List<Player>();
            List<Player> awarded = new List<Player>();
            foreach (Player player in players)
            {
                foreach (SocketGuildUser user in channel.Users)
                {
                    if (user.Mention == player.name)
                    {
                        awarded.Add(player);
                    }

                }
            }

            foreach (Player player1 in awarded)
            {
                player1.dkp++;
                player1.dkp++;
                player1.dkp++;
                player1.dkp++;
                SheetsQuickstart.ReadEditPlayerDkpSheets.UpdateEntry(player1);
            }
            return "Thank you for being on time, 3dkp for you!!";
        }
        public static string UpdateRaidDkp(string dkpstring, SocketGuildChannel channel)
        {
            foreach (SocketGuildUser user in channel.Users)
            {
                DkpResponse response = new DkpResponse();

                string name = user.Mention;
                List<Player> players = SheetsQuickstart.ReadEditPlayerDkpSheets.Read().players;
                foreach (Player player in players)
                {
                    if (player.name == name)
                    {
                        Int32.TryParse(dkpstring, out int dkp);
                        player.dkp = player.dkp + dkp;
                        response = SheetsQuickstart.ReadEditPlayerDkpSheets.UpdateEntry(player);
                        return response.message;
                    }
                }
            }
            return $"{dkpstring} has been Added or Removed from every player in {channel.Name}!";
        }
    }

    class PLayerList
    {

        //get
        public List<Player> GetPlayers()
        {
            List<Player> playerlist = SheetsQuickstart.ReadEditPlayerDkpSheets.Read().players;
            return playerlist;
        }
        public static List<Player> GetPlayerList()
        {

            List<Player> players = SheetsQuickstart.ReadEditPlayerDkpSheets.Read().players;

            return players;
        }

        //todo
        public DkpResponse EditPlayerDkp(Player player)
        {
            DkpResponse response = SheetsQuickstart.ReadEditPlayerDkpSheets.UpdateEntry(player);
            return response;
        }       
        //no need
        public void DeletePlayer()
        { }
        //todo
        public static string AddPlayer(string name)
        {
            DkpResponse response = new DkpResponse();
            string message = response.message;
            int index = 0;
            List<Player> playerlist = GetPlayerList();
            if (playerlist.Count == 0)
            {
                index = 1;
            }
            else
            {
                foreach (Player player in playerlist)
                {
                    if (player.name == name)
                    {
                        message = "you are already registered";
                    }
                    else
                    {
                        index = player.ID + 1;
                    }

                }
            }
            if (message == "you are already registered")
            {
                return response.message;
            }
            else { 
                
                List<Player> players2 = GetPlayerList();
                if (players2.Count == playerlist.Count + 1)
                {
                    message = "you have been added to the roster!";
                }
                else
                {
                    message = "an error occured!";
                }
                return response.message;
            } }
    }
}
