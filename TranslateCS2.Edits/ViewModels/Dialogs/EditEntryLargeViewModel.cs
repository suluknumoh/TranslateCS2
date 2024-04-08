using System;
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.Translators.Collectors;
using TranslateCS2.Edits.Models;
using TranslateCS2.Edits.Properties.I18N;

namespace TranslateCS2.Edits.ViewModels.Dialogs;
internal class EditEntryLargeViewModel : BindableBase, IDialogAware {

    private ButtonResult _buttonResult = ButtonResult.None;

    private string? _backUpTranslation;
    private ILocalizationDictionaryEntry? _Entry;
    public ILocalizationDictionaryEntry? Entry {
        get => this._Entry;
        set => this.SetProperty(ref this._Entry, value);
    }


    private bool _IsCount;
    public bool IsCount {
        get => this._IsCount;
        set => this.SetProperty(ref this._IsCount, value);
    }


    public string Title => I18NEdits.DialogTitle;

    public event Action<IDialogResult>? RequestClose;

    public DelegateCommand<ValueToUse?> CopyCommand { get; }
    public DelegateCommand<ValueToUse?> TranslateCommand { get; }
    public DelegateCommand SaveCommand { get; }
    public DelegateCommand CancelCommand { get; }


    public ITranslatorCollector Translators { get; }


    public EditEntryLargeViewModel(ITranslatorCollector translatorCollector) {
        this.Translators = translatorCollector;
        this.CopyCommand = new DelegateCommand<ValueToUse?>(this.CopyCommandAction);
        this.TranslateCommand = new DelegateCommand<ValueToUse?>(this.TranslateCommandAction);
        this.SaveCommand = new DelegateCommand(this.SaveCommandAction);
        this.CancelCommand = new DelegateCommand(this.CancelCommandAction);
    }

    private void CopyCommandAction(ValueToUse? valueToUse) {
        if (valueToUse is null) {
            return;
        }
        string? copy = null;
        switch (valueToUse) {
            case ValueToUse.ValueEnglish:
                copy = this.Entry?.Value;
                break;
            case ValueToUse.ValueMerge:
                copy = this.Entry?.ValueMerge;
                break;
            case ValueToUse.ValueTranslation:
                copy = this.Entry?.Translation;
                break;
        }
        Clipboard.SetText(copy);
    }

    private async void TranslateCommandAction(ValueToUse? valueToUse) {
        if (valueToUse is null || this.Entry is null) {
            return;
        }
        switch (valueToUse) {
            case ValueToUse.ValueEnglish:
                this.Entry.Translation = await this.Translators.TranslateAsync(this.Entry.ValueLanguageCode, this.Entry?.Value);
                break;
            case ValueToUse.ValueMerge:
                this.Entry.Translation = await this.Translators.TranslateAsync(this.Entry.ValueMergeLanguageCode, this.Entry?.ValueMerge);
                break;
            case ValueToUse.ValueTranslation:
                // dont translate a translation :D
                break;
        }
    }

    private void SaveCommandAction() {
        this._buttonResult = ButtonResult.OK;
        IDialogResult result = new DialogResult(this._buttonResult);
        result.Parameters.Add(nameof(ILocalizationDictionaryEntry), this.Entry);
        RequestClose?.Invoke(result);
        this.Entry = null;
        this._backUpTranslation = null;
    }

    private void CancelCommandAction() {
        this._buttonResult = ButtonResult.Cancel;
        if (this.Entry != null) {
            this.Entry.Translation = this._backUpTranslation;
        }
        IDialogResult result = new DialogResult(this._buttonResult);
        result.Parameters.Add(nameof(ILocalizationDictionaryEntry), this.Entry);
        RequestClose?.Invoke(result);
        this.Entry = null;
        this._backUpTranslation = null;
    }

    public bool CanCloseDialog() {
        return true;
    }

    public void OnDialogClosed() {
        if (ButtonResult.None == this._buttonResult) {
            // x in the top right corner is clicked
            this.CancelCommandAction();
        }
    }

    public void OnDialogOpened(IDialogParameters parameters) {
        this._buttonResult = ButtonResult.None;
        bool gotEntry = parameters.TryGetValue<ILocalizationDictionaryEntry>(nameof(ILocalizationDictionaryEntry), out ILocalizationDictionaryEntry entry);
        if (!gotEntry) {
            this.Entry = null;
            this._backUpTranslation = null;
            return;
        }
        this._backUpTranslation = entry.Translation;
        this.Entry = entry;
        bool isCount;
        bool gotIsCount = parameters.TryGetValue<bool>(nameof(isCount), out isCount);
        this.IsCount = isCount;
    }
}
