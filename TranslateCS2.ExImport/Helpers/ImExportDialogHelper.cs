using System;
using System.Windows;

using Microsoft.Win32;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Inf;

namespace TranslateCS2.ExImport.Helpers;

internal static class ImExportDialogHelper {
    public static string? ShowSaveFileDialog(string? path,
                                             string title,
                                             string? fileName,
                                             string dialogWarningCaption,
                                             string dialogWarningText) {
        SaveFileDialog dialog = new SaveFileDialog {
            Title = title,
            CheckPathExists = true,
            RestoreDirectory = true,
            FileName = fileName ?? AppConfigurationManager.ImExportDefaultFileName,
            Filter = AppConfigurationManager.ImExportFilter,
            ValidateNames = true,
            DereferenceLinks = false,
            InitialDirectory = path ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        };

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
            ok = dialog.FileName.EndsWith(ModConstants.JsonExtension);
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
