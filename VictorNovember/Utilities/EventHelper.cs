using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VictorNovember.Common;

namespace VictorNovember.Utilities
{
    internal static class EventHelper
    {
        internal static Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            client.UpdateStatusAsync(new DiscordActivity("Cope2", ActivityType.Competing), UserStatus.DoNotDisturb);
            return Task.CompletedTask;
        }

        internal static async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs args)
        {
            var message = args.Message.Content.ToLower();

            foreach (var compliment in Variables.compliments)
            {
                if (message.Contains(compliment) && ((message.Contains("november") || message.Contains("<@767616736941309962>"))))
                {
                    await args.Channel.TriggerTypingAsync();
                    await args.Channel.SendMessageAsync($"{Variables.response[Variables.randomRepliesToCompliments.Next(0, Variables.response.Length)].ToString()}");
                    return;
                }
            }

            foreach (KeyValuePair<string, string> emote in Variables.emotes)
            {
                if (message.Contains(emote.Key) && (!args.Channel.IsPrivate) && (!args.Author.IsBot))
                {
                    if (emote.Key == "kekw" && message.IndexOf("kekwshook") != -1)
                    {
                        // Skip replacing "kekw" if "kekwshook" is also present
                        continue;
                    }
                    if (emote.Key == "kekw" && message.IndexOf("kekwwut") != -1)
                    {
                        // Skip replacing "kekw" if "kekwwut" is also present
                        continue;
                    }
                    if (emote.Key == "kekw" && message.IndexOf("kekwthink") != -1)
                    {
                        // Skip replacing "kekw" if "kekwthink" is also present
                        continue;
                    }
                    await args.Channel.TriggerTypingAsync();
                    await args.Channel.SendMessageAsync($"{args.Author.Username}: {emote.Value}");
                    return;
                }
            }

            switch (message)
            {
                case var s when message.Contains("please take a shower immediately"):
                    await args.Channel.TriggerTypingAsync();
                    await args.Channel.SendMessageAsync("https://tenor.com/view/shower-duolingo-please-take-a-shower-immediately-take-a-shower-gif-23968576");
                    break;
                case var s when message.Contains("it was never personal"):
                    await args.Channel.TriggerTypingAsync();
                    await args.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/382242328695275525/781952571282685972/It_was_never_personal.gif");
                    break;

                case var s when message.Contains("i serve the soviet union"):
                    await args.Channel.TriggerTypingAsync();
                    await args.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/767653326560821248/782991988517240842/I_Serve_the_Soviet_Union.png");
                    break;

                case var s when message.Contains("flat") && (message.Contains("november") || message.Contains("<@767616736941309962>")):
                    await args.Channel.TriggerTypingAsync();
                    await args.Channel.SendMessageAsync("You wanna die?");
                    break;
                case var s when message.Contains("su-76") && (message.Contains("november") || message.Contains("<@767616736941309962>")):
                    await args.Channel.TriggerTypingAsync();
                    await args.Channel.SendMessageAsync("The SU-76 is the single strongest unit in the game. the early appearance combined with the strong main gun and the barrage make it the most op unit. the gun can go up against any tank the enemy might throw at you while the barrage will annhilate any infantry squads if targeted right. anyone who isn't using the SU-76 is either asking for defeat or plain up stupid");
                    break;
            }
        }
        internal static async Task OnGuildCreated(DiscordClient sender, GuildCreateEventArgs args)
        {
            await args.Guild.SystemChannel.TriggerTypingAsync();
            var embedMessage = new DiscordMessageBuilder()
               .AddEmbed(new DiscordEmbedBuilder()
                   .WithTitle("Thanks for the invitation!")
                   .WithDescription("I'm a work in progress, blame Ribs for everything")
                   .WithColor(DiscordColor.Azure)
                   .WithThumbnail(sender.CurrentUser.AvatarUrl)
                   .WithTimestamp(DateTime.UtcNow)
               );
            await args.Guild.SystemChannel.SendMessageAsync(embedMessage);
        }
        internal static async Task OnCommandError(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if (e.Exception is ChecksFailedException)
            {
                var castedExpection = e.Exception as ChecksFailedException;
                string cooldownTimer = string.Empty;

                foreach (var check in castedExpection.FailedChecks)
                {
                    var cooldown = check as CooldownAttribute;
                    TimeSpan timeLeft = cooldown.GetRemainingCooldown(e.Context);
                    cooldownTimer = timeLeft.ToString(@"hh\:mm\:ss");
                }
                var cooldownMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Error")
                    .WithDescription($"Please wait {cooldownTimer} seconds before using that command again!")
                    .WithColor(DiscordColor.Azure)
                );
                await e.Context.Channel.SendMessageAsync(cooldownMessage);
            }
        }

        internal static async Task OnSlashCommandError(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
        {
            if (e.Exception is SlashExecutionChecksFailedException)
            {
                var castedExpection = e.Exception as SlashExecutionChecksFailedException
;
                string cooldownTimer = string.Empty;

                foreach (SlashCheckBaseAttribute check in castedExpection.FailedChecks)
                {
                    var cooldown = check as SlashCooldownAttribute;
                    TimeSpan timeLeft = cooldown.GetRemainingCooldown(e.Context);
                    cooldownTimer = timeLeft.ToString(@"hh\:mm\:ss");
                }
                var cooldownMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Error")
                    .WithDescription($"Please wait {cooldownTimer} seconds before using that command again!")
                    .WithColor(DiscordColor.Red)
                );
                await e.Context.Channel.SendMessageAsync(cooldownMessage);
            }
        }
    }
}