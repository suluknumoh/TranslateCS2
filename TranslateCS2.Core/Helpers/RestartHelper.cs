using System.Diagnostics;
using System.Windows;

using TranslateCS2.Core.Configurations;

namespace TranslateCS2.Core.Helpers;
public static class RestartHelper {
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
