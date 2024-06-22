using System.Diagnostics;
using System.Windows;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Properties.I18N;

namespace TranslateCS2.Core.Helpers;
public static class RestartHelper {
    public static void DisplayError(Window owner) {
        string caption = I18NGlobal.RestartHelperErrorCaption;
        string text = I18NGlobal.RestartHelperErrorText;
        MessageBox.Show(owner,
                        text,
                        caption,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error,
                        MessageBoxResult.None,
                        MessageBoxOptions.None);
    }

    public static bool Restart() {
        Process process = Process.GetCurrentProcess();
        ProcessModule? mainModule = process.MainModule;
        if (mainModule is not null) {
            Process? newProcess = Process.Start(mainModule.FileName);
            if (newProcess is not null
                && newProcess.WaitForInputIdle(AppConfigurationManager.MaxWaitOnRestart)
                && newProcess.Responding) {
                Application.Current.Shutdown();
                return true;
            }
        }
        return false;
    }
}
