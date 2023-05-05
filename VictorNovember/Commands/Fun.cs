using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VictorNovember.Commands
{
    public class Fun : BaseCommandModule
    {
        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.Channel.SendMessageAsync($"Bot connected with an expected ping of `{ctx.Client.Ping} ms`");
        }

        [Command("getNovember")]
        public async Task Invite(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            await ctx.Channel.SendMessageAsync("You can use this link to invite me to your server: https://discord.com/oauth2/authorize?client_id=767616736941309962&scope=bot&permissions=8");
        }

        [Command("say")]
        public async Task Say(CommandContext ctx, [RemainingText] string text)
        {
            if ((text.ToString().IndexOf("@") >= 0) == true)
            {
                await ctx.TriggerTypingAsync();
                await ctx.Channel.SendMessageAsync("I won't say a message with that symbol.");
                return;
            }
            await ctx.Message.DeleteAsync();
            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(text);
        }
        [Command("rr")]
        [RequireGuild]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task RussianRoulette(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();


            var message = await ctx.Channel.SendMessageAsync($"{ctx.User.Username} loads the bullet and spins the cylinder...");
            await Task.Delay(2000);
            await ctx.TriggerTypingAsync();
            await message.ModifyAsync(x =>
            {
                x.Content = $"{ctx.User.Username} puts the gun up to their head and pulls the trigger...";
            });

            int bullet = new Random().Next(0, 5);
            if (bullet == 1)
            {
                var timeDuration = DateTime.Now + TimeSpan.FromMinutes(2);
                var user = (ctx.User as DiscordMember);

                await ctx.TriggerTypingAsync();
                await message.ModifyAsync(x =>
                {
                    x.Content = "BANG!";
                });
                await Task.Delay(500);
                if (user.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    await message.ModifyAsync(x =>
                    {
                        x.Content = $"The gun fired! But it bounced off of {ctx.User.Username}'s head! Their skull is just too thick";
                    });
                    return;
                }
                else
                {
                    await user.TimeoutAsync(timeDuration);
                    var DMChannel = await ctx.Member.CreateDmChannelAsync();
                    await DMChannel.SendMessageAsync($"You've been muted in {ctx.Guild.Name} for 2 minutes. You've died in a game of Russian Roulette. Try again if you dare.");
                    await ctx.TriggerTypingAsync();
                    await message.ModifyAsync(x =>
                    {
                        x.Content = $"The chamber was loaded! {ctx.User.Username} shot themself in the head!";
                    });

                    return;
                }

            }
            else
            {
                await ctx.TriggerTypingAsync();
                await Task.Delay(1000);
                await message.ModifyAsync(x =>
                {
                    x.Content = "*clicks*";
                });
                await Task.Delay(500);
                await ctx.TriggerTypingAsync();
                await message.ModifyAsync(x =>
                {
                    x.Content = $"The chamber was empty! {ctx.User.Username} has survived!";
                });
                return;
            }
        }
        [Command("embed")]
        [Hidden]
        public async Task SendEmbed(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Russian Roulett",
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = ctx.User.AvatarUrl,
                    Name = ctx.User.Username
                },
                Description = $"{ ctx.User.Username } loads the bullet and spins the cylinder...",
                Color = DiscordColor.Azure,
            };
            await ctx.Channel.SendMessageAsync(embed: embedMessage);
        }
        [Command("debug")]
        [Hidden]
        public async Task SendDebug(CommandContext ctx)
        {
            await ctx.Channel.TriggerTypingAsync();

            DiscordButtonComponent btn1 = new DiscordButtonComponent(ButtonStyle.Primary, "1", "Button 1");
            DiscordButtonComponent btn2 = new DiscordButtonComponent(ButtonStyle.Primary, "2", "Button 2");

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://dad-jokes.p.rapidapi.com/random/joke"),
                Headers =
                {
                    { "X-RapidAPI-Key", "08c63ed0ffmsh098800c19682bf6p1f3492jsnc4187cf6759c" },
                    { "X-RapidAPI-Host", "dad-jokes.p.rapidapi.com" },
                },
            };
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var joke = JsonConvert.DeserializeObject<dynamic>(body);

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = joke.body[0].setup.ToString(),
                Description = joke.body[0].punchline.ToString(),
                Color = DiscordColor.Azure
            };

            var message = new DiscordMessageBuilder()
                .AddEmbed(embedMessage)
                .AddComponents(btn1)
                .AddComponents(btn2);

            await ctx.Channel.SendMessageAsync(message);


        }
        [Command("poll")]
        [RequireGuild]
        [Hidden]
        public async Task CreatePoll(CommandContext ctx, int TimeLimit, string Option1, string Option2, string Option3, string Option4, params string[] Question)
        {
            await ctx.TriggerTypingAsync();
            TimeSpan timer = TimeSpan.FromSeconds(TimeLimit);
            var interactivity = ctx.Client.GetInteractivity();
            DiscordEmoji[] optionEmotes = { DiscordEmoji.FromName(ctx.Client, ":one:", false),
                                            DiscordEmoji.FromName(ctx.Client, ":two:", false),
                                            DiscordEmoji.FromName(ctx.Client, ":three:", false),
                                            DiscordEmoji.FromName(ctx.Client, ":four:", false),};
            string optionString = optionEmotes[0] + " | " + Option1 + "\n" +
                                  optionEmotes[1] + " | " + Option2 + "\n" +
                                  optionEmotes[2] + " | " + Option3 + "\n" +
                                  optionEmotes[3] + " | " + Option4;

            var pollMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle(string.Join("", Question))
                    .WithDescription(optionString)
                    .WithColor(DiscordColor.Azure)
                    );
            var putReactOn = await ctx.Channel.SendMessageAsync(pollMessage);

            foreach (var emote in optionEmotes)
            {
                await putReactOn.CreateReactionAsync(emote);
            }

            var result = await interactivity.CollectReactionsAsync(putReactOn, timer);

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            foreach (var emotes in result)
            {
                if (emotes.Emoji == optionEmotes[0])
                {
                    count1++;
                }
                if (emotes.Emoji == optionEmotes[1])
                {
                    count2++;
                }
                if (emotes.Emoji == optionEmotes[2])
                {
                    count3++;
                }
                if (emotes.Emoji == optionEmotes[3])
                {
                    count4++;
                }
            }
            int totalVotes = count1 + count2 + count3 + count4;
            string resultString = optionEmotes[0] + " | " + count1 + " Votes \n" +
                                  optionEmotes[1] + " | " + count2 + " Votes\n" +
                                  optionEmotes[2] + " | " + count3 + " Votes\n" +
                                  optionEmotes[3] + " | " + count4 + " Votes\n\n" +
                                  "There were " + totalVotes + " votes in this poll";

            var resultMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Results of the poll")
                    .WithDescription(resultString)
                    .WithColor(DiscordColor.Green)
                );
            await ctx.Channel.SendMessageAsync(resultMessage);
        }
    }
}