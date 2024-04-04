using System;
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using TranslateCS2.Models.Edits;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Properties.I18N;

namespace TranslateCS2.ViewModels.Dialogs;
internal class EditEntryLargeViewModel : BindableBase, IDialogAware {

    private ButtonResult _buttonResult = ButtonResult.None;

    private string? _backUpTranslation;
    private LocalizationDictionaryEntry? _Entry;
    public LocalizationDictionaryEntry? Entry {
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

    public DelegateCommand<CopyToClipBoardCommandParameters?> CopyCommand { get; }
    public DelegateCommand SaveCommand { get; }
    public DelegateCommand CancelCommand { get; }

    public EditEntryLargeViewModel() {
        this.CopyCommand = new DelegateCommand<CopyToClipBoardCommandParameters?>(this.CopyCommandAction);
        this.SaveCommand = new DelegateCommand(this.SaveCommandAction);
        this.CancelCommand = new DelegateCommand(this.CancelCommandAction);
    }

    private void CopyCommandAction(CopyToClipBoardCommandParameters? copyToClipBoardCommandParameter) {
        if (copyToClipBoardCommandParameter is null) {
            return;
        }
        string? copy = null;
        switch (copyToClipBoardCommandParameter) {
            case CopyToClipBoardCommandParameters.ValueEnglish:
                copy = this.Entry?.Value;
                break;
            case CopyToClipBoardCommandParameters.ValueMerge:
                copy = this.Entry?.ValueMerge;
                break;
            case CopyToClipBoardCommandParameters.ValueTranslation:
                copy = this.Entry?.Translation;
                break;
        }
        Clipboard.SetText(copy);
    }

    private void SaveCommandAction() {
        this._buttonResult = ButtonResult.OK;
        IDialogResult result = new DialogResult(this._buttonResult);
        result.Parameters.Add(nameof(LocalizationDictionaryEntry), this.Entry);
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
        result.Parameters.Add(nameof(LocalizationDictionaryEntry), this.Entry);
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
        bool gotEntry = parameters.TryGetValue<LocalizationDictionaryEntry>(nameof(LocalizationDictionaryEntry), out LocalizationDictionaryEntry entry);
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
