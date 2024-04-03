using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using TranslateCS2.Configurations.Views;
using TranslateCS2.Helpers;
using TranslateCS2.Models.Imports;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties.I18N;
using TranslateCS2.Services;
using TranslateCS2.ViewModels.Dialogs;
using TranslateCS2.Views.Dialogs;

namespace TranslateCS2.Controls.Imports;

internal class ImportControlContext : BindableBase, INavigationAware {
    private readonly ViewConfigurations _viewConfigurations;
    private readonly ExImportService _exportService;
    private readonly IDialogService _dialogService;
    private readonly string _dialogtitle = I18NImport.DialogTitle;
    private readonly string _dialogWarningCaption = I18NImport.DialogWarningCaption;
    private readonly string _dialogWarningText = I18NImport.DialogWarningText;


    public ComparisonDataGridContext CDGContext { get; }


    public TranslationSessionManager SessionManager { get; }


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
        set => this.SetProperty(ref this._InfoMessage, value);
    }


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


    public ImportControlContext(ViewConfigurations viewConfigurations,
                                TranslationSessionManager translationSessionManager,
                                ExImportService exportService,
                                IDialogService dialogService) {
        this._viewConfigurations = viewConfigurations;
        this.SessionManager = translationSessionManager;
        this._exportService = exportService;
        this._dialogService = dialogService;
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


    private void ReadCommandAction() {
        Task.Factory.StartNew(() => {
            this.SwitchEnablements(false);
            this.CDGContext.Clear();
            this.RaisePropertyChanged(nameof(this.CDGContext));
            this.InfoMessageColor = Brushes.Black;
            this.InfoMessage = I18NImport.MessageRead;
        })
        .ContinueWith((t) => {
            try {
                return this._exportService.ReadToReview(this.SessionManager.CurrentTranslationSession,
                                                        this.SelectedPath);
            } catch {
                return null;
            }
        })
        .ContinueWith((t) => {
            if (t.GetAwaiter().GetResult() is List<CompareExistingReadTranslation> preview) {
                if (false) {
                    preview.RemoveAll(i => i.IsEqual());
                }
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NImport.MessageReadSuccess;
                this.CDGContext.SetItems(preview);
                this.RaisePropertyChanged(nameof(this.CDGContext));
                this.CDGContext.Raiser();
                this.SwitchEnablements(true);
            } else {
                this.InfoMessageColor = Brushes.DarkRed;
                this.InfoMessage = I18NImport.MessageReadFail;
                this.SwitchEnablements(false);
            }
        });
    }

    private void ImportCommandAction() {
        MessageBoxResult result = MessageBox.Show(I18NImport.DialogText,
                                                  I18NImport.DialogTitle,
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Question,
                                                  MessageBoxResult.No,
                                                  MessageBoxOptions.None);
        if (result == MessageBoxResult.Yes) {
            Task.Factory.StartNew(() => {
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18NGlobal.MessageDatabaseBackUp;
                this.SwitchEnablements(true);
                DatabaseHelper.BackUpIfExists(Databases.DatabaseBackUpIndicators.BEFORE_IMPORT);
            })
            .ContinueWith(t => this.SessionManager.HandleImported(this.CDGContext.GetItems(), this.CDGContext.ImportMode))
            .ContinueWith(t => {
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18NImport.MessageImport;
                this.SessionManager.SaveCurrentTranslationSessionsTranslations();
                this.SessionManager.CurrentTranslationSessionChanged();
            })
            .ContinueWith(t => {
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NImport.MessageImportSuccess;
                this.CDGContext.Clear();
                this.SwitchEnablements(false);
            });
        }
    }

    private void OpenComparisonInNewWindowCommandAction() {
        this.SwitchEnablements(true);
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
        this.SwitchEnablements(true);
    }

    private void SwitchEnablements(bool withImportButton) {
        this.IsEnabled = !this.IsEnabled;
        this.IsReadButtonEnabled = !this.IsReadButtonEnabled;
        if (withImportButton) {
            this.IsImportButtonEnabled = !this.IsImportButtonEnabled;
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
