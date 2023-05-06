using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.SlashCommands.Attributes;
using System.Diagnostics;
using System.Net.Http;
using OpenAI_API;

namespace VictorNovember.SlashCommands
{
    public class GeneralSL : ApplicationCommandModule
    {
        readonly string ChatGPTAPIkey;

        public GeneralSL()
        {
            ChatGPTAPIkey = Utilities.APIHandler.ReturnSavedValue("ChatGPTAPIkey");
        }

        #region Avatar
        [SlashCommand("avatar", "Get a user's profile picture")]
        public async Task Avatar(InteractionContext ctx, [Option("user", "The user to fetch avatar from")] DiscordUser user)
        {
            await ctx.DeferAsync();
            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = $"{user.Username}#{user.Discriminator}'s avatar",
                ImageUrl = user.AvatarUrl,
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"Avatar | Requested by {ctx.User.Username}#{ctx.User.Discriminator}",
                    IconUrl = ctx.User.AvatarUrl
                }
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        #endregion

        #region Help
        [SlashCommand("help", "Get an explanation of November's commands")]
        public async Task Help(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var funBtn = new DiscordButtonComponent(ButtonStyle.Success, "funBtn", "Fun");
            var generalBtn = new DiscordButtonComponent(ButtonStyle.Success, "generalBtn", "General");
            var spaceBtn = new DiscordButtonComponent(ButtonStyle.Success, "spaceBtn", "Space");

            var helpEmbed = new DiscordEmbedBuilder()
            {
                Title = $"Help Menu",
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = ctx.Client.CurrentUser.AvatarUrl,
                    Name = ctx.Client.CurrentUser.Username
                },
                Description = "Select a button for more information on the commands",
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"Help | Requested by {ctx.User.Username}#{ctx.User.Discriminator}",
                    IconUrl = ctx.User.AvatarUrl
                }
            };


            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(helpEmbed).AddComponents(generalBtn, funBtn, spaceBtn));
        }
        #endregion

        #region Stats
        [SlashCommand("stats", "Get November's current status")]
        public async Task Status(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var time = DateTime.Now - Process.GetCurrentProcess().StartTime;
            var upTime = "";

            if (time.Days > 0)
            {
                if (time.Hours <= 0 || time.Minutes <= 0)
                {
                    upTime += $"{time.Days} Day(s) and ";
                }
                else
                {
                    upTime += $"{time.Days} Day(s),";
                }
            }

            if (time.Hours > 0)
            {
                if (time.Minutes > 0)
                {
                    upTime += $" {time.Hours} Hour(s), ";
                }
                else
                {
                    upTime += $"{time.Hours} Hour(s) And ";
                }
            }

            if (time.Minutes > 0)
            {
                upTime += $"{time.Minutes} Minute(s)";
            }

            if (time.Seconds >= 0)
            {
                if (time.Hours > 0 || time.Minutes > 0)
                {
                    upTime += $" And {time.Seconds} Second(s)";
                }
                else
                {
                    upTime += $"{time.Seconds} Second(s)";
                }
            }

            var process = Process.GetCurrentProcess();
            var mem = process.PrivateMemorySize64;
            var memory = mem / 1024 / 1024;
            var totalUsers = ctx.Client.Guilds.Sum(guild => guild.Value.MemberCount);

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Current Status",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = ctx.Client.CurrentUser.AvatarUrl,
                    Width = 100,
                    Height = 100
                },
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"Stats | Requested by {ctx.User.Username}#{ctx.User.Discriminator}",
                    IconUrl = ctx.User.AvatarUrl
                }
            };
            embedMessage.AddField("***Ping***", $"```fix\n{ctx.Client.Ping}ms```", true)
                        .AddField("***Total Servers***", $"```fix\n{ctx.Client.Guilds.Count()}```", true)
                        .AddField("***Total Users***", $"```fix\n{totalUsers}```", true)
                        .AddField("***Up Time***", $"```fix\n{upTime}```", true)
                        .AddField("***Memory Usage***", $"```fix\n{memory} MB```", true)
                        .WithTimestamp(DateTimeOffset.UtcNow);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        #endregion

        #region Info
        [SlashCommand("info", "Get some information on a user")]
        public async Task Info(InteractionContext ctx, [Option("user", "The user to fetch info from")] DiscordUser user)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = $"{user.Username}#{user.Discriminator}'s files",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = user.AvatarUrl,
                    Width = 100,
                    Height = 100
                },
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"Info | Requested by {user.Username}#{user.Discriminator}",
                    IconUrl = user.AvatarUrl
                }
            };
            if (ctx.Channel.IsPrivate)
            {
                embedMessage.AddField("***User ID***", $"```{ctx.User.Id}```", true)
                            .AddField("***Created at***", $"```{ctx.User.CreationTimestamp.ToString("dd/mm/yyyy")}```", true);
            }
            else
            {
                embedMessage.AddField("***User ID***", $"```{ctx.User.Id}```", true)
                            .AddField("***Created at***", $"```{ctx.User.CreationTimestamp.ToString("dd/mm/yyyy")}```", true)
                            .AddField("***Join date***", $"```{(ctx.User as DiscordMember).JoinedAt.ToString("dd/mm/yyyy")}```", true)
                            .AddField("***Roles***", string.Join(" ", (ctx.User as DiscordMember).Roles.Select(x => x.Mention)), true);
            }


            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        #endregion

        #region Server Info
        [SlashCommand("serverinfo", "Get the current server's info")]
        [SlashRequireGuild]

        public async Task ServerInfo(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder()
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = ctx.Guild.IconUrl,
                    Name = ctx.Guild.Name
                },
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = ctx.Guild.IconUrl,
                    Width = 50,
                    Height = 50
                },
                Title = $"Server Info: ",
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"ID: {ctx.Guild.Id} | Created on • {ctx.Guild.CreationTimestamp.ToString("dd/MM/yyyy")}"
                }
            };
            embedMessage.AddField("***Owner***", $"```{ctx.Guild.Owner.Username}#{ctx.Guild.Owner.Discriminator}```", true)
                        .AddField("***Category Channels***", $"```{ctx.Guild.GetChannelsAsync().Result.Where(x => x.IsCategory).Count()}```", true)
                        .AddField("***Text Channels***", $"```{ctx.Guild.GetChannelsAsync().Result.Where(x => x.Type.ToString() == "Text").Count()}```", true)
                        .AddField("***Voice Channels***", $"```{ctx.Guild.GetChannelsAsync().Result.Where(x => x.Type.ToString() == "Voice").Count()}```", true)
                        .AddField("***Member count***", $"```{ctx.Guild.MemberCount}```", true)
                        .AddField("***Roles***", $"```{ctx.Guild.Roles.Count()}```", true)
                        .AddField("***Role List***", string.Join(" ", ctx.Guild.Roles.Select(x => x.Value.Mention)), false);
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        #endregion

        #region ChatGPT
        [SlashCommand("chatgpt", "Ask chatgpt a question")]
        public async Task ChatGPT(InteractionContext ctx, [Option("query", "What is your question?")] string query)
        {
            try
            {
                await ctx.DeferAsync();
                var api = new OpenAIAPI(ChatGPTAPIkey);

                var chat = api.Chat.CreateConversation();
                chat.AppendSystemMessage("Type in a query");

                chat.AppendUserInput(query);

                string response = await chat.GetResponseFromChatbotAsync();
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        Name = "ChatGPT",
                        IconUrl = "https://cdn.discordapp.com/attachments/767653326560821248/1103270905314033664/open-ai-logo-8B9BFEDC26-seeklogo.com.png"
                    },
                    Description = $"> {query}\n\n {response}",
                    Color = DiscordColor.Azure,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = $"ChatGPT | Requested by {ctx.User.Username}#{ctx.User.Discriminator}",
                        IconUrl = ctx.User.AvatarUrl
                    }
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
            catch (Exception e)
            {
                await ctx.Channel.SendMessageAsync($"Something went wrong:```fix\n{e.ToString()}```");
            }
        }
        #endregion

        #region DALL-E
        [SlashCommand("imagine", "Generate an image using DALL·E")]
        public async Task DALLE(InteractionContext ctx, [Option("query", "What is your prompt?")] string query)
        {
            try
            {
                await ctx.DeferAsync();
                var api = new OpenAIAPI(ChatGPTAPIkey);
                var result = await api.ImageGenerations.CreateImageAsync(query);
                var embedMessage = new DiscordEmbedBuilder()
                {
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        Name = "DALL·E",
                        IconUrl = "https://openailabs-site.azureedge.net/public-assets/d/1a1d80e550/apple-touch-icon.png"
                    },
                    ImageUrl = result.Data[0].Url,
                    Color = DiscordColor.Azure,
                    Description = $"> {query}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = $"DALL·E | Requested by {ctx.User.Username}#{ctx.User.Discriminator}",
                        IconUrl = ctx.User.AvatarUrl
                    }
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
            catch (Exception e)
            {
                await ctx.Channel.SendMessageAsync($"Something went wrong:```fix\n{e.ToString()}```");
            }
        }
        #endregion

        #region GetOut
        [SlashCommand("getout", "Makes Novemeber leave the current server.")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task LeaveServer(InteractionContext ctx)
        {
            var embedMessage = new DiscordEmbedBuilder()
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = ctx.Client.CurrentUser.Username,
                    IconUrl = ctx.Client.CurrentUser.AvatarUrl
                },
                Title = "Goodbye!",
                Description = $"Thank you for using {ctx.Client.CurrentUser.Username}. Have a nice day!",
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"GetOut | Requested by {ctx.User.Username}#{ctx.User.Discriminator}",
                    IconUrl = ctx.User.AvatarUrl
                }
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            await ctx.Guild.LeaveAsync().ConfigureAwait(false);
        }
        #endregion
    }
}
