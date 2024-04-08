using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace TranslateCS2.Core.Translators;
public interface ITranslator {
    /// <summary>
    ///     the name
    /// </summary>
    string Name { get; }
    /// <summary>
    ///     a description
    /// </summary>
    string Description { get; }
    ObservableCollection<string> TargetLanguageCodes { get; }
    /// <summary>
    ///     the selected target-language-code
    /// </summary>
    string? SelectedTargetLanguageCode { get; set; }
    /// <summary>
    ///     <see langword="true"/>
    ///     <br/>
    ///     if <see cref="SelectedTargetLanguageCode"/>
    ///     <br/>
    ///     <see langword="is"/> <see langword="not"/> <see langword="null"/>,
    ///     <br/>
    ///     and
    ///     <br/>
    ///     <see langword="not"/> <see cref="System.String.Empty"/>
    ///     <br/>
    ///     and
    ///     <br/>
    ///     <see langword="not"/> whitespaces
    /// </summary>
    bool IsTargetLanguageCodeSelected { get; set; }
    /// <summary>
    ///     this method is called by the app itself!!!
    ///     <br/>
    ///     no need for you to call it!!!
    ///     <br/>
    ///     <br/>
    ///     to init the Translator
    ///     <br/>
    ///     <br/>
    ///     e.g. the available target-language-codes
    /// </summary>
    /// <param name="httpClient">
    ///     an <see cref="IHttpClient"/> that can not be disposed
    ///     <br/>
    ///     <br/>
    ///     <seealso href="https://learn.microsoft.com/de-de/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use"/>
    /// </param>
    void Init(HttpClient httpClient);
    /// <summary>
    ///     to translate <paramref name="s"/> from <paramref name="sourceLanguageCode"/> to <see cref="SelectedTargetLanguageCode"/> via <paramref name="httpClient"/>
    /// </summary>
    /// <param name="httpClient">
    ///     an <see cref="IHttpClient"/> that can not be disposed
    ///     <br/>
    ///     <br/>
    ///     <seealso href="https://learn.microsoft.com/de-de/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use"/>
    /// </param>
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
    Task<TranslatorResult> TranslateAsync(HttpClient httpClient, string sourceLanguageCode, string? s);
}
