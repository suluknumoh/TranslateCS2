using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using DeepL;
using DeepL.Model;

using TranslateCS2.Core.Translators;

namespace TranslateCS2.TranslatorsExample;
internal class TranslatorDeepL : ATranslator {
    private readonly string? authKey = null;
    private Translator? Translator { get; set; }
    private SourceLanguage[] SourceLanguages { get; set; } = [];
    public TranslatorDeepL() : base("DeepL", "DeepL-API") { }
    public override async Task InitAsync(HttpClient httpClient) {
        try {
            if (this.authKey is null) {
                return;
            }
            TranslatorOptions options = new TranslatorOptions {
                ClientFactory = () => new HttpClientAndDisposeFlag() { HttpClient = httpClient, DisposeClient = false }
            };
            this.Translator = new Translator(this.authKey, options);
            this.SourceLanguages = await this.Translator.GetSourceLanguagesAsync();
            TargetLanguage[] targetLanguages = await this.Translator.GetTargetLanguagesAsync();
            // this.TargetLanguageCodes takes objects
            // the objects added to this.TargetLanguageCodes have to provide an apropriate ToString-Method
            // it is used to generate the text that is displayed to select a target language within the view
            this.TargetLanguageCodes.AddRange(targetLanguages);
        } catch {
            //
        }
    }

    public override async Task<TranslatorResult> TranslateAsync(HttpClient httpClient, string sourceLanguageCode, string? s) {
        try {
            if (this.authKey is null) {
                return new TranslatorResult() { Error = "authKey is null" };
            }
            if (this.Translator is null) {
                return new TranslatorResult() { Error = "DeepL-Translator is null" };
            }
            if (String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s)) {
                return new TranslatorResult() { Error = "nothing to translate; text to translate is empty" };
            }
            if (this.SelectedTargetLanguageCode is not TargetLanguage targetLanguage) {
                return new TranslatorResult() { Error = "no target language selected" };
            }
            // should not matter if source is null -> autodetection
            SourceLanguage? sourceLanguage = this.GetSourceLanguage(sourceLanguageCode);
            TextTranslateOptions textTranslateOptions = new TextTranslateOptions();
            TextResult textResult = await this.Translator.TranslateTextAsync(s, sourceLanguage?.Code, targetLanguage.Code, textTranslateOptions);
            return new TranslatorResult() { Translation = textResult.Text };
        } catch {
            return new TranslatorResult() { Error = "an error message" };
        }
    }

    private SourceLanguage? GetSourceLanguage(string sourceLanguageCode) {
        foreach (SourceLanguage sourceLanguage in this.SourceLanguages) {
            if (sourceLanguageCode.StartsWith(sourceLanguage.Code, StringComparison.OrdinalIgnoreCase)) {
                return sourceLanguage;
            }
        }
        return null;
    }
}
