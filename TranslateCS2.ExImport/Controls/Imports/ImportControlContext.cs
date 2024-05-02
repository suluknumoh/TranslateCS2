using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Services.Databases;
using TranslateCS2.Core.Sessions;
using TranslateCS2.ExImport.Helpers;
using TranslateCS2.ExImport.Models;
using TranslateCS2.ExImport.Properties.I18N;
using TranslateCS2.ExImport.Services;
using TranslateCS2.ExImport.ViewModels.Dialogs;
using TranslateCS2.ExImport.Views.Dialogs;
using TranslateCS2.Inf;

namespace TranslateCS2.ExImport.Controls.Imports;

internal class ImportControlContext : BindableBase, INavigationAware {
    private readonly IViewConfigurations _viewConfigurations;
    private readonly ExImportService _exportImportService;
    private readonly IDialogService _dialogService;
    private readonly ITranslationsDatabaseService _db;
    private readonly string _dialogtitle = I18NImport.DialogTitle;
    private readonly string _dialogWarningCaption = I18NImport.DialogWarningCaption;
    private readonly string _dialogWarningText = I18NImport.DialogWarningText;


    public ComparisonDataGridContext CDGContext { get; }


    public ITranslationSessionManager SessionManager { get; }


    private bool _IsEnabled;
    public bool IsEnabled {
        get => this._IsEnabled;
        set => this.SetProperty(ref this._IsEnabled, value);
    }


    private bool _IsReadButtonEnabled;
    public bool IsReadButtonEnabled {
        get => this._IsReadButtonEnabled;
        set => this.SetProperty(ref this._IsReadButtonEnabled, value);
    }


    private bool _IsImportButtonEnabled;
    public bool IsImportButtonEnabled {
        get => this._IsImportButtonEnabled;
        set => this.SetProperty(ref this._IsImportButtonEnabled, value);
    }


    private string? _InfoMessage;
    public string? InfoMessage {
        get => this._InfoMessage;
        set => this.SetProperty(ref this._InfoMessage, value, () => this.RaisePropertyChanged(nameof(this.IsDisplayMessage)));
    }


