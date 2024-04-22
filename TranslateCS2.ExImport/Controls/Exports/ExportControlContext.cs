using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Sessions;
using TranslateCS2.ExImport.Helpers;
using TranslateCS2.ExImport.Models;
using TranslateCS2.ExImport.Properties.I18N;
using TranslateCS2.ExImport.Services;
using TranslateCS2.ModBridge;

namespace TranslateCS2.ExImport.Controls.Exports;

internal class ExportControlContext : BindableBase, INavigationAware {
    private readonly IViewConfigurations _viewConfigurations;
    private readonly ExImportService _exportService;
    private readonly string _dialogtitle = I18NExport.DialogTitle;
    private readonly string _dialogWarningCaption = I18NExport.DialogWarningCaption;
    private readonly string _dialogWarningText = I18NExport.DialogWarningText;


    public ITranslationSessionManager SessionManager { get; }


    private bool _IsEnabled;
    public bool IsEnabled {
        get => this._IsEnabled;
        set => this.SetProperty(ref this._IsEnabled, value);
    }


    private bool _IsExportButtonEnabled;
    public bool IsExportButtonEnabled {
        get => this._IsExportButtonEnabled;
        set => this.SetProperty(ref this._IsExportButtonEnabled, value);
    }


    private bool _IsAddKeyEnabled;
    public bool IsAddKeyEnabled {
        get => this._IsAddKeyEnabled;
        set => this.SetProperty(ref this._IsAddKeyEnabled, value);
    }


    private bool _IsAddMergeValuesEnabled;
    public bool IsAddMergeValuesEnabled {
        get => this._IsAddMergeValuesEnabled;
        set => this.SetProperty(ref this._IsAddMergeValuesEnabled, value);
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


    private bool _IsAddKey;
    public bool IsAddKey {
        get => this._IsAddKey;
        set => this.SetProperty(ref this._IsAddKey, value);
    }


    private bool _IsAddMergeValues;
    public bool IsAddMergeValues {
        get => this._IsAddMergeValues;
        set => this.SetProperty(ref this._IsAddMergeValues, value);
    }


    private Brush? _InfoMessageColor;
    public Brush? InfoMessageColor {
        get => this._InfoMessageColor;
        set => this.SetProperty(ref this._InfoMessageColor, value);
    }


    private ILocalizationFile? _ExportLocalizationFile;
    public ILocalizationFile? ExportLocalizationFile {
        get => this._ExportLocalizationFile;
        set => this.SetProperty(ref this._ExportLocalizationFile, value);
    }


    public ObservableCollection<ExportFormat> ExportFormats { get; }


    private ExportFormat _SelectedExportFormat;
    public ExportFormat SelectedExportFormat {
        get => this._SelectedExportFormat;
        set => this.SetProperty(ref this._SelectedExportFormat, value, this.OnChange);
    }


    public DelegateCommand SelectPathCommand { get; }
    public DelegateCommand ExportCommand { get; }


    public ExportControlContext(IViewConfigurations viewConfigurations,
                                ITranslationSessionManager translationSessionManager,
                                ExImportService exportService) {
        this._viewConfigurations = viewConfigurations;
        this.SessionManager = translationSessionManager;
        this._exportService = exportService;
        this.ExportFormats = new ObservableCollection<ExportFormat>(exportService.GetExportFormats());
        this._SelectedExportFormat = this.ExportFormats.First();
        this.SelectPathCommand = new DelegateCommand(this.SelectPathCommandAction);
        this.ExportCommand = new DelegateCommand(this.ExportCommandAction);
        this._IsEnabled = true;
        this._IsAddKey = false;
        this._IsAddMergeValues = false;
        this._IsExportButtonEnabled = true;
    }


    private void SelectPathCommandAction() {
        string? fileName = CultureInfoHelper.GatherCultureFromEnglishName(this.SessionManager.CurrentTranslationSession?.OverwriteLocalizationNameEN)?.Name;
        if (!StringHelper.IsNullOrWhiteSpaceOrEmpty(fileName)) {
            fileName += ".json";
        }
        string? path = this.SelectedPath;
        if (path == null) {
            string tempPath = Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low",
                                           "Colossal Order",
                                           "Cities Skylines II",
                                           ".cache",
                                           "Mods",
                                           "mods_subscribed");
            if (Path.Exists(tempPath)) {
                path = tempPath;
                tempPath = Path.Combine(tempPath, $"{ModConstants.ModId}_1");
                if (Path.Exists(tempPath)) {
                    path = tempPath;
                }
            }
        }
        string? selected = ImExportDialogHelper.ShowSaveFileDialog(path,
                                                                   this._dialogtitle,
                                                                   fileName,
                                                                   this._dialogWarningCaption,
                                                                   this._dialogWarningText);
        if (selected != null) {
            this.SelectedPath = selected;
        }
    }


    private void OnChange() {
        switch (this.SelectedExportFormat.Format) {
            case Models.ExportFormats.Direct:
                this.IsExportButtonEnabled = true;
                this.IsAddKeyEnabled = false;
                this.IsAddMergeValuesEnabled = false;
                break;
            case Models.ExportFormats.JSON:
                this.IsExportButtonEnabled = this.SelectedPath != null;
                this.IsAddKeyEnabled = true;
                this.IsAddMergeValuesEnabled = true;
                break;
        }
    }


    private async void ExportCommandAction() {
        MessageBoxResult result = MessageBox.Show(I18NExport.DialogText,
                                                  I18NExport.DialogTitle,
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Question,
                                                  MessageBoxResult.No,
                                                  MessageBoxOptions.None);
        if (result == MessageBoxResult.Yes) {
            this.InfoMessageColor = Brushes.Black;
            this.InfoMessage = I18NExport.MessagePrepareDo;
            this.IsEnabled = false;
            this.IsExportButtonEnabled = false;
            this.ExportLocalizationFile = this.SessionManager.GetForExport();
            this.InfoMessageColor = Brushes.DarkGreen;
            this.InfoMessage = I18NExport.MessagePrepareSuccess;
            this.InfoMessageColor = Brushes.DarkGreen;
            this.InfoMessage = I18NExport.MessageDo;
            try {
                await this._exportService.Export(this.SelectedExportFormat,
                                                 this.ExportLocalizationFile,
                                                 this.IsAddKey,
                                                 this.IsAddMergeValues,
                                                 this.SelectedPath);
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NExport.MessageSuccess;
            } catch {
                this.InfoMessageColor = Brushes.DarkRed;
                this.InfoMessage = I18NExport.MessageFail;
            }
            this.IsEnabled = true;
            this.IsExportButtonEnabled = true;
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
