using System;
using System.Windows;

using Microsoft.Win32;

using TranslateCS2.Configurations;
using TranslateCS2.Properties;

namespace TranslateCS2.Helpers;

internal static class ImExportDialogHelper {
    public static string? ShowSaveFileDialog(string? path) {
        SaveFileDialog dialog = new SaveFileDialog {
            Title = I18N.StringExportDialogTitle,
            CheckPathExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = AppConfigurationManager.ImExportDefaultFileName,
            Filter = AppConfigurationManager.ImExportFilter,
            ValidateNames = true,
            DereferenceLinks = false
        };
        if (path is not null) {
            dialog.InitialDirectory = path;
        }
        return Display(dialog);
    }
    public static string? ShowOpenFileDialog(string? path) {
        OpenFileDialog dialog = new OpenFileDialog {
            Title = I18N.StringImportDialogTitle,
            Multiselect = false,
            CheckPathExists = true,
            CheckFileExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = AppConfigurationManager.ImExportDefaultFileName,
            Filter = AppConfigurationManager.ImExportFilter,
            ValidateNames = true,
            DereferenceLinks = false
        };
        if (path is not null) {
            dialog.InitialDirectory = path;
        }
        return Display(dialog);
    }

    private static string? Display(FileDialog dialog) {
        bool ok;
        do {
            ok = dialog.ShowDialog() ?? false;
            if (!ok) {
                // cancel
                break;
            }
            ok = dialog.FileName.EndsWith(AppConfigurationManager.ImExportFileExtension);
            if (!ok) {
                MessageBox.Show(I18N.ImExportWarningJSON, I18N.StringWarningCap, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.None);
            }
        } while (!ok);
        if (ok) {
            return dialog.FileName;
        }
        return null;
    }
}
