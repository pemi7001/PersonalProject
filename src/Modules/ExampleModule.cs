using BrownCow;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace BrownCow.Modules
{
    [Name("Example")]
    public class ExampleModule : ModuleBase<SocketCommandContext>
    {
        [Command("hi")]
        public async Task Hi()
           => await ReplyAsync(Player.Greeting(Context.User.Mention, Context.User as SocketGuildUser));

        [Command("dkp")]
        public async Task dkp()
          => await ReplyAsync(Player.GetDkp(Context.User as SocketGuildUser));
        // => await ReplyAsync("This feature is currently being worked on!");
        [Command("roster")]
        public async Task Roster()
        => await ReplyAsync($"Hello, {Context.User.Mention} Here is the link to the Push roster!  https://docs.google.com/spreadsheets/d/1Mrt516nryUesSlfEPgM6-0X-ZcVyXkcVrA2-TKllSIM/edit#gid=0");

        [Command("help")]
        public async Task Help()
        { 
         await ReplyAsync($"/moodkp - This will check your DKP for you!");
         await ReplyAsync($"/mooroster - This will give you the link to the push group roster.");

        }
        [Command("nice")]
        public async Task Nice([Remainder] string s=null)
        {
            if (s is null)
            { await ReplyAsync($"Nice"); }
            else
            { await ReplyAsync($"{s} Nice"); }
        }
    }
}

