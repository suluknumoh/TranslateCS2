using System.Globalization;
using System.Text;

using TranslateCS2.Core.Helpers;
using TranslateCS2.ModBridge;


namespace TranslateCS2.ZModDevHelper;

public class HelpMeWithSupportedLanguages {
    private readonly Dictionary<UnitySystemLanguage, List<CultureInfo>> Dict = [];
    [Fact(Skip = "dont help me each time")]
    public void GenerateJsons() {
        string directoryPath = PathHelper.TryToGetModsPath();
        IEnumerable<string> supportedLocales = LocalesSupported.Lowered;
        foreach (string supportedLocale in supportedLocales) {
            string content = $"{{ \"Options.SECTION[General]\": \"General ({supportedLocale}.json)\" }}";
            string filePath = Path.Combine(directoryPath, $"{supportedLocale}{ModConstants.JsonExtension}");
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }
    }

    /// <summary>
    ///     generates the list of supported language/region-codes for the long description and readme markdowns
    /// </summary>
    [Fact(Skip = "dont help me each time")]
    public void GenerateMarkdownList() {
        this.FillDictionary();
        IOrderedEnumerable<KeyValuePair<UnitySystemLanguage, List<CultureInfo>>> ordered = this.Dict.OrderBy(item => item.Key.ToString());
        StringBuilder builder = new StringBuilder();
        foreach (KeyValuePair<UnitySystemLanguage, List<CultureInfo>> entry in ordered) {
            builder.AppendLine($"## {entry.Key.ToString()}");
            foreach (CultureInfo? cultureInfo in entry.Value) {
                builder.AppendLine($"* {cultureInfo.Name} - {CultureInfoHelper.EaseLocaleName(cultureInfo)}");
            }
            builder.AppendLine();
            builder.AppendLine();
        }
        string supportedLanguagesMarkDown = builder.ToString().Replace("&", "and");
        Assert.True(true);

    }

    private void FillDictionary() {
        IEnumerable<UnitySystemLanguage> languages = Enum.GetValues(typeof(UnitySystemLanguage)).OfType<UnitySystemLanguage>();
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Where(culture => LocalesSupported.IsLocaleIdSupported(culture.Name)).ToArray();
        foreach (CultureInfo culture in cultures) {
            foreach (UnitySystemLanguage language in languages) {
                string? comparator = null;
                switch (language) {
                    case UnitySystemLanguage.Chinese:
                        continue;
                    case UnitySystemLanguage.SerboCroatian:
                        if (culture.EnglishName.StartsWith(LangConstants.Serbian, StringComparison.OrdinalIgnoreCase)
                            || culture.EnglishName.StartsWith(LangConstants.Croatian, StringComparison.OrdinalIgnoreCase)
                        ) {
                            this.AddToDictionary(culture, language);
                        }
                        continue;
                    case UnitySystemLanguage.ChineseSimplified:
                        comparator = LangConstants.ChineseSimplified;
                        break;
                    case UnitySystemLanguage.ChineseTraditional:
                        comparator = LangConstants.ChineseTraditional;
                        break;
                    default:
                        comparator = language.ToString();
                        break;
                }
                if (culture.EnglishName.StartsWith(comparator, StringComparison.OrdinalIgnoreCase)) {
                    this.AddToDictionary(culture, language);
                }
            }
        }
    }

    private void AddToDictionary(CultureInfo culture, UnitySystemLanguage language) {
        if (this.Dict.TryGetValue(language, out List<CultureInfo>? cultureInfos) && cultureInfos != null) {
            cultureInfos.Add(culture);
        } else {
            cultureInfos = [culture];
            this.Dict.Add(language, cultureInfos);
        }
    }
}