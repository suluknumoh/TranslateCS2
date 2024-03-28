using System;
using System.Collections.Generic;
using System.Linq;

using TranslateCS2.Models;
using TranslateCS2.Models.Exports;
using TranslateCS2.Models.Imports;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties;

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
                                                         I18N.ToolTipExportFormatDirectOverwrite);
            exportFormats.Add(exportFormat);
        }
        {
            ExportFormat exportFormat = new ExportFormat("json",
                                                         ExportFormats.JSON,
                                                         true,
                                                         I18N.ToolTipExportFormatJSON);
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

    public void Import(TranslationSession translationSession,
                       ImportModes importMode,
                       string selectedPath) {
        List<LocalizationDictionaryEntry>? imported = this._jsonService.ReadLocalizationFileJson(selectedPath);
        if (imported == null) {
            return;
        }
        switch (importMode) {
            case ImportModes.New:
                // clear all
                foreach (LocalizationDictionaryEntry? item in translationSession.LocalizationDictionary.Where(existingItem => !String.IsNullOrEmpty(existingItem.Translation) && !String.IsNullOrWhiteSpace(existingItem.Translation))) {
                    item.Translation = null;
                }
                // set all imported
                foreach (LocalizationDictionaryEntry importedItem in imported) {
                    IEnumerable<LocalizationDictionaryEntry> filtered = translationSession.LocalizationDictionary.Where(existingItem => existingItem.Key == importedItem.Key);
                    if (filtered.Any()) {
                        filtered.First().Translation = importedItem.Translation;
                    }
                }
                break;
            case ImportModes.LeftJoin:
                // set missing imported; all existing + imported that are missing
                foreach (LocalizationDictionaryEntry importedItem in imported) {
                    IEnumerable<LocalizationDictionaryEntry> filtered = translationSession.LocalizationDictionary.Where(existingItem => existingItem.Key == importedItem.Key && String.IsNullOrEmpty(existingItem.Translation) && String.IsNullOrWhiteSpace(existingItem.Translation));
                    if (filtered.Any()) {
                        filtered.First().Translation = importedItem.Translation;
                    }
                }
                break;
            case ImportModes.RightJoin:
                // set all imported; all imported + existing that werent imported
                foreach (LocalizationDictionaryEntry item in imported) {
                    IEnumerable<LocalizationDictionaryEntry> filtered = translationSession.LocalizationDictionary.Where(existingItem => existingItem.Key == item.Key);
                    if (filtered.Any()) {
                        filtered.First().Translation = item.Translation;
                    }
                }
                break;
        }
    }
}
