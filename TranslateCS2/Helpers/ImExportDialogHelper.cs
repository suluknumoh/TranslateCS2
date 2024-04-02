using System;
using System.Windows;

using Microsoft.Win32;

using TranslateCS2.Configurations;

namespace TranslateCS2.Helpers;

internal static class ImExportDialogHelper {
    public static string? ShowSaveFileDialog(string? path,
                                             string title,
                                             string dialogWarningCaption,
                                             string dialogWarningText) {
        SaveFileDialog dialog = new SaveFileDialog {
            Title = title,
            CheckPathExists = true,
            RestoreDirectory = true,
            FileName = AppConfigurationManager.ImExportDefaultFileName,
            Filter = AppConfigurationManager.ImExportFilter,
            ValidateNames = true,
            DereferenceLinks = false
        };
        if (path is null) {
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
        return Display(dialog, dialogWarningCaption, dialogWarningText);
    }
    public static string? ShowOpenFileDialog(string? path,
                                             string title,
                                             string dialogWarningCaption,
                                             string dialogWarningText) {
        OpenFileDialog dialog = new OpenFileDialog {
            Title = title,
            Multiselect = false,
            CheckPathExists = true,
            CheckFileExists = true,
            RestoreDirectory = true,
            FileName = AppConfigurationManager.ImExportDefaultFileName,
            Filter = AppConfigurationManager.ImExportFilter,
            ValidateNames = true,
            DereferenceLinks = false
        };
        if (path is null) {
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
        return Display(dialog, dialogWarningCaption, dialogWarningText);
    }

    private static string? Display(FileDialog dialog, string dialogWarningCaption, string dialogWarningText) {
        bool ok;
        do {
            ok = dialog.ShowDialog() ?? false;
            if (!ok) {
                // cancel
                break;
            }
            ok = dialog.FileName.EndsWith(AppConfigurationManager.ImExportFileExtension);
            if (!ok) {
                MessageBox.Show(dialogWarningText,
                                dialogWarningCaption,
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None);
            }
        } while (!ok);
        if (ok) {
            return dialog.FileName;
        }
        return null;
    }
}
