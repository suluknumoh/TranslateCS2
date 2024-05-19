using System.Collections.Generic;
using System.IO;

using TranslateCS2.Core.Models.Localizations;

namespace TranslateCS2.Core.Services.LocalizationFiles;
public interface IAppLocaFileService {
    IEnumerable<FileInfo> GetLocalizationFiles();
    AppLocFile GetLocalizationFile(FileInfo fileInfo);
}