    public bool IsDisplayMessage => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this._InfoMessage);


    private string? _SelectedPath;
    public string? SelectedPath {
        get => this._SelectedPath;
        set => this.SetProperty(ref this._SelectedPath, value, this.OnChange);
    }


    private Brush? _InfoMessageColor;
    public Brush? InfoMessageColor {
        get => this._InfoMessageColor;
        set => this.SetProperty(ref this._InfoMessageColor, value);
    }



    public DelegateCommand SelectPathCommand { get; }
    public DelegateCommand ReadCommand { get; }
    public DelegateCommand OpenComparisonInNewWindowCommand { get; }
    public DelegateCommand ImportCommand { get; }


    public ImportControlContext(IViewConfigurations viewConfigurations,
                                ITranslationSessionManager translationSessionManager,
                                ExImportService exportService,
                                IDialogService dialogService,
                                ITranslationsDatabaseService db) {
        this._viewConfigurations = viewConfigurations;
        this.SessionManager = translationSessionManager;
        this._exportImportService = exportService;
        this._dialogService = dialogService;
        this._db = db;
        this.CDGContext = new ComparisonDataGridContext();
        this.SelectPathCommand = new DelegateCommand(this.SelectPathCommandAction);
        this.ReadCommand = new DelegateCommand(this.ReadCommandAction);
        this.OpenComparisonInNewWindowCommand = new DelegateCommand(this.OpenComparisonInNewWindowCommandAction);
        this.ImportCommand = new DelegateCommand(this.ImportCommandAction);
        this.CDGContext.ImportMode = ImportModes.LeftJoin;
        this._IsEnabled = true;
        this._IsReadButtonEnabled = false;
        this._IsImportButtonEnabled = false;
    }

    private void SelectPathCommandAction() {
        string? selected = ImExportDialogHelper.ShowOpenFileDialog(this.SelectedPath,
                                                                   this._dialogtitle,
                                                                   this._dialogWarningCaption,
                                                                   this._dialogWarningText);
        if (selected != null) {
            this.SelectedPath = selected;
        }
    }


    private void OnChange() {
        this.IsReadButtonEnabled = !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.SelectedPath);
    }


    private async void ReadCommandAction() {
        this._viewConfigurations.DeActivateRibbon?.Invoke(false);
        this.Disable(true);
        this.CDGContext.Clear();
        this.RaisePropertyChanged(nameof(this.CDGContext));
        this.InfoMessageColor = Brushes.Black;
        this.InfoMessage = I18NImport.MessageRead;
        List<CompareExistingReadTranslation>? preview = null;
        try {
            preview = await this._exportImportService.ReadToReview(this.SessionManager.CurrentTranslationSession,
                                                                   this.SelectedPath);
        } catch {
            // nix
        }
        if (preview is not null) {
            if (false) {
                preview.RemoveAll(i => i.IsEqual());
            }
            this.InfoMessageColor = Brushes.DarkGreen;
            this.InfoMessage = I18NImport.MessageReadSuccess;
            await Task.Delay(TimeSpan.FromSeconds(3));
            this.InfoMessage = null;
            this.CDGContext.SetItems(preview);
            this.RaisePropertyChanged(nameof(this.CDGContext));
            this.CDGContext.Raiser();
            this.Enable(true);
        } else {
            this.InfoMessageColor = Brushes.DarkRed;
            this.InfoMessage = I18NImport.MessageReadFail;
            this.Disable(false);
        }
        this._viewConfigurations.DeActivateRibbon?.Invoke(true);
    }

    private async void ImportCommandAction() {
        MessageBoxResult result = MessageBox.Show(I18NImport.DialogText,
                                                  I18NImport.DialogTitle,
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Question,
                                                  MessageBoxResult.No,
                                                  MessageBoxOptions.None);
        if (result == MessageBoxResult.Yes) {
            await Task.Factory.StartNew(() => {
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18NGlobal.MessageDatabaseBackUp;
                this.Disable(true);
                this._viewConfigurations.DeActivateRibbon?.Invoke(false);
                this._db.BackUpIfExists(DatabaseBackUpIndicators.BEFORE_IMPORT);
                this._exportImportService.HandleRead(this.CDGContext.GetItems(),
                                                         this.SessionManager.CurrentTranslationSession.LocalizationDictionary,
                                                         this.CDGContext.ImportMode);
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18NImport.MessageImport;
                this.SessionManager.SaveCurrentTranslationSessionsTranslations();
                this.SessionManager.CurrentTranslationSessionChanged();
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NImport.MessageImportSuccess;
                this.CDGContext.Clear();
                this.Enable(true);
                this._viewConfigurations.DeActivateRibbon?.Invoke(true);
            });
        }
    }

    private void OpenComparisonInNewWindowCommandAction() {
        this.Disable(true);
        DialogParameters dialogParameters = new DialogParameters {
            { ImportComparisonViewModel.ContextName, this.CDGContext }
        };
        if (false) {
            // non-modal/non-blocking dialog
            this._dialogService.Show(nameof(ImportComparisonView), dialogParameters, this.OnDialogClosed);
        } else {
            // modal/blocking dialog
            this._dialogService.ShowDialog(nameof(ImportComparisonView), dialogParameters, this.OnDialogClosed);
        }
    }

    private void OnDialogClosed(IDialogResult? result) {
        this.Enable(true);
    }

    private void Disable(bool withImportButton) {
        this.IsEnabled = false;
        this.IsReadButtonEnabled = false;
        if (withImportButton) {
            this.IsImportButtonEnabled = false;
        }
    }
    private void Enable(bool withImportButton) {
        this.IsEnabled = true;
        this.IsReadButtonEnabled = true;
        if (withImportButton) {
            this.IsImportButtonEnabled = true;
        }
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        this.CDGContext.OnNavigatedFrom(navigationContext);
        this.InfoMessage = null;
        this.IsImportButtonEnabled = false;
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        this.CDGContext.OnNavigatedTo(navigationContext);
        this.InfoMessage = null;
        this.IsImportButtonEnabled = false;
    }
}
