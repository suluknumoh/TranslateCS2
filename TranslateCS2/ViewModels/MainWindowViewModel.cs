using System.ComponentModel;
using System.Windows;

using Prism.Commands;

using TranslateCS2.Core.Brokers;

namespace TranslateCS2.ViewModels;
internal class MainWindowViewModel {
    private readonly IAppCloseBrokers _appCloseBrokers;
    public DelegateCommand<RoutedEventArgs> WindowLoadedCommand { get; }
    public DelegateCommand<CancelEventArgs> WindowClosingCommand { get; }
    public MainWindowViewModel(IAppCloseBrokers appCloseBrokers) {
        this._appCloseBrokers = appCloseBrokers;
        this.WindowLoadedCommand = new DelegateCommand<RoutedEventArgs>(this.WindowLoadedCommandAction);
        this.WindowClosingCommand = new DelegateCommand<CancelEventArgs>(this.WindowClosingCommandAction);
    }

    private void WindowClosingCommandAction(CancelEventArgs args) {
        if (this._appCloseBrokers == null) {
            return;
        }
        foreach (IAppCloseBroker appCloseBroker in this._appCloseBrokers.Items) {
            if (appCloseBroker.IsCancelClose) {
                args.Cancel = true;
                // INFO: MessageBox if there is no autosave - but autosave is default
                return;
            }
        }
    }

    private void WindowLoadedCommandAction(RoutedEventArgs args) {
        //if (Application.Current.Resources[] is string ) {
        // read startup parameters if needed
        //}
    }
}
