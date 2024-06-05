using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Prism.Commands;
using Prism.Regions;

using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.ViewModels;
using TranslateCS2.ExImport.Helpers;
using TranslateCS2.ExImport.Models;
using TranslateCS2.ExImport.Properties.I18N;
using TranslateCS2.ExImport.Services;
using TranslateCS2.Inf;

namespace TranslateCS2.ExImport.Controls.Exports;
internal class ExportControlContext : ABaseViewModel {
    // here we dont need a streamingDataPath, so its String.Empty
    private readonly Paths pathHelper = new Paths(false, String.Empty);
    private readonly IViewConfigurations viewConfigurations;
    private readonly ExImportService exportService;
    private readonly string dialogtitle = I18NExport.DialogTitle;
    private readonly string dialogWarningCaption = I18NExport.DialogWarningCaption;
    private readonly string dialogWarningText = I18NExport.DialogWarningText;


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


    public ObservableCollection<string> FileNameProposals { get; } = [];


    private string? _SelectedFileNameProposal;
    public string? SelectedFileNameProposal {
        get => this._SelectedFileNameProposal;
        set => this.SetProperty(ref this._SelectedFileNameProposal, value, this.OnChange);
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


    public ObservableCollection<ExportFormat> ExportFormats { get; } = [];


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
        this.viewConfigurations = viewConfigurations;
        this.SessionManager = translationSessionManager;
        this.exportService = exportService;
        this.SelectPathCommand = new DelegateCommand(this.SelectPathCommandAction);
        this.ExportCommand = new DelegateCommand(this.ExportCommandAction);
        this._IsEnabled = true;
        this._IsAddKey = false;
        this._IsAddMergeValues = false;
        this._IsExportButtonEnabled = true;
    }


    private void SelectPathCommandAction() {
        string? path = this.SelectedPath;
        path ??= this.pathHelper.TryToGetModsPath();
        string? selected = ImExportDialogHelper.ShowSaveFileDialog(path,
                                                                   this.dialogtitle,
                                                                   StringHelper.GetNullForEmpty(this.SelectedFileNameProposal),
                                                                   this.dialogWarningCaption,
                                                                   this.dialogWarningText);
        if (selected is not null) {
            this.SelectedPath = selected;
        }
    }


    private void OnChange() {
        if (this.SelectedExportFormat is null) {
            return;
        }
        switch (this.SelectedExportFormat.Format) {
            case Models.ExportFormats.JSON:
                this.IsExportButtonEnabled = this.SelectedPath is not null;
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
            try {
                this.InfoMessageColor = Brushes.Black;
                this.InfoMessage = I18NExport.MessagePrepareDo;
                this.viewConfigurations.DeActivateRibbon?.Invoke(false);
                this.IsEnabled = false;
                this.IsExportButtonEnabled = false;
                IDictionary<string, string> localizationDictionary = this.PrepareForExport();
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NExport.MessagePrepareSuccess;
                await Task.Delay(TimeSpan.FromSeconds(2));
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NExport.MessageDo;
                await this.exportService.Export(this.SelectedExportFormat,
                                                localizationDictionary,
                                                this.SelectedPath);
                this.InfoMessageColor = Brushes.DarkGreen;
                this.InfoMessage = I18NExport.MessageSuccess;
            } catch {
                this.InfoMessageColor = Brushes.DarkRed;
                this.InfoMessage = I18NExport.MessageFail;
            }
            this.IsEnabled = true;
            this.IsExportButtonEnabled = true;
            this.viewConfigurations.DeActivateRibbon?.Invoke(true);
        }
    }

    public override void OnNavigatedTo(NavigationContext navigationContext) {
        this.ExportFormats.Clear();
        List<ExportFormat> formats = this.exportService.GetExportFormats();
        this.ExportFormats.AddRange(formats);
        this.SelectedExportFormat = this.ExportFormats.First();

        string? selectedFileNameProposal = this.SelectedFileNameProposal;
        this.FileNameProposals.Clear();
        IEnumerable<CultureInfo>? guessedCultures = CultureInfoHelper.GatherCulturesFromEnglishName(this.SessionManager.CurrentTranslationSession?.LocNameEnglish);
        if (guessedCultures is not null) {
            foreach (CultureInfo guessedCulture in guessedCultures) {
                this.FileNameProposals.Add($"{guessedCulture.Name}{ModConstants.JsonExtension}");
            }
        }
        if (selectedFileNameProposal is not null
            && this.FileNameProposals.Contains(selectedFileNameProposal)) {
            this.SelectedFileNameProposal = selectedFileNameProposal;
        } else {
            this.SelectedFileNameProposal = null;
            if (this.FileNameProposals.Count > 0) {
                this.SelectedFileNameProposal = this.FileNameProposals.First();
            }
        }
    }

    public IDictionary<string, string> PrepareForExport() {
        ITranslationSession? session = this.SessionManager.CurrentTranslationSession;
        ArgumentNullException.ThrowIfNull(session);
        Dictionary<string, string> dictionary = [];
        if (this.IsAddKey
            && session.Name is not null
            && !StringHelper.IsNullOrWhiteSpaceOrEmpty(session.Name)) {
            dictionary.Add(ModConstants.LocaleNameLocalizedKey, session.Name);
        }
        ObservableCollection<KeyValuePair<string, IAppLocFileEntry>> localizations = session.Localizations;
        foreach (KeyValuePair<string, IAppLocFileEntry> localization in localizations) {
            if (localization.Key is null) {
                continue;
            }
            string? val = null;
            if (!StringHelper.IsNullOrWhiteSpaceOrEmpty(localization.Value.Translation)) {
                val = localization.Value.Translation;
            }
            if (val is null
                && this.IsAddMergeValues) {
                val = localization.Value.ValueMerge;
            }
            if (val is null) {
                continue;
            }
            dictionary.Add(localization.Key, val);
        }
        return dictionary;
    }
}
