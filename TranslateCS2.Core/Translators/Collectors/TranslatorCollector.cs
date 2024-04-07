using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Prism.Mvvm;

using TranslateCS2.Core.HttpClients;

namespace TranslateCS2.Core.Translators.Collectors;
/// <inheritdoc cref="ITranslatorCollector"/>
internal class TranslatorCollector : BindableBase, ITranslatorCollector {
    private readonly IHttpClient _httpClient;
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


    public TranslatorCollector(IHttpClient httpClient) {
        this._httpClient = httpClient;
        this.Translators.CollectionChanged += this.TranslatorsChangedAction;
    }

    /// <inheritdoc/>
    public string? Translate(string sourceLanguageCode, string? s) {
        if (!this.AreTranslatorsAvailable) {
            return s;
        }
        if (this.SelectedTranslator is null) {
            return s;
        }
        if (this.SelectedTranslator.SelectedTargetLanguageCode is null) {
            return s;
        }
        return this.SelectedTranslator.Translate(this._httpClient, sourceLanguageCode, s);
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
