﻿using TidyStrings = TidyChat.Utility.InternalStrings;
using Dalamud.Game.Gui;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud;
using System.Linq;

namespace TidyChat.Utility
{
    internal static class BetterStrings
    {
        public static void Commendations(Configuration configuration, ChatGui chatGui)
        {
            TidyStrings.NumberOfCommendations++;

            // Give it a few seconds before sending the /debug message with the total number of commendations in case there is any lag between commendation messages
            // There shouldn't be any lag since I think they all get sent at once - but having this small wait guarantees that there won't be any problems
            if (TidyStrings.NumberOfCommendations == 1)
            {

                var t = new System.Timers.Timer
                {
                    Interval = 2500,
                    AutoReset = false
                };
                t.Elapsed += delegate
                {
                    var stringBuilder = new SeStringBuilder();
                    if (configuration.IncludeChatTag)
                    {
                        stringBuilder.AddUiForeground(14);
                        stringBuilder.AddText(TidyStrings.Tag);
                        stringBuilder.AddUiForegroundOff();
                    }
                    string commendations = $"commendation{(TidyStrings.NumberOfCommendations == 1 ? "" : "s")}";

                    string dutyName = $"{(configuration.IncludeDutyNameInComms && TidyStrings.LastDuty.Length > 0 ? " from completing " + TidyStrings.LastDuty + "." : ".")}";

                    stringBuilder.AddText($"You received {TidyStrings.NumberOfCommendations} {commendations}{dutyName}");

                    chatGui.Print(stringBuilder.BuiltString);
                    t.Enabled = false;
                    t.Dispose();
                    TidyStrings.NumberOfCommendations = 0;
                    TidyStrings.LastDuty = "";
                };
                t.Enabled = true;
            }
        }

        public static SeString SayReminder(SeString message, Configuration configuration)
        {
            // With the chat mode in Say, enter a phrase containing "Capture this"

            int containingPhraseStart = message.TextValue.IndexOf('“');
            int containingPhraseEnd = message.TextValue.LastIndexOf('”');
            int lengthOfPhrase = containingPhraseEnd - containingPhraseStart;
            string containingPhrase = message.TextValue.Substring(containingPhraseStart + 1, lengthOfPhrase - 1);
            if (configuration.CopyBetterSayReminder)
            {
                var stringBuilder = new SeStringBuilder();
                if (configuration.IncludeChatTag)
                {
                    stringBuilder.AddUiForeground(14);
                    stringBuilder.AddText(TidyStrings.Tag);
                    stringBuilder.AddUiForegroundOff();
                }
                stringBuilder.AddText($"\"/say {containingPhrase}\" {Localization.GetTidy(TidyStrings.CopiedToClipboard)}");
                TextCopy.ClipboardService.SetText($"/say {containingPhrase}");
                return stringBuilder.BuiltString;
            }
            return $"/say {containingPhrase}";
        }

        public static SeString Instances(SeString message, Configuration configuration)
        {
            // The last character in the first sentence is the instanceNumber so
            // we capture it by finding the period that ends the first sentence and going back one character
            int index = Localization.Language switch
            {
                ClientLanguage.Japanese => message.TextValue.IndexOf("」"),
                ClientLanguage.English => message.TextValue.IndexOf("."),
                ClientLanguage.German => message.TextValue.IndexOf("“"),
                ClientLanguage.French => message.TextValue.IndexOf("”"),
                _ => message.TextValue.IndexOf("."),
            };
            string instanceNumber = message.TextValue.Substring(index - 1, 1);
            var stringBuilder = new SeStringBuilder();
            if (configuration.IncludeChatTag)
            {
                stringBuilder.AddUiForeground(14);
                stringBuilder.AddText(TidyStrings.Tag);
                stringBuilder.AddUiForegroundOff();
            }
            stringBuilder.AddText($"{Localization.GetTidy(TidyStrings.InstanceText)} {instanceNumber}");
            return stringBuilder.BuiltString;
        }

        /// <see href="https://xivapi.com/LogMessage/7027?pretty=true">You've joined the Novice Network</see>
        /// <see href="https://xivapi.com/LogMessage/7030?pretty=true">You have left the Novice Network</see>
        public static SeString NoviceNetwork(SeString originalMessage, string normalizedInput, Configuration configuration)
        {
            if (Localization.Get(ChatStrings.NoviceNetworkJoin).All(normalizedInput.Contains))
            {
                SeString newMessage = Localization.Language switch
                {
                    ClientLanguage.Japanese => "ビギナーチャンネルに参加しました。",
                    ClientLanguage.English => "You've joined the Novice Network.",
                    ClientLanguage.German => "Du bist dem Neulings-Chat beigetreten.",
                    ClientLanguage.French => "Vous avez rejoint le réseau des novices.",
                    _ => "You've joined the Novice Network.",
                };
                var stringBuilder = new SeStringBuilder();
                if (configuration.IncludeChatTag)
                {
                    stringBuilder.AddUiForeground(14);
                    stringBuilder.AddText(TidyStrings.Tag);
                    stringBuilder.AddUiForegroundOff();
                }
                stringBuilder.AddText($"{newMessage}");
                return stringBuilder.BuiltString;
            } else if (Localization.Get(ChatStrings.NoviceNetworkLeft).All(normalizedInput.Contains))
            {
                SeString newMessage = Localization.Language switch
                {
                    ClientLanguage.Japanese => "ビギナーチャンネルから退出しました。",
                    ClientLanguage.English => "You've left the Novice Network.",
                    ClientLanguage.German => "Du hast den Neulings-Chat verlassen.",
                    ClientLanguage.French => "Vous avez quitté le réseau des novices.",
                    _ => "You've left the Novice Network.",
                };
                var stringBuilder = new SeStringBuilder();
                if (configuration.IncludeChatTag)
                {
                    stringBuilder.AddUiForeground(14);
                    stringBuilder.AddText(TidyStrings.Tag);
                    stringBuilder.AddUiForegroundOff();
                }
                stringBuilder.AddText($"{newMessage}");
                return stringBuilder.BuiltString;
            } else
            {
                return originalMessage;
            }
        }
    }
}
