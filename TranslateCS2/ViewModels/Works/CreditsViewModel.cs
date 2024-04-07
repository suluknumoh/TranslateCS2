using Markdig;
using Markdig.Wpf;

using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.ViewModels;
using TranslateCS2.Helpers;

namespace TranslateCS2.ViewModels.Works;

internal class CreditsViewModel : ABaseViewModel {
    public ITranslationSessionManager SessionManager { get; }
    public string? Doc { get; private set; }
    public MarkdownPipeline? Pipeline { get; private set; }

    public CreditsViewModel(ITranslationSessionManager translationSessionManager) {
        this.SessionManager = translationSessionManager;
    }

    protected override void OnLoadedCommandAction() {
        this.Doc = MarkdownHelper.GetReadmeFromCaption("# Credits");
        this.Pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
        this.RaisePropertyChanged(nameof(this.Doc));
        this.RaisePropertyChanged(nameof(this.Pipeline));
    }
}
