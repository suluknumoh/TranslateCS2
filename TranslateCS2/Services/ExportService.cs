using System.Collections.Generic;

using TranslateCS2.Models;
using TranslateCS2.Models.Exports;
using TranslateCS2.Models.LocDictionary;

namespace TranslateCS2.Services;
internal class ExportService {
    private readonly LocalizationFilesService _localizationFilesService;

    public ExportService(LocalizationFilesService localizationFilesService) {
        this._localizationFilesService = localizationFilesService;
    }

    public List<ExportFormat> GetExportFormats() {
        List<ExportFormat> exportFormats = [];
        // TODO: I18N - see also switch-case in Export-Method!
        exportFormats.Add(new ExportFormat("direct-overwrite", false, "replace existing localization based on session-properties"));
        // TODO: exportformats
        return exportFormats;
    }

    public void Export(ExportFormat exportFormat, LocalizationFile<LocalizationDictionaryExportEntry> localizationFile) {
        // TODO: export itself
        switch (exportFormat.Name) {
            case "direct-overwrite":
                this._localizationFilesService.WriteLocalizationFile(localizationFile);
                break;
        }
    }
}
