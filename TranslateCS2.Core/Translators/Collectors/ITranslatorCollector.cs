using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TranslateCS2.Core.Translators.Collectors;
/// <summary>
///     collects <see cref="ITranslator"/>s from <see cref="Translators.Modules.ITranslatorModule"/>s
/// </summary>
public interface ITranslatorCollector {
    /// <summary>
    ///     don't add to this <see cref="ObservableCollection{T}"/>!!!
    ///     <br/>
    ///     <br/>
    ///     use <see cref="AddTranslator(ITranslator)"/> instead!!!
    /// </summary>
    ObservableCollection<ITranslator> Translators { get; }
    /// <summary>
    ///     return <see langword="true"/> if <see cref="Translators"/> has elements
    /// </summary>
    bool AreTranslatorsAvailable { get; }
    /// <summary>
    ///     the <see cref="ITranslator"/> to use
    /// </summary>
    ITranslator? SelectedTranslator { get; set; }
    /// <summary>
    ///     <see langword="true"/> if <see cref="SelectedTranslator"/> <see langword="is"/> <see langword="not"/> <see langword="null"/>
    /// </summary>
    bool IsTranslatorSelected { get; }
    /// <summary>
    ///     calls <see cref="ITranslator.Init(HttpClients.AppHttpClient)"/> and adds the <see cref="ITranslator"/> to <see cref="Translators"/>
    /// </summary>
    /// <param name="translator"></param>
    void AddTranslator(ITranslator translator);
    /// <summary>
    ///     calls <see cref="SelectedTranslator"/>s <see cref="ITranslator.TranslateAsync(HttpClients.AppHttpClient, System.String, System.String?)"/>
    /// </summary>
    /// <param name="sourceLanguageCode">
    ///     language-code that corresponds to one of the .loc-files
    ///     <br/>
    ///     <br/>
    ///     e.g.:
    ///     <list type="bullet">
    ///         <item>de-DE</item>
    ///         <item>en-US</item>
    ///         <item>es-ES</item>
    ///         <item>fr-FR</item>
    ///         <item>it-IT</item>
    ///         <item>ja-JP</item>
    ///         <item>ko-KR</item>
    ///         <item>pl-PL</item>
    ///         <item>pt-BR</item>
    ///         <item>ru-RU</item>
    ///         <item>zh-HANS</item>
    ///         <item>zh-HANT</item>
    ///     </list>
    /// </param>
    /// <param name="s">
    ///     the value as is to translate
    /// </param>
    /// <returns>
    ///     <see cref="TranslatorResult"/>
    /// </returns>
    Task<TranslatorResult> TranslateAsync(string sourceLanguageCode, string? s);
}
