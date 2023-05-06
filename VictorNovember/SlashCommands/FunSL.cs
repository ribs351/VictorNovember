using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using System.Net.Http;
using DSharpPlus.SlashCommands.Attributes;
using Newtonsoft.Json;
using DSharpPlus.Interactivity.Extensions;
using VictorNovember.Common;

namespace VictorNovember.SlashCommands
{
    public class FunSL : ApplicationCommandModule
    {
        readonly string dadJokeAPIkey;

        public FunSL()
        {
            dadJokeAPIkey = Utilities.APIHandler.ReturnSavedValue("dadJokeAPIkey");
        }

        #region Answer
        [SlashCommand("answer", "Answers a yes no question, much like the magic eightball")]
        [SlashCooldown(1, 10, SlashCooldownBucketType.Channel)]
        public async Task Answer(InteractionContext ctx, [Option("string", "Your yes/no question")] string question)
        {
            await ctx.DeferAsync();
            Random random = new Random();
            string output = "";

            if ((question.IndexOf("?", StringComparison.CurrentCultureIgnoreCase) >= 0) == false)
            {
                await Extensions.SendErrorAsync(ctx, "Error!", "That's not a question though? Make a question pls!");
                return;
            }
            switch (question)
            {
                case var s when question.Contains("are you retarded"):
                    output += "You're retarded";
                    await Extensions.SendSuccessAsync(ctx, question, output);
                    break;
                case var s when question.Contains("are you gay"):
                    output += "You are gay";
                    await Extensions.SendSuccessAsync(ctx, question, output);
                    break;
                case var s when question.Contains("how are you"):
                    output += "Fine til I met you, I guess";
                    await Extensions.SendSuccessAsync(ctx, question, output);
                    break;
                case var s when question.Contains("who are you"):
                    output += "Who are you? Why are you asking me this?";
                    await Extensions.SendSuccessAsync(ctx, question, output);
                    break;
                case var s when question.Contains("definition of insanity"):
                    output += "Mentioning one Far Cry game, over and over again";
                    await Extensions.SendSuccessAsync(ctx, question, output);
                    break;
                default: output = Variables.answers[random.Next(0, Variables.answers.Length)];
                    await Extensions.SendSuccessAsync(ctx, question, output);
                    break;
            }
            
        }
        #endregion

