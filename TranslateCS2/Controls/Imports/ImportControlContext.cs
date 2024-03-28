using System;
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
using TranslateCS2.Properties;
using TranslateCS2.Services;

namespace TranslateCS2.Controls.Imports;

internal class ImportControlContext : BindableBase, INavigationAware {
    private readonly ViewConfigurations _viewConfigurations;
    private readonly TranslationSessionManager _translationSessionManager;
    private readonly ExImportService _exportService;


    private bool _IsEnabled;
    public bool IsEnabled {
        get => this._IsEnabled;
        set => this.SetProperty(ref this._IsEnabled, value);
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


    public DelegateCommand SelectPathCommand { get; }
    public DelegateCommand ImportCommand { get; }


    public ImportControlContext(ViewConfigurations viewConfigurations,
                                TranslationSessionManager translationSessionManager,
                                ExImportService exportService) {
        this._viewConfigurations = viewConfigurations;
        this._translationSessionManager = translationSessionManager;
        this._exportService = exportService;
        this.SelectPathCommand = new DelegateCommand(this.SelectPathCommandAction);
        this.ImportCommand = new DelegateCommand(this.ImportCommandAction);
        this._ImportMode = ImportModes.New;
        this._IsEnabled = true;
        this._IsImportButtonEnabled = true;
    }

    private void SelectPathCommandAction() {
        string? selected = ImExportDialogHelper.ShowOpenFileDialog(this.SelectedPath);
        if (selected != null) {
            this.SelectedPath = selected;
        }
    }


    private void OnChange() {
        this.IsImportButtonEnabled = !String.IsNullOrEmpty(this.SelectedPath) && !String.IsNullOrWhiteSpace(this.SelectedPath);
    }


    private void ImportCommandAction() {
        MessageBoxResult result = MessageBox.Show(I18N.QuestionAreYouSure,
                                                  I18N.StringExportTranslation,
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Question,
                                                  MessageBoxResult.No,
                                                  MessageBoxOptions.None);
        if (result == MessageBoxResult.Yes) {
            Task.Factory.StartNew(() => {
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18N.MessagePreparingTranslationExport;
                this.IsEnabled = false;
                this.IsImportButtonEnabled = false;
            })
            .ContinueWith((t) => {
                try {
                    this._exportService.Import(this._translationSessionManager.CurrentTranslationSession,
                                               this.ImportMode,
                                               this.SelectedPath);
                    this._translationSessionManager.SaveCurrentTranslationSessionsTranslations();
                    return null;
                } catch (Exception ex) {
                    return String.Empty;
                }
            })
            .ContinueWith((t) => {
                if (t.GetAwaiter().GetResult() is string error) {
                    this.InfoMessageColor = Brushes.DarkRed;
                    this.InfoMessage = I18N.MessageImportFailed;
                } else {
                    this.InfoMessageColor = Brushes.DarkGreen;
                    this.InfoMessage = I18N.StringImported;
                }
                this.IsEnabled = true;
                this.IsImportButtonEnabled = true;
            });
        }
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        //
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        //
    }
}
