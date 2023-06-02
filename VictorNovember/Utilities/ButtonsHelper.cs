using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VictorNovember.Utilities
{
    // Separate this from the event helper as there will be a lot more buttons in the future so it makes sense for this to have its own class
    internal static class ButtonsHelper
    {
        internal static async Task ButtonPressResponse(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {

            switch (e.Interaction.Data.CustomId)
            {
                case "1":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("Your mom gay"));
                    break;
                case "2":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("Your mom gayer"));
                    break;
                case "generalBtn":
                    {
                        string generalList = "stats -> Get November's current status\n"
                                           + "info -> Get some information on a user\n"
                                           + "serverinfo -> Get the current server's info\n"
                                           + "avatar -> Get a user's profile picture\n"
                                           + "help -> Get an explanation of November's commands\n";
                        var generalCommandList = new DiscordEmbedBuilder()
                        {
                            Title = "General Command List",
                            Description = $"```fix\n{generalList}```",
                            Color = DiscordColor.Azure,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = $"Help | Requested by {e.User.Username}#{e.User.Discriminator}",
                                IconUrl = e.User.AvatarUrl
                            }
                        };

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(generalCommandList));
                    }
                    break;
                case "funBtn":
                    {
                        string funList = "say -> Get November to say something\n"
                                       + "inspire -> Generates an inspirational quote from inspirobot\n"
                                       + "joke -> Get a random dad joke\n"
                                       + "rps -> Play a game of rock paper scissors with November\n"
                                       + "pp -> Get November to measure your pp\n"
                                       + "rr -> Play a game of Russian Roulette\n"
                                       + "hrr -> Play a game of hardcore Russian Roulette, if you die you will be kicked from the server!\n"
                                       + "caption -> Give any image a Caption\n"
                                       + "neko -> Rate a random image of a neko\n"
                                       + "l -> Goodbye Temporarily\n"
                                       + "q -> Goodbye Forever\n";
                        var funCommandList = new DiscordEmbedBuilder()
                        {
                            Title = "Fun Command List",
                            Description = $"```fix\n{funList}```",
                            Color = DiscordColor.Azure,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = $"Help | Requested by {e.User.Username}#{e.User.Discriminator}",
                                IconUrl = e.User.AvatarUrl
                            }
                        };

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(funCommandList));
                    }
                    break;
                case "moderationBtn":
                    {
                        string spaceList = "kick -> Kick a user\n"
                                         + "ban -> Ban a user\n"
                                         + "timeout -> Silence a user, default duration is 2 minutes\n"
                                         + "slowmode -> Sets a channel's slowmode interval to a user's desired ammount\n";

                        var spaceCommandList = new DiscordEmbedBuilder()
                        {
                            Title = "General Command List",
                            Description = $"```fix\n{spaceList}```",
                            Color = DiscordColor.Azure,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = $"Help | Requested by {e.User.Username}#{e.User.Discriminator}",
                                IconUrl = e.User.AvatarUrl
                            }
                        };

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(spaceCommandList));
                    }
                    break;
                case "spaceBtn":
                    {
                        string spaceList = "apod -> Returns a Picture of the Day from NASA\n";

                        var spaceCommandList = new DiscordEmbedBuilder()
                        {
                            Title = "General Command List",
                            Description = $"```fix\n{spaceList}```",
                            Color = DiscordColor.Azure,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = $"Help | Requested by {e.User.Username}#{e.User.Discriminator}",
                                IconUrl = e.User.AvatarUrl
                            }
                        };

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(spaceCommandList));
                    }
                    break;
            }
        }
    }
}
