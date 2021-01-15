using BrownCow;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrownCowBattalionDiscordBot.Responses
{
    public class DkpResponse
    {
        public SocketGuildUser user { get; set; }
        public string message { get; set; }
        public int dkp { get; set; }
        public List<Player> players { get; set; }
    }
}
