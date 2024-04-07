using System.Collections.Generic;
using System.Linq;

using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Services.LocalizationFiles;
using TranslateCS2.Core.Sessions;
using TranslateCS2.ExImport.Models;
using TranslateCS2.ExImport.Properties.I18N;

namespace TranslateCS2.ExImport.Services;
internal class ExImportService {
    private readonly ILocalizationFilesService _localizationFilesService;
    private readonly JSONService _jsonService;

    public ExImportService(ILocalizationFilesService localizationFilesService,
                           JSONService jsonService) {
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
                       ILocalizationFile localizationFile,
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

    public List<CompareExistingReadTranslation>? ReadToReview(ITranslationSession translationSession,
                                                              string selectedPath) {
        List<ILocalizationDictionaryEntry>? imports = this._jsonService.ReadLocalizationFileJson(selectedPath);
        if (imports == null) {
            return null;
        }
        List<CompareExistingReadTranslation> preview = [];
        foreach (ILocalizationDictionaryEntry existing in translationSession.LocalizationDictionary) {
            IEnumerable<ILocalizationDictionaryEntry> importsForKey = imports.Where(item => item.Key == existing.Key);
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

    public void HandleImported(IList<CompareExistingReadTranslation> preview, IList<ILocalizationDictionaryEntry> localizationDictionary, ImportModes importMode) {
        foreach (ILocalizationDictionaryEntry currentEntry in localizationDictionary) {
            foreach (CompareExistingReadTranslation compareItem in preview) {
                if (compareItem.Key == currentEntry.Key) {
                    switch (importMode) {
                        case ImportModes.NEW:
                            // set all read
                            currentEntry.Translation = compareItem.TranslationRead;
                            break;
                        case ImportModes.LeftJoin:
                            // set missing read; all existing + read that are missing
                            // preview leads!
                            // preview knows about Existing and Read!
                            // take care of method name 'is ... EXISTING available'
                            if (!compareItem.IsTranslationExistingAvailable) {
                                // only set if no translation existed
                                currentEntry.Translation = compareItem.TranslationRead;
                            } else {
                                // just for clarififaction
                                // keep current!
                            }
                            break;
                        case ImportModes.RightJoin:
                            // set all read; all read + existing that werent read
                            // preview leads!
                            // preview knows about Existing and Read!
                            // take care of method name 'is ... READ available'
                            if (compareItem.IsTranslationReadAvailable) {
                                // only set, if a translation is read
                                currentEntry.Translation = compareItem.TranslationRead;
                            } else {
                                // just for clarififaction
                                // keep current!
                            }
                            break;
                    }
                    continue;
                }
            }
        }
    }
}
