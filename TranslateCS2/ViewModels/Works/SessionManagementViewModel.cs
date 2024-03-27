using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Configurations;
using TranslateCS2.Controls.Sessions;
using TranslateCS2.Models.Sessions;

namespace TranslateCS2.ViewModels.Works;

internal class SessionManagementViewModel : BindableBase, INavigationAware {

    private readonly IRegionManager _regionManager;

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


    public TranslationSessionManager SessionManager { get; }

    public SessionManagementViewModel(IRegionManager regionManager, TranslationSessionManager translationSessionManager) {
        this._regionManager = regionManager;
        this.SessionManager = translationSessionManager;
        this.IsEditEnabled = this.SessionManager.HasTranslationSessions;
        this.InstallPath = this.SessionManager.InstallPath;
        this.CreateEditSessionCommand = new DelegateCommand<SessionActions?>(this.CreateEditSessionCommandAction);
    }

    private void CreateEditSessionCommandAction(SessionActions? action) {
        this.SessionAction = action;
        NavigationParameters parameters = [];
        // dont translate/localize parameter key's
        parameters.Add(nameof(CallBacks.CallBackAfter), this.CallbackAfter);
        parameters.Add(nameof(SessionActions), this.SessionAction);
        string? regionName = AppConfigurationManager.AppNewEditSessionRegion;
        this.IsEnabled = false;
        this.IsEditEnabled = false;
        this._regionManager.RequestNavigate(regionName, nameof(NewEditSessionControl), parameters);
    }

    private void CallbackAfter() {
        this.IsEnabled = true;
        this.IsEditEnabled = this.SessionManager.HasTranslationSessions;
        this.SessionAction = null;
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
