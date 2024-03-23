using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Configurations;
using TranslateCS2.Configurations.Views;
using TranslateCS2.Models;
using TranslateCS2.Models.Exports;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Services;

namespace TranslateCS2.ViewModels.Works;
internal class ExportViewModel : BindableBase, INavigationAware {
    private readonly ViewConfigurations _viewConfigurations;
    private readonly TranslationSessionManager _translationSessionManager;
    private readonly ExportService _exportService;


    private bool _IsEnabled;
    public bool IsEnabled {
        get => this._IsEnabled;
        set => this.SetProperty(ref this._IsEnabled, value);
    }


    private string? _InfoMessage;
    public string? InfoMessage {
        get => this._InfoMessage;
        set => this.SetProperty(ref this._InfoMessage, value);
    }


    private Brush? _InfoMessageColor;
    public Brush? InfoMessageColor {
        get => this._InfoMessageColor;
        set => this.SetProperty(ref this._InfoMessageColor, value);
    }


    private LocalizationFile<LocalizationDictionaryExportEntry>? _Merged;
    public LocalizationFile<LocalizationDictionaryExportEntry>? Merged {
        get => this._Merged;
        set => this.SetProperty(ref this._Merged, value);
    }


    public ObservableCollection<ExportFormat> ExportFormats { get; }


    private ExportFormat _SelectedExportFormat;
    public ExportFormat SelectedExportFormat {
        get => this._SelectedExportFormat;
        set => this.SetProperty(ref this._SelectedExportFormat, value);
    }


    public DelegateCommand ExportCommand { get; }


    public ExportViewModel(ViewConfigurations viewConfigurations, TranslationSessionManager translationSessionManager, ExportService exportService) {
        this._viewConfigurations = viewConfigurations;
        this._translationSessionManager = translationSessionManager;
        this._exportService = exportService;
        this.ExportFormats = new ObservableCollection<ExportFormat>(exportService.GetExportFormats());
        this._SelectedExportFormat = this.ExportFormats.First();
        this.ExportCommand = new DelegateCommand(this.ExportCommandAction);
        this._IsEnabled = true;
    }

    private void ExportCommandAction() {
        MessageBoxResult result = MessageBox.Show(I18N.QuestionAreYouSure, I18N.StringExportTranslation, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.None);
        if (result == MessageBoxResult.Yes) {

            Task.Factory.StartNew(() => {
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18N.MessagePreparingTranslationExport;
                this.IsEnabled = false;
            })
        .ContinueWith((t) => this.Merged = this._translationSessionManager.GetMerged())
        .ContinueWith((t) => {
            this.InfoMessageColor = Brushes.DarkGreen;
            this.InfoMessage = I18N.MessageTranslationReadyExport;
        }).ContinueWith((t) => {
            this.InfoMessageColor = Brushes.DarkGreen;
            this.InfoMessage = I18N.StringExporting;
        })
            .ContinueWith((t) => this._exportService.Export(this.SelectedExportFormat, this.Merged))
            .ContinueWith((t) => {
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18N.StringExported;
                this.IsEnabled = true;
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
