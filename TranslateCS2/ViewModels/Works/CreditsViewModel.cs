using Markdig;
using Markdig.Wpf;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Helpers;

namespace TranslateCS2.ViewModels.Works;

internal class CreditsViewModel : BindableBase, INavigationAware {
    public string Doc { get; }
    public MarkdownPipeline Pipeline { get; }

    public CreditsViewModel() {
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
