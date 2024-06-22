using System;
using System.Windows;

using Microsoft.Win32;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Inf;

namespace TranslateCS2.Core.Services.InstallPaths;
public class ManualPathSelector {
    private readonly string dialogWarningCaption = I18NGlobal.ManualPathSelectorNotOkCaption;
    private readonly string dialogWarningText = String.Format(I18NGlobal.ManualPathSelectorNotOkText, AppConfigurationManager.CitiesExe);
    private readonly OpenFileDialog dialog;
    public ManualPathSelector() {
        string title = String.Format(I18NGlobal.ManualPathSelectorSelectTitle, AppConfigurationManager.CitiesExe);
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

    public void DisplayInformation(Window owner) {
        string caption = String.Format(I18NGlobal.ManualPathSelectorInformationCaption, AppConfigurationManager.CitiesExe);
        string text = String.Format(I18NGlobal.ManualPathSelectorInformationText, AppConfigurationManager.CitiesExe);
        MessageBox.Show(owner,
                        text,
                        caption,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information,
                        MessageBoxResult.None,
                        MessageBoxOptions.None);
    }
}
