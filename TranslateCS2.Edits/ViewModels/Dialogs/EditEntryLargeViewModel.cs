using System;
using System.Windows;
using System.Windows.Media;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.Translators;
using TranslateCS2.Core.Translators.Collectors;
using TranslateCS2.Edits.Models;
using TranslateCS2.Edits.Properties.I18N;

namespace TranslateCS2.Edits.ViewModels.Dialogs;
internal class EditEntryLargeViewModel : BindableBase, IDialogAware {
    private readonly int _translationTextBoxHeightLineMultiplier = 3;
    private readonly int _translationTextBoxHeightLine = 20;
    private readonly int _translationTextBoxHeightMax = 360;

    private ButtonResult _buttonResult = ButtonResult.None;

    private string? _backUpTranslation;
    private ILocalizationDictionaryEntry? _Entry;
    public ILocalizationDictionaryEntry? Entry {
        get => this._Entry;
        set => this.SetProperty(ref this._Entry, value, this.OnEntryChanged);
    }


    private bool _IsCount;
    public bool IsCount {
        get => this._IsCount;
        set => this.SetProperty(ref this._IsCount, value);
    }


    private Brush _ActionTextColor = Brushes.DarkGreen;
    public Brush ActionTextColor {
        get => this._ActionTextColor;
        set => this.SetProperty(ref this._ActionTextColor, value);
    }


    private string? _ActionText;
    public string? ActionText {
        get => this._ActionText;
        set => this.SetProperty(ref this._ActionText, value);
    }


    private bool _IsEnabled = true;
    public bool IsEnabled {
        get => this._IsEnabled;
        set => this.SetProperty(ref this._IsEnabled, value);
    }


    private int _TranslationTextBoxHeight;
    public int TranslationTextBoxHeight {
        get => this._TranslationTextBoxHeight;
        set => this.SetProperty(ref this._TranslationTextBoxHeight, value);
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
        this.IsEnabled = !this.IsEnabled;
        this.ActionText = I18NEdits.MessageTranslating;
        this.ActionTextColor = Brushes.DarkGreen;
        TranslatorResult? result = null;
        switch (valueToUse) {
            case ValueToUse.ValueEnglish:
                result = await this.Translators.TranslateAsync(this.Entry.ValueLanguageCode, this.Entry?.Value);
                break;
            case ValueToUse.ValueMerge:
                result = await this.Translators.TranslateAsync(this.Entry.ValueMergeLanguageCode, this.Entry?.ValueMerge);
                break;
            case ValueToUse.ValueTranslation:
                // dont translate a translation :D
                break;
        }
        this.IsEnabled = !this.IsEnabled;
        if (result == null) {
            this.ActionText = I18NEdits.MessageSomethingWentWrong;
            this.ActionTextColor = Brushes.DarkRed;
            return;
        }
        if (result.IsError) {
            this.ActionText = result.Error;
            this.ActionTextColor = Brushes.DarkRed;
            return;
        }
        this.Entry.Translation = result.Translation;
        this.ActionText = null;
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
            return;
        }
        this.Entry = entry;
        bool isCount;
        bool gotIsCount = parameters.TryGetValue<bool>(nameof(isCount), out isCount);
        this.IsCount = isCount;
    }

    private void OnEntryChanged() {
        int lines = 1;
        if (this.Entry != null) {
            this._backUpTranslation = this.Entry.Translation;
            lines = this.Entry.Value.Split("\n").Length;
        }
        int height = this._translationTextBoxHeightLine * this._translationTextBoxHeightLineMultiplier * lines;
        if (height > this._translationTextBoxHeightMax) {
            height = this._translationTextBoxHeightMax;
        }
        this.TranslationTextBoxHeight = height;
    }
}
