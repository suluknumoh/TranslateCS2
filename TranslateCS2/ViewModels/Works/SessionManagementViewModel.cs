using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Configurations;
using TranslateCS2.Controls.Sessions;
using TranslateCS2.Models.Sessions;

namespace TranslateCS2.ViewModels.Works;

internal class SessionManagementViewModel : BindableBase, INavigationAware {

    private readonly IRegionManager _regionManager;

    public DelegateCommand<string> CreateEditSessionCommand { get; }

    private string? _InstallPath;
    public string? InstallPath {
        get => this._InstallPath;
        private set => this.SetProperty(ref this._InstallPath, value);
    }


    public TranslationSessionManager SessionManager { get; }

    public SessionManagementViewModel(IRegionManager regionManager, TranslationSessionManager translationSessionManager) {
        this._regionManager = regionManager;
        this.SessionManager = translationSessionManager;
        this.InstallPath = this.SessionManager.InstallPath;
        this.CreateEditSessionCommand = new DelegateCommand<string>(this.CreateEditSessionCommandAction);
    }

    private void CreateEditSessionCommandAction(string action) {
        NavigationParameters parameters = [];
        // dont translate/localize CommandParameter
        if (action == "edit") {
            parameters.Add("edit", true);
        }
        string? regionName = AppConfigurationManager.AppNewEditSessionRegion;
        this._regionManager.RequestNavigate(regionName, nameof(NewEditSessionControl), parameters);
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
