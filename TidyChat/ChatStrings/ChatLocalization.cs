﻿using Dalamud;
using Dalamud.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TidyChat
{
    internal class Localization
    {
        public static ClientLanguage language { get; set; }

        public static string[] Get(string[] strings)
        {
            // PluginLog.LogDebug("Strings not localized: %s", string.Join(",", strings));
            return strings;
        }

        public static string[] Get(LocalizedStrings strings)
        {
            return language switch
            {
                ClientLanguage.English => strings.en,
                ClientLanguage.Japanese => strings.ja,
                _ => strings.en // probably won't work but at least it's not a crash
            };
        }

        public static Regex Get(Regex regex)
        {
            // PluginLog.LogDebug("Regex not localized: %s", regex.ToString());
            return regex;
        }

        public static Regex Get(LocalizedRegex regex)
        {
            return language switch
            {
                ClientLanguage.English => regex.en,
                ClientLanguage.Japanese => regex.ja,
                _ => regex.en
            };
        }
    }

    internal struct LocalizedStrings
    {
        public string[] en = Array.Empty<string>();
        public string[] ja = Array.Empty<string>();
    }

    internal struct LocalizedRegex
    {
        public Regex en = new("");
        public Regex ja = new("");
    }
}
