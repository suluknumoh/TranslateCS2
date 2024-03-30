using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Configurations.Views;
using TranslateCS2.Helpers;
using TranslateCS2.Models.Imports;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties.I18N;
using TranslateCS2.Services;

namespace TranslateCS2.Controls.Imports;

internal class ImportControlContext : BindableBase, INavigationAware {
    private readonly ViewConfigurations _viewConfigurations;
    private readonly TranslationSessionManager _translationSessionManager;
    private readonly ExImportService _exportService;
    private readonly string _dialogtitle = I18NImport.DialogTitle;
    private readonly string _dialogWarningCaption = I18NImport.DialogWarningCaption;
    private readonly string _dialogWarningText = I18NImport.DialogWarningText;


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


    private ImportModes _ImportMode;
    public ImportModes ImportMode {
        get => this._ImportMode;
        set => this.SetProperty(ref this._ImportMode, value);
    }


    public ObservableCollection<CompareExistingImportedTranslations> Preview { get; } = [];
    public bool IsPreviewAvailable => this.Preview.Any();


    public DelegateCommand SelectPathCommand { get; }
    public DelegateCommand ReadCommand { get; }
    public DelegateCommand ImportCommand { get; }


    public ImportControlContext(ViewConfigurations viewConfigurations,
                                TranslationSessionManager translationSessionManager,
                                ExImportService exportService) {
        this._viewConfigurations = viewConfigurations;
        this._translationSessionManager = translationSessionManager;
        this._exportService = exportService;
        this.SelectPathCommand = new DelegateCommand(this.SelectPathCommandAction);
        this.ReadCommand = new DelegateCommand(this.ReadCommandAction);
        this.ImportCommand = new DelegateCommand(this.ImportCommandAction);
        this._ImportMode = ImportModes.LeftJoin;
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
            this.IsEnabled = false;
            this.IsReadButtonEnabled = false;
            Application.Current.Dispatcher.Invoke(this.Preview.Clear);
            this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
            this.InfoMessageColor = Brushes.Black;
            this.InfoMessage = I18NImport.MessageRead;
        })
        .ContinueWith((t) => {
            try {
                return this._exportService.Import(this._translationSessionManager.CurrentTranslationSession,
                                                  this.SelectedPath);
            } catch (Exception ex) {
                return null;
            }
        })
        .ContinueWith((t) => {
            if (t.GetAwaiter().GetResult() is List<CompareExistingImportedTranslations> preview) {
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NImport.MessageReadSuccess;
                Application.Current.Dispatcher.Invoke(() => this.Preview.AddRange(preview));
                this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
                this.IsImportButtonEnabled = true;
            } else {
                this.InfoMessageColor = Brushes.DarkRed;
                this.InfoMessage = I18NImport.MessageReadFail;
            }
            this.IsEnabled = true;
            this.IsReadButtonEnabled = true;
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
                this.IsEnabled = false;
                this.IsReadButtonEnabled = false;
                this.IsImportButtonEnabled = false;
                DatabaseHelper.BackUpIfExists(Databases.DatabaseBackUpIndicators.BEFORE_IMPORT);
            })
            .ContinueWith(t => this._translationSessionManager.HandleImported(this.Preview, this.ImportMode))
            .ContinueWith(t => {
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18NImport.MessageImport;
                this._translationSessionManager.SaveCurrentTranslationSessionsTranslations();
                this._translationSessionManager.CurrentTranslationSessionChanged();
            })
            .ContinueWith(t => {
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NImport.MessageImportSuccess;
                Application.Current.Dispatcher.Invoke(this.Preview.Clear);
                this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
                this.IsEnabled = true;
                this.IsReadButtonEnabled = true;
                this.IsImportButtonEnabled = true;
            })
            ;
        }
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        this.Preview.Clear();
        this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
        this.InfoMessage = null;
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        this.Preview.Clear();
        this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
        this.InfoMessage = null;
    }
}
