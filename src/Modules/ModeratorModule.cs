using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
namespace BrownCow.Modules
{
    [Name("Moderator")]
    [RequireContext(ContextType.Guild)]
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [Summary("Kick the specified user.")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick([Remainder] SocketGuildUser user)
        {
            await ReplyAsync($"cya {user.Mention} :wave:");

        }
        
        [Command("drive")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task Drive()
        {
            foreach (Google.Apis.Drive.v3.Data.File file in BrownCow.Drive.Connect())
            {
                if (file.Name == "BrowncowScreenshots")
                {
                    await ReplyAsync($"{file.Name}");
                }
            }
        }
        [Command("push")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task Push()
        {
            await ReplyAsync($"@everyone Hello, The push group roster has been updated. please check it out!!");
            await ReplyAsync($"https://discord.com/channels/713540919336632420/713540919336632423/792829672123531284");
        }
        [Command("rbg")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task RBG()
        {
            await ReplyAsync($"@everyone Hello, RBGs will be happening on Sat and Sunday at 8, Here are the groups.!!");
            await ReplyAsync($"");
        }
        [Command("normal")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task Normal()
        {
            await ReplyAsync($"@everyone Hello, The Normal raids are now tuesdays at 730 server time. See you there!!");
        }
        [Command("wingclear")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task Adddkp()
           => await ReplyAsync(Player.AddDkp(Context.Guild.GetChannel(727737862916931584) as SocketGuildChannel));
        [Command("ontime")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task OnTimeDkpAdddkp()
           => await ReplyAsync(Player.OnTimeDkp(Context.Guild.GetChannel(727737862916931584) as SocketGuildChannel));
        [Command("whatisdkp")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task GetDkp([Remainder] SocketGuildUser user)
           => await ReplyAsync(Player.GetDkp(user));
        [Command("updatedkp")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task updatedkp([Remainder] string useranddkp)
           => await ReplyAsync(Player.updateDkp(useranddkp));
        [Command("spendyspendy")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task spenddkp([Remainder] string useranddkp)
           => await ReplyAsync(Player.spendDkp(useranddkp));
        [Command("AddPlayer")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task AddPlayer([Remainder] SocketGuildUser user)
           => await ReplyAsync(Player.ForceAddPlayer(user));
        [Command("leadhelp")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task Help()
        {
            await ReplyAsync("push - This command will display the push info");
            await ReplyAsync("wingclear - This command will Award 1dkp to each person in the Raiding channel.");
            await ReplyAsync("ontime - This command will award 3dkp to each person in the Raiding channel.");
            await ReplyAsync("whatisdkp {@username} - This command will look up a players dkp for you");
            await ReplyAsync("spendyspendy {@username amountspent} -  This command will spend dkp from the user");
            await ReplyAsync("updatedkp {@username changeindkp} -  This will change the dkp of the tagged member");
            await ReplyAsync("raiddkp {@username changeindkp} -  this will change the dkp of everyone in the raiding channel");
        }
        [Command("completelist")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task Complete()
            => await ReplyAsync(Player.CompleteList(Context.Guild as SocketGuild));
        [Command("raiddkp")]
        [RequireUserPermission(GuildPermission.PrioritySpeaker)]
        public async Task updateraiddkp([Remainder] string dkp)
           => await ReplyAsync(Player.UpdateRaidDkp(dkp, Context.Guild.GetChannel(727737862916931584) as SocketGuildChannel));
    }
}
