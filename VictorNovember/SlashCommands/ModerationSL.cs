using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VictorNovember.Common;
using VictorNovember.Utilities;

namespace VictorNovember.SlashCommands
{
    public class ModerationSL : ApplicationCommandModule
    {
        #region Kick
        [SlashCommand("kick", "Kick a user")]
        public async Task Kick(InteractionContext ctx, [Option("user", "The user to kick")] DiscordUser user, [Option("reason", "Reason for the kick.")] string reason = null)
        {
            await ctx.DeferAsync();
            var embedMessage = new EmbedCreator(null, null, DiscordColor.Red).GenerateEmbed()
                         .WithAuthor(ctx.Client.CurrentUser.Username, null, ctx.Client.CurrentUser.AvatarUrl);

            if (ctx.Channel.IsPrivate)
            {
                embedMessage.WithTitle("Error!")
                            .WithDescription("This command can only be used in a server!");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;
            }
            var member = (user as DiscordMember);
            if (ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                if (ctx.Client.CurrentUser.Id == user.Id)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("Kick me yourself, coward!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                if (ctx.Member.Id == user.Id)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("You cannot kick yourself!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                if (member.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("That user has a higher position than mine!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                await member.RemoveAsync(reason);

                embedMessage.WithTitle("Kicked!")
                            .WithDescription($"{member.Mention} was kicked from the server!")
                            .WithColor(DiscordColor.Green);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
            else
            {
                embedMessage.WithTitle("Access Denined!")
                            .WithDescription("You don't have the permission to execute this command!");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;
            }
        }
        #endregion

        #region Ban
        [SlashCommand("ban", "Ban a user")]
        public async Task Ban(InteractionContext ctx, [Option("user", "The user to ban")] DiscordUser user, [Option("reason", "Reason for the ban.")] string reason = null, [Option("delete_message_days", "How many days worth of message to delete.")] double day = 0)
        {
            await ctx.DeferAsync();
            var embedMessage = new EmbedCreator(null, null, DiscordColor.Red).GenerateEmbed()
                         .WithAuthor(ctx.Client.CurrentUser.Username, null, ctx.Client.CurrentUser.AvatarUrl);

            if (ctx.Channel.IsPrivate)
            {
                embedMessage.WithTitle("Error!")
                            .WithDescription("This command can only be used in a server!");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;
            }
            var member = (user as DiscordMember);
            if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                if (ctx.Client.CurrentUser.Id == user.Id)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("Ban me yourself, coward!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                if (ctx.Member.Id == user.Id)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("You cannot ban yourself!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                if (member.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("That user has a higher position than mine!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                await member.BanAsync(Convert.ToInt32(day), reason);

                embedMessage.WithTitle("Kicked!")
                            .WithDescription($"{member.Mention} was banned from the server!")
                            .WithColor(DiscordColor.Green);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
            else
            {
                embedMessage.WithTitle("Access Denined!")
                            .WithDescription("You don't have the permission to execute this command!");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;
            }
        }
        #endregion

        #region Timeout
        [SlashCommand("timeout", "Silence a user, default duration is 2 minutes")]
        public async Task Timeout(InteractionContext ctx, [Option("user", "The user to silence")] DiscordUser user, [Option("duration", "Duration of the timeout in seconds")] long duration = 120)
        {
            await ctx.DeferAsync();
            var embedMessage = new EmbedCreator(null, null, DiscordColor.Red).GenerateEmbed()
                         .WithAuthor(ctx.Client.CurrentUser.Username, null, ctx.Client.CurrentUser.AvatarUrl);

            if (ctx.Channel.IsPrivate)
            {
                embedMessage.WithTitle("Error!")
                            .WithDescription("This command can only be used in a server!");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;
            }
            var member = (user as DiscordMember);
            var timeDuration = DateTime.Now + TimeSpan.FromSeconds(duration);
            if (ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                if (ctx.Client.CurrentUser.Id == user.Id)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("I will not be silenced!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                if (ctx.Member.Id == user.Id)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("You cannot mute yourself!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                if (member.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    embedMessage.WithTitle("Error!")
                                .WithDescription("That user has a higher position than mine!");
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                    return;
                }
                await member.TimeoutAsync(timeDuration);

                embedMessage.WithTitle($"{member.Mention} has been timed out!")
                            .WithDescription($"Duration: {TimeSpan.FromSeconds(duration).ToString()}")
                            .WithColor(DiscordColor.Green);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
            else
            {
                embedMessage.WithTitle("Access Denined!")
                            .WithDescription("You don't have the permission to execute this command!");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;
            }
        }
        #endregion

        #region Slow Mode
        [SlashCommand("slowmode", "Sets a channel's slowmode interval to a user's desired ammount")]
        public async Task SlowMode(InteractionContext ctx, [Option("interval", "Sets the slowmode's interval.")] long interval)
        {
            await ctx.DeferAsync();
            var embedMessage = new EmbedCreator(null, null, DiscordColor.Red).GenerateEmbed()
                         .WithAuthor(ctx.Client.CurrentUser.Username, null, ctx.Client.CurrentUser.AvatarUrl);

            if (ctx.Channel.IsPrivate)
            {
                embedMessage.WithTitle("Error!")
                            .WithDescription("This command can only be used in a server!");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;
            }
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                await ctx.Channel.ModifyAsync(x => x.PerUserRateLimit = Convert.ToInt32(interval));
                embedMessage.WithTitle("Slowmode")
                            .WithDescription($"Channel's slowmode interval has been adjusted to {interval} seconds!")
                            .WithColor(DiscordColor.Green);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;

            }
            else
            {
                embedMessage.WithTitle("Access Denined!")
                            .WithDescription("You don't have the permission to execute this command!");
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                return;
            }

        }

        #endregion
    }
}
