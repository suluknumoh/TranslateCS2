using System;
using System.Windows;

using Microsoft.Win32;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Inf;

namespace TranslateCS2.Core.Services.InstallPaths;
public class ManualPathSelector {
    private readonly string dialogWarningCaption;
    private readonly string dialogWarningText;
    private readonly OpenFileDialog dialog;
    public ManualPathSelector(string title,
                              string dialogWarningCaption,
                              string dialogWarningText) {
        this.dialogWarningCaption = dialogWarningCaption;
        this.dialogWarningText = dialogWarningText;
        this.dialog = new OpenFileDialog {
            Title = title,
            Multiselect = false,
            CheckPathExists = true,
            CheckFileExists = true,
            RestoreDirectory = true,
            FileName = AppConfigurationManager.CitiesExe,
            Filter = AppConfigurationManager.CitiesExeFilter,
            DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            DefaultExt = AppConfigurationManager.ExeExtension,
            ValidateNames = true,
            DereferenceLinks = true
        };
    }

    public string? Display(Window owner) {
        bool ok;
        do {
            ok = this.dialog.ShowDialog(owner) ?? false;
            if (!ok) {
                // cancel
                break;
            }
            ok = this.dialog.FileName.EndsWith(AppConfigurationManager.CitiesExe, StringComparison.OrdinalIgnoreCase);
            if (!ok) {
                MessageBox.Show(this.dialogWarningText,
                                this.dialogWarningCaption,
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None);
            }
        } while (!ok);
        if (ok) {
            string directoryPath = this.dialog.FileName.Replace(AppConfigurationManager.CitiesExe, String.Empty);
            return Paths.NormalizeUnix(directoryPath);
        }
        return null;
    }
}
