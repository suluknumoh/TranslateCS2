using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Inf;
using TranslateCS2.Inf.Models;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Core.Sessions;
public interface ITranslationSessionManager {
    MyLocalization<IAppLocFileEntry> BaseLocalizationFile { get; }
    IEnumerable<FileInfo> LocalizationFiles { get; }
    ObservableCollection<ITranslationSession> TranslationSessions { get; }
    ITranslationSession? CurrentTranslationSession { get; set; }
    string InstallPath { get; }
    bool HasTranslationSessions => this.TranslationSessions.Any();
    bool IsAppUseAble { get; }
    string? DatabaseError { get; set; }
    bool HasDatabaseError => !this.HasNoDatabaseError;
    bool HasNoDatabaseError => StringHelper.IsNullOrWhiteSpaceOrEmpty(this.DatabaseError);
    void Insert(ITranslationSession? session);
    void Update(ITranslationSession? session);
    void UpdateCurrentWith(ITranslationSession? session);
    void Delete(ITranslationSession? session);
    void SaveCurrentTranslationSessionsTranslations();
    void CurrentTranslationSessionChanged();
    ITranslationSession GetNewTranslationSession();
    bool ExistsKeyInCurrentTranslationSession(string key);
    IndexCountHelperValidationResult IsIndexKeyValid(string key, string? keyOrigin);
    ITranslationSession? CloneCurrent();
}
