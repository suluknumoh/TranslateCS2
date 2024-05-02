using System;
using System.Reflection;

using Markdig;
using Markdig.Wpf;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Services.LatestVersions;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.ViewModels;

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
        Assembly? assembly = Assembly.GetAssembly(typeof(StartViewModel));
        this.Doc = MarkDownHelper.GetMarkDown(assembly, $"{AppConfigurationManager.AssetPath}README.md");
        this.Pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
        this.RaisePropertyChanged(nameof(this.Doc));
        this.RaisePropertyChanged(nameof(this.Pipeline));
    }
}
