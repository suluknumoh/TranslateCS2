using System.Collections.Generic;

using TranslateCS2.Models;
using TranslateCS2.Models.Exports;
using TranslateCS2.Models.Sessions;

namespace TranslateCS2.Services;
internal class ExportService {
    private readonly LocalizationFilesService _localizationFilesService;
    private readonly TranslationSessionManager _translationSessionManager;

    public ExportService(LocalizationFilesService localizationFilesService, TranslationSessionManager translationSessionManager) {
        this._localizationFilesService = localizationFilesService;
        this._translationSessionManager = translationSessionManager;
    }

    public List<ExportFormat> GetExportFormats() {
        List<ExportFormat> exportFormats = [];
        // TODO: I18N - see also switch-case in Export-Method!
        {
            ExportFormat exportFormat = new ExportFormat("direct-overwrite",
                                                         ExportFormats.Direct,
                                                         false,
                                                         "replace existing localization based on session-properties");
            exportFormats.Add(exportFormat);
        }
        {
            ExportFormat exportFormat = new ExportFormat("json",
                                                         ExportFormats.JSON,
                                                         true,
                                                         "export as json to share with others");
            exportFormats.Add(exportFormat);
        }
        return exportFormats;
    }

    public void Export(ExportFormat exportFormat, LocalizationFile localizationFile, string? file) {
        switch (exportFormat.Format) {
            case ExportFormats.Direct:
                this._localizationFilesService.WriteLocalizationFileDirect(localizationFile);
                break;
            case ExportFormats.JSON:
                this._localizationFilesService.WriteLocalizationFileJson(localizationFile, file);
                break;
        }
    }
}
