using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Core.Services.Databases;
public interface ITranslationsDatabaseService {
    delegate void OnErrorCallBack(string message);
    void CreateIfNotExists();
    bool DatabaseExists();
    void BackUpIfExists(DatabaseBackUpIndicators backUpIndicator);
    void EnrichTranslationSessions(ITranslationSessionManager translationSessionManager, OnErrorCallBack? onError);
    void UpsertTranslationSession(ITranslationSession translationSession, OnErrorCallBack? onError);
    void EnrichSavedTranslations(ITranslationSession session, OnErrorCallBack? onError);
    void SaveTranslations(ITranslationSession translationSession, OnErrorCallBack? onError);
    void DeleteTranslationSession(ITranslationSession translationSession, OnErrorCallBack onError);
}
