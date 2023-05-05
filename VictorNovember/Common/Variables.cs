﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VictorNovember.Common
{
    public class Variables
    {
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
        public static Dictionary<string, string> emotes = new Dictionary<string, string>
        {
            {"kekw", "<:kekw:986585139234631700>"},
            {"whatspin","<a:whatspin:1103004399569211453>"},
            {"kekwshook","<:kekwshook:965242519132717086>"},
            {"antimori", "<:antimori:997236942993379388>"},
            {"doubt", "<:doubt:790853671586889751>"},
            {"ripbozo", "<:Ripbozo:962833922323013672>"}
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
    }
}