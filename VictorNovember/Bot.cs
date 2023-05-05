using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using System.IO;
using Newtonsoft.Json;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.EventArgs;
using VictorNovember.Commands;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using VictorNovember.SlashCommands;
using VictorNovember.Utilities;

namespace VictorNovember
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public async Task RunAsync()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("Config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();
            var configJson = JsonConvert.DeserializeObject<ConfigJSON>(json);
            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(config);
            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(1)
            });

            Client.Ready += EventHelper.OnClientReady;
            Client.GuildCreated += EventHelper.OnGuildCreated;
            Client.MessageCreated += EventHelper.OnMessageCreated;
            Client.ComponentInteractionCreated += EventHelper.ButtonPressResponse;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            var slashCommandConfig = Client.UseSlashCommands();

            //prefix based commands
            Commands.RegisterCommands<Fun>();

            //slash commands
            slashCommandConfig.RegisterCommands<FunSL>();
            slashCommandConfig.RegisterCommands<GeneralSL>();
            slashCommandConfig.RegisterCommands<SpaceSL>();


            Commands.CommandErrored += EventHelper.OnCommandError;

            await Client.ConnectAsync();
            await Task.Delay(-1);

        }
    }
}