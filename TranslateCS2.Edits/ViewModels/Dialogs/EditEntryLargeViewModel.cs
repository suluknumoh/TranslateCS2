using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.Translators;
using TranslateCS2.Core.Translators.Collectors;
using TranslateCS2.Edits.Models;
using TranslateCS2.Edits.Properties.I18N;

namespace TranslateCS2.Edits.ViewModels.Dialogs;
internal class EditEntryLargeViewModel : BindableBase, IDialogAware {
    private readonly int translationTextBoxHeightLineMultiplier = 3;
    private readonly int translationTextBoxHeightLine = 20;
    private readonly int translationTextBoxHeightMax = 360;
    private bool canCloseDialog = false;
    private bool isLoaded = false;
    private BindingGroup? bindingGroup;

    private ITranslationSessionManager SessionManager { get; }

    private IAppLocFileEntry? original;
    private EditEntry? _Entry;
    public EditEntry? Entry {
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
    public DelegateCommand<RoutedEventArgs> OnLoaded { get; }

    public ITranslatorCollector Translators { get; }


    public EditEntryLargeViewModel(ITranslatorCollector translatorCollector,
                                   ITranslationSessionManager translationSessionManager) {
        this.Translators = translatorCollector;
        this.SessionManager = translationSessionManager;
        this.CopyCommand = new DelegateCommand<ValueToUse?>(this.CopyCommandAction);
        this.TranslateCommand = new DelegateCommand<ValueToUse?>(this.TranslateCommandAction);
        this.SaveCommand = new DelegateCommand(this.SaveCommandAction);
        this.CancelCommand = new DelegateCommand(this.CancelCommandAction);
        this.DeleteCommand = new DelegateCommand(this.DeleteCommandAction);
        this.OnLoaded = new DelegateCommand<RoutedEventArgs>(this.OnLoadedAction);
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
                result = await this.Translators.TranslateAsync(this.SessionManager.BaseLocalizationFile.Id, this.Entry?.Value);
                break;
            case ValueToUse.ValueMerge:
                result = await this.Translators.TranslateAsync(this.SessionManager.CurrentTranslationSession.MergeLocalizationId, this.Entry?.ValueMerge);
                break;
            case ValueToUse.ValueTranslation:
                // dont translate a translation :D
                break;
        }
        this.IsEnabled = !this.IsEnabled;
        if (result is null) {
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
        if (this.bindingGroup is not null
            && this.bindingGroup.CommitEdit()) {
            this.canCloseDialog = true;
            IDialogResult result = new DialogResult(ButtonResult.OK);
            this.Entry.ApplyChangesTo(this.original);
            result.Parameters.Add(nameof(IAppLocFileEntry), this.original);
            RequestClose?.Invoke(result);
            this.SetEntriesToNull();
        }
    }

    private void DeleteCommandAction() {
        if (this.Entry is null) {
            return;
        }
        if (!this.Entry.IsDeleteAble) {
            return;
        }
        if (IsActionInterrupted(I18NEdits.DialogDeleteCaption,
                                I18NEdits.DialogDeleteText)) {
            return;
        }
        this.canCloseDialog = true;
        this.Entry.Translation = null;
        IDialogResult result = new DialogResult(ButtonResult.Yes);
        this.Entry.ApplyChangesTo(this.original);
        result.Parameters.Add(nameof(IAppLocFileEntry), this.original);
        RequestClose?.Invoke(result);
        this.SetEntriesToNull();
    }

    private void CancelCommandAction() {
        if (this.Entry is null) {
            return;
        }
        if (this.IsCancelInterruptable()) {
            if (IsActionInterrupted(I18NEdits.DialogCancelCaption,
                                    I18NEdits.DialogCancelText)) {
                return;
            }
        }
        this.canCloseDialog = true;
        IDialogResult result = new DialogResult(ButtonResult.Cancel);
        result.Parameters.Add(nameof(IAppLocFileEntry), null);
        RequestClose?.Invoke(result);
        this.SetEntriesToNull();
    }

    private void SetEntriesToNull() {
        this.original = null;
        this.Entry = null;
    }

    private void OnLoadedAction(RoutedEventArgs e) {
        if (e.Source is Grid grid) {
            this.bindingGroup = grid.BindingGroup;
            this.InitBindingGroup();
            this.isLoaded = true;
        }
    }

    private void InitBindingGroup() {
        if (this.bindingGroup is not null) {
            this.bindingGroup.CancelEdit();
            this.bindingGroup.BeginEdit();
        }
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
        return !Equals(this.Entry?.Translation, this.original.Translation);
    }

    public bool CanCloseDialog() {
        return this.canCloseDialog;
    }

    public void OnDialogClosed() {
        // x in the top right corner is clicked
        // nothing can and should be done
    }

    public void OnDialogOpened(IDialogParameters parameters) {
        this.canCloseDialog = false;
        bool gotEntry = parameters.TryGetValue<IAppLocFileEntry>(nameof(IAppLocFileEntry), out IAppLocFileEntry entry);
        if (!gotEntry) {
            this.Entry = null;
            return;
        }
        this.original = entry;
        this.Entry = new EditEntry(this.SessionManager, entry);
        bool gotIsCount = parameters.TryGetValue<bool>(nameof(EditEntryLargeViewModel.IsCount), out bool isCount);
        this.IsCount = gotIsCount && isCount;
        if (!this.isLoaded) {
            return;
        }
        this.InitBindingGroup();
    }

    private void OnEntryChanged() {
        int lines = 3;
        if (this.Entry is not null) {
            if (this.Entry.Value is not null) {
                int valueLines = this.Entry.Value.Split("\n").Length;
                if (valueLines > lines) {
                    lines = valueLines;
                }
            }
        }
        int height = this.translationTextBoxHeightLine * this.translationTextBoxHeightLineMultiplier * lines;
        if (height > this.translationTextBoxHeightMax) {
            height = this.translationTextBoxHeightMax;
        }
        this.TranslationTextBoxHeight = height;
    }
}
