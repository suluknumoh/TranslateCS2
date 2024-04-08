using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using Prism.Mvvm;

using TranslateCS2.Core.Helpers;

namespace TranslateCS2.Core.Translators;
public abstract class ATranslator : BindableBase, ITranslator {
    /// <inheritdoc/>
    public string Name { get; }
    /// <inheritdoc/>
    public string Description { get; }
    /// <inheritdoc/>
    public ObservableCollection<string> TargetLanguageCodes { get; } = [];


    private string? _SelectedTargetLanguageCode;
    /// <inheritdoc/>
    public string? SelectedTargetLanguageCode {
        get => this._SelectedTargetLanguageCode;
        set => this.SetProperty(ref this._SelectedTargetLanguageCode, value, () => this.IsTargetLanguageCodeSelected = !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.SelectedTargetLanguageCode));
    }


    private bool _IsLanguageCodeSelected;
    /// <inheritdoc/>
    public bool IsTargetLanguageCodeSelected {
        get => this._IsLanguageCodeSelected;
        set => this.SetProperty(ref this._IsLanguageCodeSelected, value);
    }


    public ATranslator(string name, string description) {
        this.Name = name;
        this.Description = description;
    }
    /// <inheritdoc/>
    public abstract void Init(HttpClient httpClient);
    /// <inheritdoc/>
    public abstract Task<TranslatorResult> TranslateAsync(HttpClient httpClient, string sourceLanguageCode, string? s);
}
