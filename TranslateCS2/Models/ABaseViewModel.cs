using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace TranslateCS2.Models;
internal abstract class ABaseViewModel : BindableBase, INavigationAware {
    public DelegateCommand OnLoadedCommand { get; }
    protected ABaseViewModel() {
        this.OnLoadedCommand = new DelegateCommand(this.OnLoadedCommandAction);
    }
    protected abstract void OnLoadedCommandAction();
    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }
    public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

    public virtual void OnNavigatedTo(NavigationContext navigationContext) { }
}
