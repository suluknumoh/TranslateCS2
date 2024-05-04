using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using TranslateCS2.Inf;

using UnityEngine;


namespace TranslateCS2.ZModDevHelper;

public class HelpMeWithSupportedLanguages {
    private readonly ModTestRuntimeContainer runtimeContainer;
    public HelpMeWithSupportedLanguages() {
        this.runtimeContainer = new ModTestRuntimeContainer();
    }
    [Fact(Skip = "dont help me each time")]
    public void GenerateJsons() {
        string directoryPath = PathHelper.TryToGetModsPath();
        IDictionary<SystemLanguage, IList<CultureInfo>> mapping = this.runtimeContainer.LocaleHelper.GetSystemLanguageCulturesMapping(true);
        foreach (KeyValuePair<SystemLanguage, IList<CultureInfo>> entry in mapping) {
            foreach (CultureInfo supportedLocale in entry.Value) {
                string content = $"{{ \"Options.SECTION[General]\": \"General ({supportedLocale}.json)\" }}";
                string filePath = Path.Combine(directoryPath, $"{supportedLocale}{ModConstants.JsonExtension}");
                if (File.Exists(filePath)) {
                    File.Delete(filePath);
                }
                File.WriteAllText(filePath, content, Encoding.UTF8);
            }
        }
    }

    /// <summary>
    ///     generates the list of supported language/region-codes for the long description and readme markdowns
    /// </summary>
    [Fact(Skip = "dont help me each time")]
    public void GenerateMarkdownList() {
        IDictionary<SystemLanguage, IList<CultureInfo>> mapping = this.runtimeContainer.LocaleHelper.GetSystemLanguageCulturesMapping(true);
        IOrderedEnumerable<KeyValuePair<SystemLanguage, IList<CultureInfo>>> ordered = mapping.OrderBy(item => item.Key.ToString());
        StringBuilder builder = new StringBuilder();
        foreach (KeyValuePair<SystemLanguage, IList<CultureInfo>> entry in ordered) {
            builder.AppendLine($"## {entry.Key.ToString()}");
            foreach (CultureInfo? cultureInfo in entry.Value) {
                builder.AppendLine($"* {cultureInfo.Name} - {cultureInfo.EnglishName}");
            }
            builder.AppendLine();
            builder.AppendLine();
        }
        string supportedLanguagesMarkDown = builder.ToString().Replace("&", "and")
            //.ReplaceLineEndings("\n")
            ;
        Assert.True(true);

    }

    [Fact(Skip = "dont help me each time")]
    public void MappAbleLocalesSupported() {
        List<CultureInfo> cis = [];
        IDictionary<SystemLanguage, IList<CultureInfo>> mapping = this.runtimeContainer.LocaleHelper.GetSystemLanguageCulturesMapping(true);
        foreach (KeyValuePair<SystemLanguage, IList<CultureInfo>> entry in mapping) {
            cis.AddRange(entry.Value);
        }
        string text = "\"";
        text += String.Join("\",\n\"", cis);
        text += "\"";
        Assert.NotNull(text);
    }

}
