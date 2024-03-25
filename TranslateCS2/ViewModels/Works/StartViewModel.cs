using System;

using Markdig;
using Markdig.Wpf;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Helpers;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Services;

namespace TranslateCS2.ViewModels.Works;

internal class StartViewModel : BindableBase, INavigationAware {
    public TranslationSessionManager SessionManager { get; }
    public string Doc { get; }
    public MarkdownPipeline Pipeline { get; }
    public string? NVAString { get; }

    public StartViewModel(TranslationSessionManager translationSessionManager,
                          LatestVersionCheckService latestVersionCheckService) {
        this.SessionManager = translationSessionManager;
        bool newVersionAvailable = latestVersionCheckService.IsNewVersionAvailable();
        if (newVersionAvailable) {
            Version current = latestVersionCheckService.Current;
            Version latest = latestVersionCheckService.Latest;
            this.NVAString = $"New Version available: Current Version {current} - Latest Version {latest}";
        }
        this.Doc = MarkdownHelper.GetReadmeTillCaption("# Credits");
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
