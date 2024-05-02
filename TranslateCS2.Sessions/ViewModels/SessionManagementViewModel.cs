using System.Windows;

using Prism.Commands;
using Prism.Regions;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.ViewModels;
using TranslateCS2.Sessions.Controls;
using TranslateCS2.Sessions.Models;
using TranslateCS2.Sessions.Properties.I18N;

using static TranslateCS2.Sessions.Controls.NewEditSessionControlContext;

namespace TranslateCS2.Sessions.ViewModels;

internal class SessionManagementViewModel : ABaseViewModel {

    private readonly IRegionManager _regionManager;
    private readonly IViewConfigurations _viewConfigurations;
    public DelegateCommand DeleteSessionCommand { get; }

    public DelegateCommand<SessionActions?> CreateEditSessionCommand { get; }


    private bool _IsEnabled = true;
    public bool IsEnabled {
        get => this._IsEnabled;
        set => this.SetProperty(ref this._IsEnabled, value);
    }

    private bool _IsEditEnabled;
    public bool IsEditEnabled {
        get => this._IsEditEnabled;
        set => this.SetProperty(ref this._IsEditEnabled, value);
    }


    private string? _InstallPath;
    public string? InstallPath {
        get => this._InstallPath;
        private set => this.SetProperty(ref this._InstallPath, value);
    }


    private SessionActions? _SessionAction;
    public SessionActions? SessionAction {
        get => this._SessionAction;
        set => this.SetProperty(ref this._SessionAction, value);
    }


    public ITranslationSessionManager SessionManager { get; }

    public SessionManagementViewModel(IRegionManager regionManager,
                                      ITranslationSessionManager translationSessionManager,
                                      IViewConfigurations viewConfigurations) {
        this._regionManager = regionManager;
        this.SessionManager = translationSessionManager;
        this._viewConfigurations = viewConfigurations;
        this.CreateEditSessionCommand = new DelegateCommand<SessionActions?>(this.CreateEditSessionCommandAction);
        this.DeleteSessionCommand = new DelegateCommand(this.DeleteSessionCommandAction);
    }

    private void CreateEditSessionCommandAction(SessionActions? action) {
        this.SessionAction = action;
        NavigationParameters parameters = [];
        // dont translate/localize parameter key's
        CallBackAfter @delegate = this.CallbackAfter;
        parameters.Add(nameof(CallBackAfter), @delegate);
        parameters.Add(nameof(SessionActions), this.SessionAction);
        string? regionName = AppConfigurationManager.AppNewEditSessionRegion;
        this.IsEnabled = false;
        this.IsEditEnabled = false;
        this._viewConfigurations.DeActivateRibbon?.Invoke(false);
        this._regionManager.RequestNavigate(regionName, nameof(NewEditSessionControl), parameters);
    }

    private void CallbackAfter() {
        this.IsEnabled = true;
        this.IsEditEnabled = this.SessionManager.HasTranslationSessions;
        this.SessionAction = null;
        this._viewConfigurations.DeActivateRibbon?.Invoke(true);
    }
    private void DeleteSessionCommandAction() {
        MessageBoxResult result = MessageBox.Show(I18NSessions.DialogDeleteText,
                                                  I18NSessions.DialogDeleteTitle,
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Warning,
                                                  MessageBoxResult.No,
                                                  MessageBoxOptions.None);
        if (result == MessageBoxResult.Yes) {
            this.SessionManager.Delete(this.SessionManager.CurrentTranslationSession);
            this.IsEditEnabled = this.SessionManager.HasTranslationSessions;
        }
    }

    protected override void OnLoadedCommandAction() {
        this.IsEditEnabled = this.SessionManager.HasTranslationSessions;
        this.InstallPath = this.SessionManager.InstallPath;
    }
}
