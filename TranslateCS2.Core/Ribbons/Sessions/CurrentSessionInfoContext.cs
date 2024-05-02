using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Core.Ribbons.Sessions;
internal class CurrentSessionInfoContext {
    public ITranslationSessionManager SessionManager { get; }
    public CurrentSessionInfoContext(ITranslationSessionManager sessionManager) {
        this.SessionManager = sessionManager;
    }
}
