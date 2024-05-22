using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Inf;
[MyExcludeClassFromCoverage]
public static class LangConstants {
    public static string ChineseSimplified { get; } = "Chinese (Simplified";
    public static string ChineseTraditional { get; } = "Chinese (Traditional";
    public static string Croatian { get; } = nameof(Croatian);
    public static string Serbian { get; } = nameof(Serbian);
    public static string Latin { get; } = nameof(Latin);
    public static string Cyrillic { get; } = nameof(Cyrillic);
    public static string OtherLanguages { get; } = "other languages";
    public static string OtherLanguagesSelect { get; } = "Select a language.";
}