        #region Say
        [SlashCommand("say", "Get November to say something")]
        public async Task Say(InteractionContext ctx, [Option("string", "Type in what you want November to say")] string text)
        {
            if ((text.ToString().IndexOf("@") >= 0) == true)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Nice try, pal. I'm not saying anything with that symbol."));
                return;
            }
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(text));
        }
        #endregion

        #region Inspire
        [SlashCommand("inspire", "Generates an inspirational quote from inspirobot")]
        public async Task InspireAced(InteractionContext ctx)
        {
            await ctx.Channel.TriggerTypingAsync();
            await ctx.DeferAsync();

            var client = new HttpClient();
            string img = await client.GetStringAsync("https://inspirobot.me/api?generate=true");

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Be Inspired!",
                ImageUrl = img,
                Color = DiscordColor.Azure
            };
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));

        }
        #endregion

        #region Dadjoke
        [SlashCommand("joke", "Get a random dad joke")]
        public async Task Joker(InteractionContext ctx)
        {
            await ctx.Channel.TriggerTypingAsync();

            await ctx.DeferAsync();

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://dad-jokes.p.rapidapi.com/random/joke"),
                Headers =
                {
                    { "X-RapidAPI-Key", $"{dadJokeAPIkey}" },
                    { "X-RapidAPI-Host", "dad-jokes.p.rapidapi.com" },
                },
            };
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var joke = JsonConvert.DeserializeObject<dynamic>(body);
            //gets the first element of the json's body element, kept forgetting about this lol
            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = joke.body[0].setup.ToString(),
                Description = joke.body[0].punchline.ToString(),
                Color = DiscordColor.Azure
            };
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));

        }
        #endregion

        #region RockPaperScissors
        [SlashCommand("rps", "Play a game of rock paper scissors with November")]
        [SlashCooldown(1, 10, SlashCooldownBucketType.Channel)]
        public async Task RockPaperScissors(InteractionContext ctx, [Choice("Rock", "rock")][Choice("Paper", "paper")][Choice("Scissors", "scissors")][Option("string", "Choose your weapon")] string playerChoice)
        {
            await ctx.Channel.TriggerTypingAsync();
            await ctx.DeferAsync();


            string[] novemberOptions = new string[3] { "rock", "paper", "scissors" };
            int rnd = new Random().Next(1, 4);
            string novemberChoice = novemberOptions[rnd];

            string[] winnerResponses = new string[]{
                            "I've won! Hah!",
                            "You've done well to lose against me.",
                            "Outplayed! Don't feel bad, I'm just that great yknow?"
                        };
            string[] loserResponses = new string[]{
                            "Aww man, I lost!",
                            "Dammit!",
                            "One more go, I'll get it next time!"
                        };

            //display the user choice

            var playerChoiceMessage = new DiscordEmbedBuilder()
            {
                Title = "You went with:",
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = ctx.User.AvatarUrl,
                    Name = ctx.User.Username
                },
                Description = $"```{playerChoice}```",
                Color = DiscordColor.Azure,
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(playerChoiceMessage));

            var novemberChoiceMessage = new DiscordEmbedBuilder()
            {
                Title = "I picked:",
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = ctx.Client.CurrentUser.AvatarUrl,
                    Name = ctx.Client.CurrentUser.Username
                },
                Description = $"```{novemberChoice}```",
                Color = DiscordColor.Azure,
            };


            await ctx.Channel.SendMessageAsync(embed: novemberChoiceMessage);

            //game logic be like
            if (playerChoice == novemberChoice)
            {
                var drawMessage = new DiscordEmbedBuilder()
                {
                    Title = "Draw!",
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        IconUrl = ctx.Client.CurrentUser.AvatarUrl,
                        Name = ctx.Client.CurrentUser.Username
                    },
                    Description = "Draw! There were no winners!",
                    Color = DiscordColor.Yellow,
                };

                await ctx.Channel.SendMessageAsync(embed: drawMessage);
            }
            else if (playerChoice == "rock" && novemberChoice == "paper" || playerChoice == "scissors" && novemberChoice == "rock" || playerChoice == "paper" && novemberChoice == "scissors")
            {
                var novemberWonMessage = new DiscordEmbedBuilder()
                {
                    Title = "You've lost!",
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        IconUrl = ctx.Client.CurrentUser.AvatarUrl,
                        Name = ctx.Client.CurrentUser.Username
                    },
                    Description = $"{winnerResponses[rnd]}",
                    Color = DiscordColor.Red,
                };

                await ctx.Channel.SendMessageAsync(embed: novemberWonMessage);
            }
            else
            {
                var playerWonMessage = new DiscordEmbedBuilder()
                {
                    Title = "You won!",
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        IconUrl = ctx.Client.CurrentUser.AvatarUrl,
                        Name = ctx.Client.CurrentUser.Username
                    },
                    Description = $"{loserResponses[rnd]}",
                    Color = DiscordColor.Green,
                };

                await ctx.Channel.SendMessageAsync(embed: playerWonMessage);
            }

        }
        #endregion

        #region PP
        [SlashCommand("pp", "Get November to measure your pp")]
        [SlashCooldown(1, 10, SlashCooldownBucketType.Channel)]
        public async Task PP(InteractionContext ctx)
        {

            await ctx.DeferAsync();

            if (ctx.User.Id == 502023592083718145)
            {
                var embedNegevMessage = new DiscordEmbedBuilder()
                {
                    Title = "Error!",
                    Description = "PP not found!",
                    Color = DiscordColor.Azure,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = ctx.User.AvatarUrl,
                        Text = $"PP | Requested by {ctx.User.Username}"
                    }
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedNegevMessage));

                return;
            }


            int size = new Random().Next(0, 16);
            string pp = "=";
            for (int i = 0; i < size; i++)
            {
                pp += "=";
            }

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "PP",
                Description = $"{ctx.User.Username}'s pp looks like:" + $"```8{pp}D```",
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = ctx.User.AvatarUrl,
                    Text = $"PP | Requested by {ctx.User.Username}"
                }
            };
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        #endregion

        #region RR
        [SlashCommand("rr", "Play a game of Russian Roulette")]
        [SlashCooldown(1, 10, SlashCooldownBucketType.Channel)]
        public async Task RussianRoulette(InteractionContext ctx, [Choice("One", 1)][Choice("Two", 2)][Choice("Three", 3)][Choice("Four", 4)][Choice("Five", 5)][Choice("Six", 6)][Option("double", "Decide how many bullets you want to play with")] double amount)
        {
            //Lets the user choose how many bullets they want to load
            await ctx.DeferAsync();
            if (ctx.Channel.IsPrivate)
            {
                // if channel is a DM, then say no
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in a server, where the stakes are present."));
                return;
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{ctx.User.Username} loads {amount.ToString()} bullet(s) and spins the cylinder..."));            
            await Task.Delay(1000);
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{ctx.User.Username} puts the gun up to their head and pulls the trigger..."));

            //game logic goes here, the chances of the gun going off get higher the more bullets are loaded, with 6 being an instant kill lmao
            var death = (amount / 6) * 100;
            var rand = new Random().Next(100);

            if (rand < death)
            {
                //get the time out duration
                var timeDuration = DateTime.Now + TimeSpan.FromMinutes(2);
                //casts the user as DiscordMember because guild bs
                var user = (ctx.User as DiscordMember);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("BANG!"));
                await Task.Delay(500);

                //if the gun goes off but the User is ranked higher than the bot, this happens
                if (user.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"The gun fired! But it bounced off of {ctx.User.Username}'s head! Their skull is just too thick"));
                    return;
                }
                else
                //if the gun goes off, user is timed out, gets sent a DM saying that they're timed out
                {
                    await user.TimeoutAsync(timeDuration);
                    var DMChannel = await ctx.Member.CreateDmChannelAsync();
                    await DMChannel.SendMessageAsync($"You've been muted in {ctx.Guild.Name} for 2 minutes. You've died in a game of Russian Roulette. Try again if you dare.");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"The chamber was loaded! {ctx.User.Username} shot themself in the head!"));
                    return;
                }

            }
            else
            //the gun doesn't go off, user lives
            {
                await Task.Delay(1000);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("*clicks*"));
                await Task.Delay(500);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"The chamber was empty! {ctx.User.Username} has survived!"));
                return;
            }
        }
        #endregion

        #region HRR
        [SlashCommand("hrr", "Play a game of hardcore Russian Roulette, if you die you will be kicked from the server!")]
        [SlashCooldown(1, 10, SlashCooldownBucketType.Channel)]
        public async Task HardcoreRussianRoulette(InteractionContext ctx, [Choice("One", 1)][Choice("Two", 2)][Choice("Three", 3)][Choice("Four", 4)][Choice("Five", 5)][Choice("Six", 6)][Option("double", "Decide how many bullets you want to play with")] double amount)
        {
            //the same has RR but instead of getting timed out they just get kicked lol
            await ctx.DeferAsync();
            if (ctx.Channel.IsPrivate)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in a server, where the stakes are present."));
                return;
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{ctx.User.Username} loads {amount.ToString()} bullet(s) and spins the cylinder..."));
            await Task.Delay(1000);
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{ctx.User.Username} puts the gun up to their head and pulls the trigger..."));

            var death = (amount / 6) * 100;
            var rand = new Random().Next(100);

            if (rand < death)
            {

                var user = (ctx.User as DiscordMember);
                var guildChannel = ctx.Channel;
                var inviteLink = await guildChannel.CreateInviteAsync();
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("BANG!"));
                await Task.Delay(500);
                if (user.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"The gun fired! But it bounced off of {ctx.User.Username}'s head! Their skull is just too thick"));
                    return;
                }
                else
                {
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"The chamber was loaded! {ctx.User.Username} shot themself in the head!"));
                    await user.RemoveAsync("Played Hardcore Russian Roulette and died");
                    var DMChannel = await ctx.Member.CreateDmChannelAsync();
                    await DMChannel.SendMessageAsync($"You've been kicked from {ctx.Guild.Name}. You've died in a game of Russian Roulette. Try again if you dare!");
                    await DMChannel.SendMessageAsync($"https://discord.gg/{inviteLink.Code}");

                    return;
                }

            }
            else
            {
                await Task.Delay(1000);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("*clicks*"));
                await Task.Delay(500);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"The chamber was empty! {ctx.User.Username} has survived!"));
                return;
            }
        }
        #endregion

        #region HiddenPoll
        //[SlashCommand("poll", "Create your own poll")]
        //[Hidden]
        //public async Task PollCommand(InteractionContext ctx, [Option("question", "The main poll subject/question")] string Question,
        //                                                     [Option("timelimit", "The time set on this poll")] long TimeLimit,
        //                                                     [Option("option1", "Option 1")] string Option1,
        //                                                     [Option("option2", "Option 1")] string Option2,
        //                                                     [Option("option3", "Option 1")] string Option3,
        //                                                     [Option("option4", "Option 1")] string Option4)
        //{
        //    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
        //                                                                        .WithContent("..."));

        //    var interactvity = ctx.Client.GetInteractivity(); //Getting the Interactivity Module
        //    TimeSpan timer = TimeSpan.FromSeconds(TimeLimit); //Converting my time parameter to a timespan variable

        //    DiscordEmoji[] optionEmojis = { DiscordEmoji.FromName(ctx.Client, ":one:", false),
        //                                    DiscordEmoji.FromName(ctx.Client, ":two:", false),
        //                                    DiscordEmoji.FromName(ctx.Client, ":three:", false),
        //                                    DiscordEmoji.FromName(ctx.Client, ":four:", false) }; //Array to store discord emojis

        //    string optionsString = optionEmojis[0] + " | " + Option1 + "\n" +
        //                           optionEmojis[1] + " | " + Option2 + "\n" +
        //                           optionEmojis[2] + " | " + Option3 + "\n" +
        //                           optionEmojis[3] + " | " + Option4; //String to display each option with its associated emojis

        //    var pollMessage = new DiscordMessageBuilder()
        //        .AddEmbed(new DiscordEmbedBuilder()

        //        .WithColor(DiscordColor.Azure)
        //        .WithTitle(string.Join(" ", Question))
        //        .WithDescription(optionsString)
        //        ); //Making the Poll message

        //    var putReactOn = await ctx.Channel.SendMessageAsync(pollMessage); //Storing the await command in a variable

        //    foreach (var emoji in optionEmojis)
        //    {
        //        await putReactOn.CreateReactionAsync(emoji); //Adding each emoji from the array as a reaction on this message
        //    }

        //    var result = await interactvity.CollectReactionsAsync(putReactOn, timer); //Collects all the emoji's and how many peopele reacted to those emojis

        //    int count1 = 0; //Counts for each emoji
        //    int count2 = 0;
        //    int count3 = 0;
        //    int count4 = 0;

        //    foreach (var emoji in result) //Foreach loop to go through all the emojis in the message and filtering out the 4 emojis we need
        //    {
        //        if (emoji.Emoji == optionEmojis[0])
        //        {
        //            count1++;
        //        }
        //        if (emoji.Emoji == optionEmojis[1])
        //        {
        //            count2++;
        //        }
        //        if (emoji.Emoji == optionEmojis[2])
        //        {
        //            count3++;
        //        }
        //        if (emoji.Emoji == optionEmojis[3])
        //        {
        //            count4++;
        //        }
        //    }

        //    int totalVotes = count1 + count2 + count3 + count4;

        //    string resultsString = optionEmojis[0] + ": " + count1 + " Votes \n" +
        //               optionEmojis[1] + ": " + count2 + " Votes \n" +
        //               optionEmojis[2] + ": " + count3 + " Votes \n" +
        //               optionEmojis[3] + ": " + count4 + " Votes \n\n" +
        //               "The total number of votes is " + totalVotes; //String to show the results of the poll

        //    var resultsMessage = new DiscordMessageBuilder()
        //        .AddEmbed(new DiscordEmbedBuilder()

        //        .WithColor(DiscordColor.Green)
        //        .WithTitle("Results of Poll")
        //        .WithDescription(resultsString)
        //        );

        //    await ctx.Channel.SendMessageAsync(resultsMessage); //Making the embed and sending it off            
        //}
        #endregion

        #region Caption
        [SlashCommand("caption", "Give any image a Caption")]
        public async Task CaptionCommand(InteractionContext ctx, [Option("caption", "The caption you want the image to have")] string caption,
                                                                 [Option("image", "The image you want to upload")] DiscordAttachment picture)
        {
            await ctx.DeferAsync();

            var captionMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Azure)
                    .WithTitle(caption)
                    .WithImageUrl(picture.Url)
                    );

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(captionMessage.Embed));
        }
        #endregion

        #region Neko
        [SlashCommand("neko", "Rate a random image of a neko")]
        [SlashCooldown(1, 10, SlashCooldownBucketType.Channel)]
        public async Task Neko(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            if ((!ctx.Channel.IsPrivate) && ctx.Channel.Guild.Id == 580555191938711580)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Sorry bud, not on this server :/"));
                return;
            }

            var client = new HttpClient();
            var result = await client.GetStringAsync("https://nekos.moe/api/v1/random/image?count=1&nsfw=false");
            if (result == null)
            {
                await ctx.Channel.SendMessageAsync("Something went wrong with the Neko API!");
                return;
            }
            var neko = JsonConvert.DeserializeObject<dynamic>(result);

            var embedMessage = new DiscordEmbedBuilder()
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = "Bruh moment",
                    IconUrl = "https://nekos.moe/static/favicon/favicon-32x32.png"
                },
                Title = "Smash or pass? | You have 15 seconds to vote",
                ImageUrl = $"https://nekos.moe/image/{neko.images[0].id.ToString()}",
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"Neko | Requested by {ctx.User.Username}#{ctx.User.Discriminator}",
                    IconUrl = ctx.User.AvatarUrl
                }
            };

            var putReactOn = await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage)); //Storing the await command in a variable

            var interactvity = ctx.Client.GetInteractivity(); //Getting the Interactivity Module
            TimeSpan timer = TimeSpan.FromSeconds(15);

            DiscordEmoji[] optionEmojis = { DiscordEmoji.FromName(ctx.Client, ":thumbsup:", false),
                                            DiscordEmoji.FromName(ctx.Client, ":thumbsdown:", false)}; //Array to store discord emojis

            foreach (var emoji in optionEmojis)
            {
                await putReactOn.CreateReactionAsync(emoji); //Adding each emoji from the array as a reaction on this message
            }

            var pollResult = await interactvity.CollectReactionsAsync(putReactOn, timer); //Collects all the emoji's and how many peopele reacted to those emojis

            int count1 = 0; //Counts for each emoji
            int count2 = 0;

            foreach (var emoji in pollResult) //Foreach loop to go through all the emojis in the message and filtering out the 4 emojis we need
            {
                if (emoji.Emoji == optionEmojis[0])
                {
                    count1++;
                }
                if (emoji.Emoji == optionEmojis[1])
                {
                    count2++;
                }
            }

            int totalVotes = count1 + count2;

            string resultsString = optionEmojis[0] + ": " + count1 + " Votes \n\n" +
                       optionEmojis[1] + ": " + count2 + " Votes \n\n" +
                       "A total of " + totalVotes + " users voted in this session"; //String to show the results of the poll

            var resultsMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Green)
                .WithTitle("Results of Poll")
                .WithDescription(resultsString)
                );

            await ctx.Channel.SendMessageAsync(resultsMessage); //Making the embed and sending it off     

            var verdictMessage = new DiscordEmbedBuilder()
            {
                Title = $"Smash or Pass?",
                Color = DiscordColor.Azure
            };

            if (count1 > count2)
            {
                verdictMessage.WithDescription($"Smash wins with {count1} votes!");
                await ctx.Channel.SendMessageAsync(verdictMessage);
                return;
            }
            if (count2 > count1)
            {
                verdictMessage.WithDescription($"Pass wins with {count2} votes!");
                await ctx.Channel.SendMessageAsync(verdictMessage);
                return;
            }
            if (count1 == 0 && count2 == 0)
            {
                verdictMessage.WithDescription("Nobody voted.");
                await ctx.Channel.SendMessageAsync(verdictMessage);
                return;
            }
            if (count1 == count2)
            {
                verdictMessage.WithDescription("Results inconclusive.");
                await ctx.Channel.SendMessageAsync(verdictMessage);
                return;
            }
        }
        #endregion

        #region Slash L
        [SlashCommand("l", "Leaves the server")]
        public async Task Leave(InteractionContext ctx)
        {
            if (ctx.Channel.IsPrivate)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in a server."));
                return;
            }

            var user = (ctx.User as DiscordMember);

            if (user.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You can not leave the server."));
                return;
            }

            var guildChannel = ctx.Channel;
            var inviteLink = await guildChannel.CreateInviteAsync();
            var DMChannel = await ctx.Member.CreateDmChannelAsync();
            await DMChannel.SendMessageAsync("With this character's death, the thread of prophecy is severed. Restore a saved game to restore the weave of fate, or persist in the doomed world you have created.");
            await DMChannel.SendMessageAsync($"https://discord.gg/{inviteLink.Code}");

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Goodbye temporarily!"));
            await user.RemoveAsync();
            return;
        }
        #endregion

        #region Slash Q
        [SlashCommand("q", "Leaves the server FOREVER")]
        public async Task GoodbyeForever(InteractionContext ctx)
        {
            if (ctx.Channel.IsPrivate)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in a server."));
                return;
            }

            var user = (ctx.User as DiscordMember);

            if (user.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You can not leave the server."));
                return;
            }


            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Goodbye forever!"));
            var DMChannel = await ctx.Member.CreateDmChannelAsync();
            await DMChannel.SendMessageAsync("Goodbye forever!");
            await user.BanAsync();
            return;
        }
        #endregion
    }
}
