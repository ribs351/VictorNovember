﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VictorNovember.Common
{
    public static class Extensions
    {
        // Experimented with extention methods, these work but I didn't like how they turned out, so currently these methods are unused
        public static async Task<DiscordMessage> SendSuccessAsync(InteractionContext ctx, string title, string description)
        {
            var embedMessage = new DiscordEmbedBuilder()
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = title,
                    IconUrl = "http://clipart-library.com/image_gallery2/Success-PNG-Image.png"
                },
                Description = description,
                Color = DiscordColor.Green
            };
            return await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        public static async Task<DiscordMessage> SendErrorAsync(InteractionContext ctx, string title, string description)
        {
            var embedMessage = new DiscordEmbedBuilder()
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = title,
                    IconUrl = "https://cdn.icon-icons.com/icons2/1380/PNG/512/vcsconflicting_93497.png"
                },
                Description = description,
                Color = DiscordColor.Red
            };
            return await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        public static async Task<DiscordEmbed> CreateBasicEmbed(string title, string description, DiscordColor color)
        {
            var embed = await Task.Run(() => new DiscordEmbedBuilder()
                .WithTitle(title)
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(color)
                .WithTimestamp(DateTime.Now)
                .Build());
            return embed;
        }
    }
}
