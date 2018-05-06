using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Threading;


namespace Muto.Modules {
    public class Starter : ModuleBase<SocketCommandContext> {
        [Command("hello"), Summary("First command")]
        public async Task HelloAsync() {
            var name = Context.User.Mention;
            await Context.Channel.SendMessageAsync($"Hello, {name}!");
        }
        [Command("ping"), Summary("Response time")]
        public async Task PingAsync() {
            var ping = Context.Client.Latency;
            var name = Context.User.Mention;
            await Context.Channel.SendMessageAsync($"{name} Pong! :ping_pong: {ping} ms");

        }
        [Command("roll"), Summary("placeholder")]
        public async Task Roll() {
            Random rnd = new Random();
            int yourNum = rnd.Next(0, 100);
            int theirNum = rnd.Next(0, 100);
            if (yourNum > theirNum) {
                await Context.Channel.SendMessageAsync($"You won! You rolled: **{yourNum}** They rolled: **{theirNum}**");
            }
            else if (yourNum < theirNum) {
                await Context.Channel.SendMessageAsync($"You lost! You rolled: **{yourNum}** They rolled: **{theirNum}**");
            }
            else {
                await Context.Channel.SendMessageAsync($"Tie! You rolled: **{yourNum}** They also rolled: **{theirNum}**");
            }
        }
    }
}
