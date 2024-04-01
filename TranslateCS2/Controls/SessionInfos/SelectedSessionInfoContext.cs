using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Models.Sessions;

namespace TranslateCS2.Controls.SessionInfos;
internal class SelectedSessionInfoContext : BindableBase, INavigationAware {
    public TranslationSessionManager SessionManager { get; }


    private bool _Display;
    public bool Display {
        get => this._Display;
        set => this.SetProperty(ref this._Display, value);
    }


    public DelegateCommand DisplayCommand { get; }


    public SelectedSessionInfoContext(TranslationSessionManager translationSessionManager) {
        this._Display = true;
        this.SessionManager = translationSessionManager;
        this.DisplayCommand = new DelegateCommand(this.DisplayCommandAction);
    }

    private void DisplayCommandAction() {
        this.Display = !this.Display;
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        //
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        //
    }
}
