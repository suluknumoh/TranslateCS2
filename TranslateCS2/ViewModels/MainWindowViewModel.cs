using System.ComponentModel;
using System.Windows;

using Prism.Commands;

using TranslateCS2.Core.Brokers;
using TranslateCS2.Core.Configurations.CitiesLocations;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Services.InstallPaths;

namespace TranslateCS2.ViewModels;
internal class MainWindowViewModel {
    private readonly IAppCloseBrokers appCloseBrokers;
    public DelegateCommand<RoutedEventArgs> WindowLoadedCommand { get; }
    public DelegateCommand<CancelEventArgs> WindowClosingCommand { get; }
    public MainWindowViewModel(IAppCloseBrokers appCloseBrokers) {
        this.appCloseBrokers = appCloseBrokers;
        this.WindowLoadedCommand = new DelegateCommand<RoutedEventArgs>(this.WindowLoadedCommandAction);
        this.WindowClosingCommand = new DelegateCommand<CancelEventArgs>(this.WindowClosingCommandAction);
    }

    private void WindowClosingCommandAction(CancelEventArgs args) {
        if (this.appCloseBrokers is null) {
            return;
        }
        foreach (IAppCloseBroker appCloseBroker in this.appCloseBrokers.Items) {
            if (appCloseBroker.IsCancelClose) {
                args.Cancel = true;
                // INFO: MessageBox if there is no autosave - but autosave is default
                return;
            }
        }
    }

    private void WindowLoadedCommandAction(RoutedEventArgs args) {
        // TODO: also via StartUpParameter?
        //if (Application.Current.Resources[] is string ) {
        // read startup parameters if needed
        //}
        if (args.Source is Window window) {
            this.DetectInstallPath(window);
        }
    }
    private void DetectInstallPath(Window owner) {
        IInstallPathDetector installPathDetector = IInstallPathDetector.Instance;
        bool detected = installPathDetector.Detect();
        if (!detected) {
            // TODO: MessageBox that informs about failure to detect etc.
            // TODO: title, caption and text
            ManualPathSelector manualPathSelector = new ManualPathSelector("a", "b", "c");
            string? path = manualPathSelector.Display(owner);
            if (path is not null) {
                CitiesLocationsSection.AddLocation(path);
                bool restarted = RestartHelper.Restart();
                if (!restarted) {
                    // TODO: show error
                }
            }
        }
    }
}
