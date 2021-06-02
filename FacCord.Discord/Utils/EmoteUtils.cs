using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IsekaiTechnologies.FacCord.Discord.Utils
{
    public class EmoteUtils
    {
        public static IEmote GetEmote(string emoteString)
        {
            return Regex.IsMatch(emoteString, @"<\w*:\w+:\d+>") ? Emote.Parse(emoteString) : (IEmote) new Emoji(emoteString);
        }
    }
}
