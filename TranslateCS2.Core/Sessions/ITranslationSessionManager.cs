using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using TranslateCS2.Core.Helpers;

namespace TranslateCS2.Core.Sessions;
public interface ITranslationSessionManager {
    ILocalizationFile BaseLocalizationFile { get; }
    IEnumerable<FileInfo> LocalizationFiles { get; }
    ObservableCollection<ITranslationSession> TranslationSessions { get; }
    ITranslationSession? CurrentTranslationSession { get; set; }
    string InstallPath { get; }
    bool HasTranslationSessions => this.TranslationSessions.Any();
    bool IsAppUseAble { get; }
    string? DatabaseError { get; set; }
    bool HasDatabaseError => !this.HasNoDatabaseError;
    bool HasNoDatabaseError => StringHelper.IsNullOrWhiteSpaceOrEmpty(this.DatabaseError);
    ILocalizationFile GetForExport();
    void Insert(ITranslationSession? session);
    void Update(ITranslationSession? session);
    void Delete(ITranslationSession? session);
    void SaveCurrentTranslationSessionsTranslations();
    void CurrentTranslationSessionChanged();
    ITranslationSession GetNewTranslationSession();
    bool ExistsKeyInCurrentTranslationSession(string key);
}
