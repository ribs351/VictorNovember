using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VictorNovember.Common;

namespace VictorNovember.SlashCommands
{
    public class ModerationSL : ApplicationCommandModule
    {
        #region Kick
        [SlashCommand("kick", "Kick a user")]
        public async Task Kick(InteractionContext ctx, [Option("user", "The user to kick")] DiscordUser user, [Option("reason", "Reason for the kick.")] string reason = null)
        {
            await ctx.DeferAsync();
            if (ctx.Channel.IsPrivate)
            {
                await Extensions.SendErrorAsync(ctx, "Error!", "This command can only be used in a server!");
                return;
            }
            var member = (user as DiscordMember);
            if (ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                if (ctx.Client.CurrentUser.Id == user.Id)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "Kick me yourself, coward!");
                    return;
                }
                if (ctx.Member.Id == user.Id)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "You cannot kick yourself!");
                    return;
                }
                if (member.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "That user has a higher position than the mine!");
                    return;
                }
                await member.RemoveAsync(reason);
                await Extensions.SendSuccessAsync(ctx, "Kicked!", $"{member.Mention} was kicked from the server!");
            }
            else
            {
                await Extensions.SendErrorAsync(ctx, "Access Denined!", "You don't have the permission to execute this command!");
            }
        }
        #endregion

        #region Ban
        [SlashCommand("ban", "Ban a user")]
        public async Task Ban(InteractionContext ctx, [Option("user", "The user to ban")] DiscordUser user, [Option("reason", "Reason for the ban.")] string reason = null, [Option("delete_message_days", "How many days worth of message to delete.")] double day = 0)
        {
            await ctx.DeferAsync();
            if (ctx.Channel.IsPrivate)
            {
                await Extensions.SendErrorAsync(ctx, "Error!", "This command can only be used in a server!");
                return;
            }
            var member = (user as DiscordMember);
            if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                if (ctx.Client.CurrentUser.Id == user.Id)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "Ban me yourself, coward!");
                    return;
                }
                if (ctx.Member.Id == user.Id)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "You cannot ban yourself!");
                    return;
                }
                if (member.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "That user has a higher position than the mine!");
                    return;
                }
                await member.BanAsync(Convert.ToInt32(day), reason);
                await Extensions.SendSuccessAsync(ctx, "Banned!", $"{member.Mention} was banned from the server!");
            }
            else
            {
                await Extensions.SendErrorAsync(ctx, "Access Denined!", "You don't have the permission to execute this command!");
            }
        }
        #endregion

        #region Timeout
        [SlashCommand("timeout", "Silence a user, default duration is 2 minutes")]
        public async Task Timeout(InteractionContext ctx, [Option("user", "The user to silence")] DiscordUser user, [Option("duration", "Duration of the timeout in seconds")] long duration = 120)
        {
            await ctx.DeferAsync();
            if (ctx.Channel.IsPrivate)
            {
                await Extensions.SendErrorAsync(ctx, "Error!", "This command can only be used in a server!");
                return;
            }
            var member = (user as DiscordMember);
            var timeDuration = DateTime.Now + TimeSpan.FromSeconds(duration);
            if (ctx.Member.Permissions.HasPermission(Permissions.KickMembers))
            {
                if (ctx.Client.CurrentUser.Id == user.Id)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "I will not be silenced!");
                    return;
                }
                if (ctx.Member.Id == user.Id)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "You cannot mute yourself!");
                    return;
                }
                if (member.Hierarchy > ctx.Guild.CurrentMember.Hierarchy)
                {
                    await Extensions.SendErrorAsync(ctx, "Error!", "That user has a higher position than the mine!");
                    return;
                }
                await member.TimeoutAsync(timeDuration);
                await Extensions.SendSuccessAsync(ctx, $"{member.Mention} has been timed out!", $"Duration: {TimeSpan.FromSeconds(duration).ToString()}");
            }
            else
            {
                await Extensions.SendErrorAsync(ctx, "Access Denined!", "You don't have the permission to execute this command!");
            }
        }
        #endregion
    }
}
