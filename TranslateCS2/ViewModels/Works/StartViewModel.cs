using System;

using Markdig;
using Markdig.Wpf;

using TranslateCS2.Core.Services.LatestVersions;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.ViewModels;
using TranslateCS2.Helpers;

namespace TranslateCS2.ViewModels.Works;

internal class StartViewModel : ABaseViewModel {
    private readonly ILatestVersionCheckService _latestVersionCheckService;
    public ITranslationSessionManager SessionManager { get; }
    public string? Doc { get; private set; }
    public MarkdownPipeline? Pipeline { get; private set; }
    public string? NVAString { get; private set; }

    public StartViewModel(ITranslationSessionManager translationSessionManager,
                          ILatestVersionCheckService latestVersionCheckService) {
        this.SessionManager = translationSessionManager;
        this._latestVersionCheckService = latestVersionCheckService;
    }

    protected override async void OnLoadedCommandAction() {
        bool newVersionAvailable = await this._latestVersionCheckService.IsNewVersionAvailable();
        if (newVersionAvailable) {
            Version current = this._latestVersionCheckService.Current;
            Version latest = this._latestVersionCheckService.Latest;
            this.NVAString = $"New Version available: Current Version {current} - Latest Version {latest}";
            this.RaisePropertyChanged(nameof(this.NVAString));
        }
        this.Doc = MarkdownHelper.GetReadmeTillCaption("# Credits");
        this.Pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
        this.RaisePropertyChanged(nameof(this.Doc));
        this.RaisePropertyChanged(nameof(this.Pipeline));
    }
}
