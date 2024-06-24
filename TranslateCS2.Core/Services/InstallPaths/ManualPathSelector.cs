using System;
using System.IO;
using System.Windows;

using Microsoft.Win32;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Inf;

namespace TranslateCS2.Core.Services.InstallPaths;
public class ManualPathSelector {
    private readonly string dialogWarningCaption = I18NGlobal.ManualPathSelectorNotOkCaption;
    private readonly string dialogWarningText = String.Format(I18NGlobal.ManualPathSelectorNotOkText, AppConfigurationManager.Cities2_Data);
    private readonly OpenFolderDialog dialog;
    public ManualPathSelector() {
        string title = String.Format(I18NGlobal.ManualPathSelectorSelectTitle, AppConfigurationManager.Cities2_Data);
        this.dialog = new OpenFolderDialog {
            Title = title,
            Multiselect = false,
            DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            ValidateNames = true,
            DereferenceLinks = true
        };
    }

    public string? Display(Window owner) {
        while (true) {
            bool ok = this.dialog.ShowDialog(owner) ?? false;
            if (!ok) {
                // cancel
                return null;
            }
            //
            string? directoryPath = null;
            if (this.IsSelectionOK(ref directoryPath)
                && directoryPath != null) {
                return Paths.NormalizeUnix(directoryPath);
            }
            this.ShowWarningWrongDirectory();
        }
    }

    private bool IsSelectionOK(ref string? directoryPath) {
        if (!AppConfigurationManager.Cities2_Data.Equals(this.dialog.SafeFolderName, StringComparison.OrdinalIgnoreCase)) {
            // selected directory is NOT Cities2_Data
            return false;
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(this.dialog.FolderName);
        DirectoryInfo? parent = directoryInfo.Parent;
        if (parent is null) {
            // has NO parent
            return false;
        }
        string citiesExePath = Path.Combine(parent.FullName, AppConfigurationManager.CitiesExe);
        if (!File.Exists(citiesExePath)) {
            // Cities2.exe NOT exists
            return false;
        }

        directoryPath = parent.FullName;
        return true;
    }

    private void ShowWarningWrongDirectory() {
        MessageBox.Show(this.dialogWarningText,
                        this.dialogWarningCaption,
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None);
    }

    public void DisplayInformation(Window owner) {
        string caption = String.Format(I18NGlobal.ManualPathSelectorInformationCaption, AppConfigurationManager.Cities2_Data);
        string text = String.Format(I18NGlobal.ManualPathSelectorInformationText, AppConfigurationManager.Cities2_Data);
        MessageBox.Show(owner,
                        text,
                        caption,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information,
                        MessageBoxResult.None,
                        MessageBoxOptions.None);
    }
}
