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
using TranslateCS2.Models;
using TranslateCS2.Models.Exports;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties.I18N;
using TranslateCS2.Services;

namespace TranslateCS2.Controls.Exports;

internal class ExportControlContext : BindableBase, INavigationAware {
    private readonly ViewConfigurations _viewConfigurations;
    private readonly TranslationSessionManager _translationSessionManager;
    private readonly ExImportService _exportService;
    private readonly string _dialogtitle = I18NExport.DialogTitle;
    private readonly string _dialogWarningCaption = I18NExport.DialogWarningCaption;
    private readonly string _dialogWarningText = I18NExport.DialogWarningText;


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


    private LocalizationFile? _ExportLocalizationFile;
    public LocalizationFile? ExportLocalizationFile {
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


    public ExportControlContext(ViewConfigurations viewConfigurations,
                                TranslationSessionManager translationSessionManager,
                                ExImportService exportService) {
        this._viewConfigurations = viewConfigurations;
        this._translationSessionManager = translationSessionManager;
        this._exportService = exportService;
        this.ExportFormats = new ObservableCollection<ExportFormat>(exportService.GetExportFormats());
        this._SelectedExportFormat = this.ExportFormats.First();
        this.SelectPathCommand = new DelegateCommand(this.SelectPathCommandAction);
        this.ExportCommand = new DelegateCommand(this.ExportCommandAction);
        this._IsEnabled = true;
        this._IsExportButtonEnabled = true;
    }


    private void SelectPathCommandAction() {
        string? selected = ImExportDialogHelper.ShowSaveFileDialog(this.SelectedPath,
                                                                   this._dialogtitle,
                                                                   this._dialogWarningCaption,
                                                                   this._dialogWarningText);
        if (selected != null) {
            this.SelectedPath = selected;
        }
    }


    private void OnChange() {
        switch (this.SelectedExportFormat.Format) {
            case Models.Exports.ExportFormats.Direct:
                this.IsExportButtonEnabled = true;
                break;
            case Models.Exports.ExportFormats.JSON:
                this.IsExportButtonEnabled = this.SelectedPath != null;
                break;
        }
    }


    private void ExportCommandAction() {
        MessageBoxResult result = MessageBox.Show(I18NExport.DialogText,
                                                  I18NExport.DialogTitle,
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Question,
                                                  MessageBoxResult.No,
                                                  MessageBoxOptions.None);
        if (result == MessageBoxResult.Yes) {
            Task.Factory.StartNew(() => {
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18NExport.MessagePrepareDo;
                this.IsEnabled = false;
                this.IsExportButtonEnabled = false;
            })
            .ContinueWith((t) => this.ExportLocalizationFile = this._translationSessionManager.GetForExport())
            .ContinueWith((t) => {
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NExport.MessagePrepareSuccess;
            }).ContinueWith((t) => {
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NExport.MessageDo;
            })
            .ContinueWith((t) => {
                try {
                    this._exportService.Export(this.SelectedExportFormat, this.ExportLocalizationFile, this.SelectedPath);
                } catch {
                    return I18NExport.MessageFail;
                }
                return null;
            })
            .ContinueWith((t) => {
                if (t.GetAwaiter().GetResult() is string error) {
                    this.InfoMessageColor = Brushes.DarkRed;
                    this.InfoMessage = error;
                } else {
                    this.InfoMessageColor = Brushes.DarkGreen;
                    this.InfoMessage = I18NExport.MessageSuccess;
                }

                this.IsEnabled = true;
                this.IsExportButtonEnabled = true;
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
