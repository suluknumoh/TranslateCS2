using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Core.Services.LocalizationFiles;
public interface ILocalizationFilesService {
    IEnumerable<FileInfo> GetLocalizationFiles();
    LocalizationFile GetLocalizationFile(FileInfo fileInfo);
    Task WriteLocalizationFileDirect(ILocalizationFile localizationFile,
                                     Stream? streamParameter = null);
}
