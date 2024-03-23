using Prism.Mvvm;
using Prism.Regions;

namespace TranslateCS2.ViewModels.Works;

internal class SettingsViewModel : BindableBase, INavigationAware {
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
