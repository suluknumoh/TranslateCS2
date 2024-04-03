using System.Windows;

using Prism.Commands;
using Prism.Regions;

using TranslateCS2.Configurations;
using TranslateCS2.Controls.Sessions;
using TranslateCS2.Models;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties.I18N;

using static TranslateCS2.Controls.Sessions.NewEditSessionControlContext;

namespace TranslateCS2.ViewModels.Works;

internal class SessionManagementViewModel : ABaseViewModel {

    private readonly IRegionManager _regionManager;
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


    public TranslationSessionManager SessionManager { get; }

    public SessionManagementViewModel(IRegionManager regionManager,
                                      TranslationSessionManager translationSessionManager) {
        this._regionManager = regionManager;
        this.SessionManager = translationSessionManager;
        this.CreateEditSessionCommand = new DelegateCommand<SessionActions?>(this.CreateEditSessionCommandAction);
        this.DeleteSessionCommand = new DelegateCommand(this.DeleteSessionCommandAction);
    }

    private void CreateEditSessionCommandAction(SessionActions? action) {
        this.SessionAction = action;
        NavigationParameters parameters = [];
        // dont translate/localize parameter key's
        CallBackAfter @delegate = this.CallbackAfter;
        parameters.Add(nameof(NewEditSessionControlContext.CallBackAfter), @delegate);
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
