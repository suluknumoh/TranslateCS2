using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Services.LocalizationFiles;
using TranslateCS2.Core.Sessions;
using TranslateCS2.ExImport.Models;
using TranslateCS2.ExImport.Properties.I18N;
using TranslateCS2.Inf;

namespace TranslateCS2.ExImport.Services;
internal class ExImportService {
    private readonly IAppLocaFileService localizationFilesService;
    private readonly JSONService jsonService;

    public ExImportService(IAppLocaFileService localizationFilesService,
                           JSONService jsonService) {
        this.localizationFilesService = localizationFilesService;
        this.jsonService = jsonService;
    }

    public List<ExportFormat> GetExportFormats() {
        List<ExportFormat> exportFormats = [];
        {
            ExportFormat exportFormat = new ExportFormat(nameof(ExportFormats.JSON),
                                                         ExportFormats.JSON,
                                                         true,
                                                         I18NExport.ToolTipExportFormatJSON);
            exportFormats.Add(exportFormat);
        }
        return exportFormats;
    }

    public async Task Export(ExportFormat exportFormat,
                             AppLocFile localizationFile,
                             bool addKey,
                             bool addMergeValues,
                             string? file) {
        switch (exportFormat.Format) {
            case ExportFormats.JSON:
                await this.jsonService.WriteLocalizationFileJson(localizationFile, file, addKey, addMergeValues);
                break;
        }
    }

    public async Task<List<CompareExistingReadTranslation>> ReadToReview(ITranslationSession translationSession,
                                                                         string selectedPath) {
        List<AppLocFileEntry>? imports = await this.jsonService.ReadLocalizationFileJson(selectedPath);
        ArgumentNullException.ThrowIfNull(imports);
        List<CompareExistingReadTranslation> preview = [];
        {
            // read keys used by colossal order
            foreach (KeyValuePair<string, AppLocFileEntry> existing in translationSession.Localizations) {
                IEnumerable<AppLocFileEntry> importsForKey = imports.Where(item => item.Key.Key == existing.Key);
                string key = existing.Key;
                string? translationExisting = existing.Value.Translation;
                string? translationRead = null;
                if (importsForKey.Any()) {
                    translationRead = StringHelper.GetNullForEmpty(importsForKey.First().Translation);
                }
                if (StringHelper.IsNullOrWhiteSpaceOrEmpty(translationRead)
                    && StringHelper.IsNullOrWhiteSpaceOrEmpty(translationExisting)) {
                    continue;
                }
                CompareExistingReadTranslation previewEntry = new CompareExistingReadTranslation(key,
                                                                                                 translationExisting,
                                                                                                 translationRead);
                preview.Add(previewEntry);
            }
        }
        {
            // read entries other than those used by colossal order (key-value-pairs for mods)
            IEnumerable<AppLocFileEntry> remaining =
                imports
                    .Where(import => !translationSession.Localizations.Select(existing => existing.Key).Contains(import.Key.Key));
            if (remaining.Any()) {
                foreach (AppLocFileEntry remain in remaining) {
                    string? key = StringHelper.GetNullForEmpty(remain.Key.Key);
                    string? translationExisting = null;
                    string? translationRead = StringHelper.GetNullForEmpty(remain.Translation);
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(key)
                        && StringHelper.IsNullOrWhiteSpaceOrEmpty(translationRead)) {
                        continue;
                    }
                    CompareExistingReadTranslation previewEntry = new CompareExistingReadTranslation(key,
                                                                                                     translationExisting,
                                                                                                     translationRead);
                    preview.Add(previewEntry);
                }
            }
        }
        return preview;
    }

    public void HandleRead(IList<CompareExistingReadTranslation> preview,
                           ITranslationSession translationSession,
                           ImportModes importMode) {
        IList<KeyValuePair<string, AppLocFileEntry>> localizationDictionary = translationSession.Localizations;
        {
            // handle key-value-pairs used by colossal order
            foreach (KeyValuePair<string, AppLocFileEntry> currentEntry in localizationDictionary) {
                foreach (CompareExistingReadTranslation compareItem in preview) {
                    if (compareItem.Key == currentEntry.Key) {
                        switch (importMode) {
                            case ImportModes.NEW:
                                // set all read
                                currentEntry.Value.Translation = compareItem.TranslationRead;
                                break;
                            case ImportModes.LeftJoin:
                                // set missing read; all existing + read that are missing
                                // preview leads!
                                // preview knows about Existing and Read!
                                // take care of method name 'is ... EXISTING available'
                                if (!compareItem.IsTranslationExistingAvailable) {
                                    // only set if no translation existed
                                    currentEntry.Value.Translation = compareItem.TranslationRead;
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
                                    currentEntry.Value.Translation = compareItem.TranslationRead;
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
        {
            // handle other key-value-pairs that are not used by colossal order (key-value-pairs for mods)
            IEnumerable<CompareExistingReadTranslation> remaining =
                preview
                    .Where(import => !localizationDictionary
                    .Select(existing => existing.Key).Contains(import.Key));
            foreach (CompareExistingReadTranslation remain in remaining) {
                switch (importMode) {
                    case ImportModes.NEW:
                    case ImportModes.RightJoin:
                    case ImportModes.LeftJoin:
                        // remaining do not exist within the given localizationDictionary
                        // they should be added always (i think)
                        AppLocFileEntry newItem = new AppLocFileEntry(remain.Key,
                                                                      null,
                                                                      null,
                                                                      remain.TranslationRead,
                                                                      true);
                        localizationDictionary.Add(new KeyValuePair<string, AppLocFileEntry>(newItem.Key.Key, newItem));
                        break;
                }
            }
        }
        // TODO:
        //IndexCountHelper.AutoCorrect(localizationDictionary);
    }
}
