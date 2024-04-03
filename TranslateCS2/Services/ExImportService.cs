using System.Collections.Generic;
using System.Linq;

using TranslateCS2.Helpers;
using TranslateCS2.Models;
using TranslateCS2.Models.Exports;
using TranslateCS2.Models.Imports;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties.I18N;

namespace TranslateCS2.Services;
internal class ExImportService {
    private readonly LocalizationFilesService _localizationFilesService;
    private readonly TranslationSessionManager _translationSessionManager;
    private readonly JSONService _jsonService;

    public ExImportService(TranslationSessionManager translationSessionManager,
                           LocalizationFilesService localizationFilesService,
                           JSONService jsonService) {
        this._translationSessionManager = translationSessionManager;
        this._localizationFilesService = localizationFilesService;
        this._jsonService = jsonService;
    }

    public List<ExportFormat> GetExportFormats() {
        List<ExportFormat> exportFormats = [];
        {
            ExportFormat exportFormat = new ExportFormat("direct-overwrite",
                                                         ExportFormats.Direct,
                                                         false,
                                                         I18NExport.ToolTipExportFormatDirectOverwrite);
            exportFormats.Add(exportFormat);
        }
        {
            ExportFormat exportFormat = new ExportFormat("json",
                                                         ExportFormats.JSON,
                                                         true,
                                                         I18NExport.ToolTipExportFormatJSON);
            exportFormats.Add(exportFormat);
        }
        return exportFormats;
    }

    public void Export(ExportFormat exportFormat,
                       LocalizationFile localizationFile,
                       string? file) {
        switch (exportFormat.Format) {
            case ExportFormats.Direct:
                this._localizationFilesService.WriteLocalizationFileDirect(localizationFile);
                break;
            case ExportFormats.JSON:
                this._jsonService.WriteLocalizationFileJson(localizationFile, file);
                break;
        }
    }

    public List<CompareExistingReadTranslation>? ReadToReview(TranslationSession translationSession,
                                                                  string selectedPath) {
        List<LocalizationDictionaryEntry>? imports = this._jsonService.ReadLocalizationFileJson(selectedPath);
        if (imports == null) {
            return null;
        }
        List<CompareExistingReadTranslation> preview = [];
        foreach (LocalizationDictionaryEntry existing in translationSession.LocalizationDictionary) {
            IEnumerable<LocalizationDictionaryEntry> importsForKey = imports.Where(item => item.Key == existing.Key);
            string key = existing.Key;
            string? translationExisting = existing.Translation;
            string? translationRead = null;
            if (importsForKey.Any()) {
                translationRead = StringHelper.GetNullForEmpty(importsForKey.First().Translation);
            }
            if (StringHelper.IsNullOrWhiteSpaceOrEmpty(translationRead)
                && StringHelper.IsNullOrWhiteSpaceOrEmpty(translationExisting)) {
                continue;
            }
            CompareExistingReadTranslation previewEntry = new CompareExistingReadTranslation(key, translationExisting, translationRead);
            preview.Add(previewEntry);
        }
        return preview;
    }
}
