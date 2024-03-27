using Prism.Mvvm;
using Prism.Regions;

namespace TranslateCS2.Controls.Imports;

internal class ImportControlContext : BindableBase, INavigationAware {
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
