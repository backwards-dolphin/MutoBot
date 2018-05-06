using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Muto{
    class Program{
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
        public async Task Start()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            IServiceCollection serviceCollection = new ServiceCollection();
            services = serviceCollection.BuildServiceProvider();

            await InstallCommands(); //?
            string token = File.ReadAllText("discordtoken.txt");
            await client.LoginAsync(Discord.TokenType.Bot, token);
            await client.StartAsync();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Muto is now online, and hungry...");
            Console.ResetColor();

            await Task.Delay(-1);
        }
        public async Task InstallCommands()
        {
            client.GuildAvailable += _client_GuildAvailable;
            /*client.MessageReceived += HandleCommand;
            client.ReactionAdded += client_ReactionAdded;
            client.UserLeft += Client_UserLeft;
            client.UserBanned += Client_UserBanned;
            client.UserJoined += Client_UserJoined;
            client.RoleCreated += Client_RoleCreated;*/
            client.MessageReceived += HandleCommand;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }
        /*private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new AudioService());
        }*/
        private Task _client_GuildAvailable(SocketGuild arg) {
            if (arg.Name.ToLower() == Environment.GetEnvironmentVariable("SERVER_NAME")) {
                Console.WriteLine($"Registering handler for {arg.Name}");
            }
            Console.WriteLine($"Discovered server {arg.Name}");
            return Task.CompletedTask;
        }
        public async Task HandleCommand(SocketMessage messageParam) {
            //for console output below
            var guild = (messageParam.Channel as IGuildChannel)?.Guild;
            var user = (messageParam.Author) as SocketGuildUser;
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new SocketCommandContext(client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);

            /******************/
            Console.ForegroundColor = ConsoleColor.Cyan;

            DateTime dateTime = DateTime.Now;
            string strMinFormat = dateTime.ToString("hh:mm:ss tt");//12 hours format

            Console.Write("[" + strMinFormat + "] ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(messageParam.Author.Username);//
            Console.ResetColor();

            Console.Write(" in ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(guild.Name + " #" + messageParam.Channel.Name + ": ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(messageParam.ToString() + "\n");
            Console.ResetColor();
        }
    }

}

