using System;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DiscordBot.Commands;
using static System.Reflection.Assembly;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Net;
using DSharpPlus.Lavalink;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
namespace DiscordBot
{
    class Program
    {
        public readonly EventId BotEventId = new EventId(42, "Bot-Ex01");
        public DiscordClient Client { get; set; }
        static void Main(string[] args)
        {
            var prog = new Program();
            prog.MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            
            var json = "";
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            var cfgjson = JsonConvert.DeserializeObject<ConfigJson>(json);
            
            var client = new DiscordClient(new DiscordConfiguration()
            {
                Token = cfgjson.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Information
            });

            var commands = client.UseCommandsNext(new CommandsNextConfiguration()
            { 
                StringPrefixes = new[] { cfgjson.Prefix }
                
            });
            
            commands.RegisterCommands<CMD>();
            var endpoint = new ConnectionEndpoint
            {
                Hostname = "127.0.0.1",
                Port = 2333
            };

            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = "monSuperMotDePasse",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };
            var lavalink = client.UseLavalink();

            await client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);

            await Task.Delay(-1);
        }
        
        public struct ConfigJson
        {
            [JsonProperty("token")]
            public string Token { get; private set; }

            [JsonProperty("prefix")]
            public string Prefix { get; private set; }
        }
    }
}