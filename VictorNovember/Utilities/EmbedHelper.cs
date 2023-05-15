using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VictorNovember.Utilities
{
    public class EmbedCreator
    {
        // Useful for when creating basic embeds without typing the same thing over and over
        public string Title { get; set; }
        public string Description { get; set; }
        public DiscordColor Color { get; set; }

        public EmbedCreator(string title, string desc, DiscordColor color)
        {
            this.Title = title;
            this.Description = desc;
            this.Color = color;
        }

        public DiscordEmbedBuilder GenerateEmbed()
        {
            var embed = new DiscordEmbedBuilder()
            {
                Title = this.Title,
                Description = this.Description,
                Color = this.Color,
                Timestamp = DateTime.Now
            };

            return embed;
        }
    }
}
