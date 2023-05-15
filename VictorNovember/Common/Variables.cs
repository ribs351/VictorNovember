using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VictorNovember.Common
{
    public class Variables
    {
        public static string[] answers = 
        {
                "Yes",
                "No",
                "Maybe",
                "dunno",
                "Yesn't",
                "Perhaps",
                "Possibly",
                "Positively",
                "Conceivably",
                "I don't feel like answering right now, try again later",
                "In your dreams",
                "You sure you want to know the answer to that?",
                "*cricket noises*",
                "W-W-why would you ask that?"
        };
        public static Dictionary<string, string> emotes = new Dictionary<string, string>
        {
            {"kekw", "<:kekw:986585139234631700>"},
            {"whatspin","<a:whatspin:1103004399569211453>"},
            {"kekwshook","<:kekwshook:965242519132717086>"},
            {"antimori", "<:antimori:997236942993379388>"},
            {"doubt", "<:doubt:790853671586889751>"},
            {"ripbozo", "<:Ripbozo:962833922323013672>"},
            {"kekwwut", "<a:kekwwut:967597323683254292>"},
            {"kekwthink", "<:kekwthink:1054837412901167175>"},
            {"dread", "<:dread:1055090498525732993>"},
            {"plsdont", "<:plsdont:928099176460455936>"},
            {"peepowtf", "<:peepowtf:1058049116732661810>"}
        };

        public static Random randomRepliesToCompliments = new Random();
        public static string[] response =
        {
                "Wh-Wha?!",
                "Wh-What are you saying?",
                "?!!",
                "Don't say those things out loud!",
                "Hmph",
                "Sheesh!",
                "Can you not?",
                "Hehhh!?",
                "...",
                "G-Geez!"
            };
        public static Random randomRepliesToInsults = new Random();
        public static string[] comebacks =
        {
                "If your opinion mattered to me, I'd be offended",
                "I'd punch you, but that's animal abuse",
                "I could eat a bowl of alphabet soup and shit out a smarter statement than what you just said",
                "Is your ass jealous of the amount of crap that just came out of your mouth?",
                "Oh yeah? The jerk store called and they ran out of you."
        };
        public static List<string> compliments = new List<string> {
            {"you're cute"},
            {"you're so cute"},
            {"you are so cute" },
            {"you're a cutie" },
            {"short and cute" },
            {"i like you" },
            {"i love you" },
            {"marry me" },
            {"go on a date" },
            {"best girl" }
        };
    }
}