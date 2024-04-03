using System;

using Markdig;
using Markdig.Wpf;

using TranslateCS2.Helpers;
using TranslateCS2.Models;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Services;

namespace TranslateCS2.ViewModels.Works;

internal class StartViewModel : ABaseViewModel {
    private readonly LatestVersionCheckService _latestVersionCheckService;
    public TranslationSessionManager SessionManager { get; }
    public string? Doc { get; private set; }
    public MarkdownPipeline? Pipeline { get; private set; }
    public string? NVAString { get; private set; }

    public StartViewModel(TranslationSessionManager translationSessionManager,
                          LatestVersionCheckService latestVersionCheckService) {
        this.SessionManager = translationSessionManager;
        this._latestVersionCheckService = latestVersionCheckService;
    }

    protected override void OnLoadedCommandAction() {
        bool newVersionAvailable = this._latestVersionCheckService.IsNewVersionAvailable();
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
