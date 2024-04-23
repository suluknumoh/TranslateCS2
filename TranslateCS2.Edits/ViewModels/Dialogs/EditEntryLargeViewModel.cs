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
    private bool _canCloseDialog = false;


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
    public DelegateCommand DeleteCommand { get; }


    public ITranslatorCollector Translators { get; }


    public EditEntryLargeViewModel(ITranslatorCollector translatorCollector) {
        this.Translators = translatorCollector;
        this.CopyCommand = new DelegateCommand<ValueToUse?>(this.CopyCommandAction);
        this.TranslateCommand = new DelegateCommand<ValueToUse?>(this.TranslateCommandAction);
        this.SaveCommand = new DelegateCommand(this.SaveCommandAction);
        this.CancelCommand = new DelegateCommand(this.CancelCommandAction);
        this.DeleteCommand = new DelegateCommand(this.DeleteCommandAction);
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
        this._canCloseDialog = true;
        IDialogResult result = new DialogResult(ButtonResult.OK);
        result.Parameters.Add(nameof(ILocalizationDictionaryEntry), this.Entry);
        RequestClose?.Invoke(result);
        this.Entry = null;
        this._backUpTranslation = null;
    }

    private void DeleteCommandAction() {
        if (this.Entry == null) {
            return;
        }
        if (!this.Entry.IsDeleteAble) {
            return;
        }
        if (IsActionInterrupted(I18NEdits.DialogDeleteCaption,
                                I18NEdits.DialogDeleteText)) {
            return;
        }
        this._canCloseDialog = true;
        this.Entry.Translation = null;
        IDialogResult result = new DialogResult(ButtonResult.Yes);
        result.Parameters.Add(nameof(ILocalizationDictionaryEntry), this.Entry);
        RequestClose?.Invoke(result);
        this.Entry = null;
        this._backUpTranslation = null;
    }

    private void CancelCommandAction() {
        if (this.Entry == null) {
            return;
        }
        if (this.IsCancelInterruptable()) {
            if (IsActionInterrupted(I18NEdits.DialogCancelCaption,
                                    I18NEdits.DialogCancelText)) {
                return;
            }
        }
        this._canCloseDialog = true;
        this.Entry.Translation = this._backUpTranslation;
        IDialogResult result = new DialogResult(ButtonResult.Cancel);
        result.Parameters.Add(nameof(ILocalizationDictionaryEntry), this.Entry);
        RequestClose?.Invoke(result);
        this.Entry = null;
        this._backUpTranslation = null;
    }

    private static bool IsActionInterrupted(string caption, string text) {
        MessageBoxResult messageBoxResult = MessageBox.Show(text,
                                                            caption,
                                                            MessageBoxButton.YesNo,
                                                            MessageBoxImage.Warning,
                                                            MessageBoxResult.None);
        // yes = cancel anyway
        // everything else interrupts canceling
        return MessageBoxResult.Yes != messageBoxResult;
    }

    private bool IsCancelInterruptable() {
        return !Equals(this.Entry?.Translation, this._backUpTranslation);
    }

    public bool CanCloseDialog() {
        return this._canCloseDialog;
    }

    public void OnDialogClosed() {
        // x in the top right corner is clicked
        // nothing can and should be done
    }

    public void OnDialogOpened(IDialogParameters parameters) {
        this._canCloseDialog = false;
        bool gotEntry = parameters.TryGetValue<ILocalizationDictionaryEntry>(nameof(ILocalizationDictionaryEntry), out ILocalizationDictionaryEntry entry);
        if (!gotEntry) {
            this.Entry = null;
            return;
        }
        this.Entry = entry;
        //
        bool gotIsCount = parameters.TryGetValue<bool>(nameof(EditEntryLargeViewModel.IsCount), out bool isCount);
        this.IsCount = gotIsCount && isCount;
    }

    private void OnEntryChanged() {
        int lines = 3;
        if (this.Entry != null) {
            this._backUpTranslation = this.Entry.Translation;
            if (this.Entry.Value != null) {
                int valueLines = this.Entry.Value.Split("\n").Length;
                if (valueLines > lines) {
                    lines = valueLines;
                }
            }
        }
        int height = this._translationTextBoxHeightLine * this._translationTextBoxHeightLineMultiplier * lines;
        if (height > this._translationTextBoxHeightMax) {
            height = this._translationTextBoxHeightMax;
        }
        this.TranslationTextBoxHeight = height;
    }
}
