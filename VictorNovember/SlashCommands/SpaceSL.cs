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
using System.Net.Http;
using Newtonsoft.Json;
using VictorNovember.Utilities;

namespace VictorNovember.SlashCommands
{
    public class SpaceSL : ApplicationCommandModule
    {
        readonly string NASA_APIkey;

        public SpaceSL()
        {
            NASA_APIkey = Utilities.APIHandler.ReturnSavedValue("NASA_APIkey");
        }
        #region APOD
        [SlashCommand("apod", "Returns a Picture of the Day from NASA")]
        public async Task APOD(InteractionContext ctx)
        {
            try
            {
                await ctx.DeferAsync();
                await ctx.Channel.TriggerTypingAsync();
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.nasa.gov/planetary/apod?api_key={NASA_APIkey}"),
                };
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var apod = JsonConvert.DeserializeObject<dynamic>(body);

                var embedMessage = new EmbedCreator(apod.title.ToString(), apod.explanation.ToString(), DiscordColor.Azure).GenerateEmbed()
                    .WithAuthor("NASA's Astronomy Picture of the Day", null, "https://cdn.discordapp.com/attachments/767653326560821248/1101251469400866877/NASA_logo.png")
                    .WithFooter($"APOD | Requested by {ctx.User.Username}#{ctx.User.Discriminator}", ctx.User.AvatarUrl)
                    .WithImageUrl(apod.url.ToString());
               
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
            }
            catch (Exception e)
            {
                await ctx.Channel.SendMessageAsync($"Something went wrong:```fix\n{e.ToString()}```");
            }

        }
        #endregion
    }
}
