using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using Prism.Mvvm;

namespace TranslateCS2.Core.Translators;
public abstract class ATranslator : BindableBase, ITranslator {
    /// <inheritdoc cref="ITranslator.Name"/>
    public string Name { get; }
    /// <inheritdoc cref="ITranslator.Description"/>
    public string Description { get; }
    /// <inheritdoc cref="ITranslator.TargetLanguageCodes"/>
    public ObservableCollection<object> TargetLanguageCodes { get; } = [];


    private object? _SelectedTargetLanguageCode;
    /// <inheritdoc cref="ITranslator.SelectedTargetLanguageCode"/>
    public object? SelectedTargetLanguageCode {
        get => this._SelectedTargetLanguageCode;
        set => this.SetProperty(ref this._SelectedTargetLanguageCode, value, () => this.IsTargetLanguageCodeSelected = this.SelectedTargetLanguageCode is not null);
    }


    private bool _IsLanguageCodeSelected;
    /// <inheritdoc cref="ITranslator.IsTargetLanguageCodeSelected"/>
    public bool IsTargetLanguageCodeSelected {
        get => this._IsLanguageCodeSelected;
        set => this.SetProperty(ref this._IsLanguageCodeSelected, value);
    }


    public ATranslator(string name, string description) {
        this.Name = name;
        this.Description = description;
    }
    /// <inheritdoc cref="ITranslator.InitAsync(HttpClient)"/>
    public abstract Task InitAsync(HttpClient httpClient);
    /// <inheritdoc cref="ITranslator.TranslateAsync(HttpClient, System.String, System.String?)"/>
    public abstract Task<TranslatorResult> TranslateAsync(HttpClient httpClient, string sourceLanguageCode, string? s);
}
