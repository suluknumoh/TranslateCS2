namespace TranslateCS2.ZZZModTestLib;
/// <summary>
///     <see langword="const"/>s/<see langword="value"/>s
///     <br/>
///     that should not be calculated
///     <br/>
///     if something is changed by colossal order,
///     <br/>
///     the <see cref="UnityEngine"/> or the net-framework-version, for example,
///     <br/>
///     and the new <see cref="UnityEngine"/> has more <see cref="UnityEngine.SystemLanguage"/>s
///     <br/>
///     it can be recognized
/// </summary>
public static class ModTestConstants {
    /// <summary>
    ///     for now <see cref="UnityEngine.SystemLanguage"/> has 44 <see langword="enum"/>-<see langword="value"/>s
    ///     <br/>
    ///     <see cref="UnityEngine.SystemLanguage.Chinese"/> is 'dismissed'
    ///     <br/>
    ///     cause the more specific <see cref="UnityEngine.SystemLanguage"/>s
    ///     <br/>
    ///     <see cref="UnityEngine.SystemLanguage.ChineseSimplified"/>
    ///     <br/>
    ///     and
    ///     <br/>
    ///     <see cref="UnityEngine.SystemLanguage.ChineseTraditional"/>
    ///     <br/>
    ///     are present
    /// </summary>
    public static int ExpectedLanguageCount { get; } = 43;

    public static int ExpectedFlavorCount { get; } = 303;
}
