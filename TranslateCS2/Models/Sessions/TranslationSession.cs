﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

using Prism.Mvvm;

using TranslateCS2.Configurations;
using TranslateCS2.Helpers;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Properties.I18N;

namespace TranslateCS2.Models.Sessions;
// https://learn.microsoft.com/en-us/archive/msdn-magazine/2010/june/msdn-magazine-input-validation-enforcing-complex-business-data-rules-with-wpf
// https://blog.magnusmontin.net/2013/08/26/data-validation-in-wpf/
internal class TranslationSession : BindableBase, IEquatable<TranslationSession?>, IDataErrorInfo {
    private long _ID;
    public long ID {
        get => this._ID;
        set => this.SetProperty(ref this._ID, value, this.ChangeRefresh);
    }
    private string? _Name;
    public string? Name {
        get => this._Name;
        set => this.SetProperty(ref this._Name, value, this.ChangeRefresh);
    }
    public DateTime Started { get; set; }


    private DateTime _LastEdited;
    public DateTime LastEdited {
        get => this._LastEdited;
        set => this.SetProperty(ref this._LastEdited, value, this.ChangeRefresh);
    }


    private string? _MergeLocalizationFileName = AppConfigurationManager.LeadingLocFileName;
    public string? MergeLocalizationFileName {
        get => this._MergeLocalizationFileName;
        set => this.SetProperty(ref this._MergeLocalizationFileName, value);
    }


    private string? _OverwriteLocalizationFileName;
    public string? OverwriteLocalizationFileName {
        get => this._OverwriteLocalizationFileName;
        set => this.SetProperty(ref this._OverwriteLocalizationFileName, value);
    }


    public string? OverwriteLocalizationLocaleID => this.OverwriteLocalizationFileName?.Split(".")[0];


    private string? _OverwriteLocalizationNameEN;
    public string? OverwriteLocalizationNameEN {
        get => this._OverwriteLocalizationNameEN;
        set => this.SetProperty(ref this._OverwriteLocalizationNameEN, value);
    }


    private string? _OverwriteLocalizationNameLocalized;
    public string? OverwriteLocalizationNameLocalized {
        get => this._OverwriteLocalizationNameLocalized;
        set {
            if (value != null) {
                this.SetProperty(ref this._OverwriteLocalizationNameLocalized, value.Replace(" ", String.Empty));
            } else {
                this.SetProperty(ref this._OverwriteLocalizationNameLocalized, value);
            }
        }
    }

    public bool IsAutoSave => true;


    public bool AreBaseAndMergeLocalizationFilesDifferent => this.OverwriteLocalizationFileName != this.MergeLocalizationFileName;
    public ObservableCollection<LocalizationDictionaryEntry> LocalizationDictionary { get; } = [];


    private string _DisplayName;
    public string DisplayName {
        get => this._DisplayName;
        set => this.SetProperty(ref this._DisplayName, value);
    }


    public TranslationSession() { }

    private void ChangeRefresh() {
        this.DisplayName = $"{nameof(this.ID)}: {this.ID} - {nameof(this.Name)}: {this.Name} - {nameof(this.Started)}: {this.Started.ToLocalTime()} - {nameof(this.LastEdited)}: {this.LastEdited.ToLocalTime()}";
    }


    public string Error => String.Empty;

    public string this[string columnName] {
        get {
            switch (columnName) {
                case nameof(this.Name):
                    if (String.IsNullOrEmpty(this.Name) || String.IsNullOrWhiteSpace(this.Name)) {
                        return I18NSessions.InputWarningNotEmptyOrWhitespace;
                    }
                    break;
                case nameof(this.OverwriteLocalizationNameEN):
                    if (String.IsNullOrEmpty(this.OverwriteLocalizationNameEN) || String.IsNullOrWhiteSpace(this.OverwriteLocalizationNameEN)) {
                        return I18NSessions.InputWarningNotEmptyOrWhitespace;
                    }
                    Regex regex = new Regex("^[a-zA-Z]+$");
                    if (!regex.IsMatch(this.OverwriteLocalizationNameEN)) {
                        return I18NSessions.InputWarningConsistOfCharacters;
                    }
                    break;
                case nameof(this.OverwriteLocalizationNameLocalized):
                    if (String.IsNullOrEmpty(this.OverwriteLocalizationNameLocalized) || String.IsNullOrWhiteSpace(this.OverwriteLocalizationNameLocalized)) {
                        return I18NSessions.InputWarningNotEmptyOrWhitespace;
                    }
                    break;
                case nameof(this.OverwriteLocalizationFileName):
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.OverwriteLocalizationFileName)) {
                        return I18NSessions.InputOverwriteFileWarningEmpty;
                    } else if (this.OverwriteLocalizationFileName == this.MergeLocalizationFileName) {
                        return I18NSessions.InputOverwriteFileWarningDifferentMergeFile;
                    } else if (this.OverwriteLocalizationFileName == AppConfigurationManager.LeadingLocFileName) {
                        return $"{I18NSessions.InputOverwriteFileWarningOthersThan} '{AppConfigurationManager.LeadingLocFileName}'!";
                    }
                    break;
            }
            return String.Empty;
        }
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as TranslationSession);
    }

    public bool Equals(TranslationSession? other) {
        return other is not null &&
               this.ID == other.ID;
    }

    public override int GetHashCode() {
        return HashCode.Combine(this.ID);
    }

    internal bool HasTranslationForKey(string key) {
        return this.LocalizationDictionary.Where(item => item.Key == key && item.IsTranslated).Any();
    }

    public static bool operator ==(TranslationSession? left, TranslationSession? right) {
        return EqualityComparer<TranslationSession>.Default.Equals(left, right);
    }

    public static bool operator !=(TranslationSession? left, TranslationSession? right) {
        return !(left == right);
    }
}
