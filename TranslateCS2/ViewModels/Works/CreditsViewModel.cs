using Markdig;
using Markdig.Wpf;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Helpers;
using TranslateCS2.Models.Sessions;

namespace TranslateCS2.ViewModels.Works;

internal class CreditsViewModel : BindableBase, INavigationAware {
    public TranslationSessionManager SessionManager { get; }
    public string Doc { get; }
    public MarkdownPipeline Pipeline { get; }

    public CreditsViewModel(TranslationSessionManager translationSessionManager) {
        this.SessionManager = translationSessionManager;
        this.Doc = MarkdownHelper.GetReadmeFromCaption("# Credits");
        this.Pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        //
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        //
    }
}
