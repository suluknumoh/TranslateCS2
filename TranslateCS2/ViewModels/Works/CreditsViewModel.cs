using System.Reflection;

using Markdig;
using Markdig.Wpf;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.ViewModels;

namespace TranslateCS2.ViewModels.Works;

internal class CreditsViewModel : ABaseViewModel {
    public ITranslationSessionManager SessionManager { get; }
    public string? Doc { get; private set; }
    public MarkdownPipeline? Pipeline { get; private set; }

    public CreditsViewModel(ITranslationSessionManager translationSessionManager) {
        this.SessionManager = translationSessionManager;
    }

    protected override void OnLoadedCommandAction() {
        Assembly? assembly = Assembly.GetAssembly(typeof(CreditsViewModel));
        this.Doc = MarkDownHelper.GetMarkDown(assembly, $"{AppConfigurationManager.AssetPath}CREDITS.md");
        this.Pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
        this.RaisePropertyChanged(nameof(this.Doc));
        this.RaisePropertyChanged(nameof(this.Pipeline));
    }
}
