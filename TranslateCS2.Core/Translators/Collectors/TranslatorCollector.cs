using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

using Prism.Mvvm;

using TranslateCS2.Core.Properties.I18N;

namespace TranslateCS2.Core.Translators.Collectors;
/// <inheritdoc cref="ITranslatorCollector"/>
internal class TranslatorCollector : BindableBase, ITranslatorCollector {
    private readonly HttpClient _httpClient;
    /// <inheritdoc/>
    public ObservableCollection<ITranslator> Translators { get; } = [];
    /// <inheritdoc/>
    public bool AreTranslatorsAvailable => this.Translators.Count > 0;


    private ITranslator? _SelectedTranslator;
    /// <inheritdoc/>
    public ITranslator? SelectedTranslator {
        get => this._SelectedTranslator;
        set => this.SetProperty(ref this._SelectedTranslator, value, () => this.IsTranslatorSelected = this.SelectedTranslator != null);
    }


    private bool _IsTranslatorSelected;
    /// <inheritdoc/>
    public bool IsTranslatorSelected {
        get => this._IsTranslatorSelected;
        set => this.SetProperty(ref this._IsTranslatorSelected, value);
    }


    public TranslatorCollector(HttpClient httpClient) {
        this._httpClient = httpClient;
        this.Translators.CollectionChanged += this.TranslatorsChangedAction;
    }

    /// <inheritdoc/>
    public async Task<TranslatorResult> TranslateAsync(string sourceLanguageCode, string? s) {
        if (!this.AreTranslatorsAvailable) {
            return new TranslatorResult() { Error = I18NGlobal.MessageNoTranslators };
        }
        if (this.SelectedTranslator is null) {
            return new TranslatorResult() { Error = I18NGlobal.MessageNoTranslatorSelected };
        }
        if (this.SelectedTranslator.SelectedTargetLanguageCode is null) {
            return new TranslatorResult() { Error = I18NGlobal.MessageNoTargetLanguageSelected };
        }
        return await this.SelectedTranslator.TranslateAsync(this._httpClient, sourceLanguageCode, s);
    }

    private void TranslatorsChangedAction(object? sender, NotifyCollectionChangedEventArgs e) {
        this.RaisePropertyChanged(nameof(this.AreTranslatorsAvailable));
    }

    /// <inheritdoc/>
    public void AddTranslator(ITranslator translator) {
        translator.Init(this._httpClient);
        this.Translators.Add(translator);
    }
}
