using System;
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
    private TargetLanguage[] TargetLanguages { get; set; } = [];
    public TranslatorDeepL() : base("DeepL", "DeepL-API") { }
    public override async void Init(HttpClient httpClient) {
        if (this.authKey is null) {
            return;
        }
        TranslatorOptions options = new TranslatorOptions {
            ClientFactory = () => new HttpClientAndDisposeFlag() { HttpClient = httpClient, DisposeClient = false }
        };
        this.Translator = new Translator(this.authKey, options);
        this.SourceLanguages = await this.Translator.GetSourceLanguagesAsync();
        this.TargetLanguages = await this.Translator.GetTargetLanguagesAsync();
        foreach (TargetLanguage targetLanguage in this.TargetLanguages) {
            this.TargetLanguageCodes.Add(targetLanguage);
        }
    }

    public override async Task<TranslatorResult> TranslateAsync(HttpClient httpClient, string sourceLanguageCode, string? s) {
        if (this.authKey is null) {
            return new TranslatorResult() { Error = "authKey is null" };
        }
        if (this.Translator is null) {
            return new TranslatorResult() { Error = "DeepL-Translator is null" };
        }
        if (String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s)) {
            return new TranslatorResult() { Error = "nothing to translate; text to translate is empty" };
        }
        TargetLanguage? targetLanguage = this.GetTargetLanguageCode();
        if (targetLanguage is null) {
            return new TranslatorResult() { Error = "no target language selectede" };
        }
        // should not matter if source is null -> autodetection
        SourceLanguage? sourceLanguage = this.GetSourceLanguage(sourceLanguageCode);
        TextTranslateOptions textTranslateOptions = new TextTranslateOptions();
        TextResult textResult = await this.Translator.TranslateTextAsync(s, sourceLanguage?.Code, targetLanguage.Code, textTranslateOptions);
        return new TranslatorResult() { Translation = textResult.Text };
    }

    private SourceLanguage? GetSourceLanguage(string sourceLanguageCode) {
        foreach (SourceLanguage sourceLanguage in this.SourceLanguages) {
            if (sourceLanguageCode.StartsWith(sourceLanguage.Code, StringComparison.OrdinalIgnoreCase)) {
                return sourceLanguage;
            }
        }
        return null;
    }

    private TargetLanguage? GetTargetLanguageCode() {
        foreach (TargetLanguage targetLanguage in this.TargetLanguages) {
            if (targetLanguage.ToString() == this.SelectedTargetLanguageCode) {
                return targetLanguage;
            }
        }
        return null;
    }
}
