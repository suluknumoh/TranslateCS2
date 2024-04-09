﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using Prism.Mvvm;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Properties.I18N;

namespace TranslateCS2.Core.Sessions;
// https://learn.microsoft.com/en-us/archive/msdn-magazine/2010/june/msdn-magazine-input-validation-enforcing-complex-business-data-rules-with-wpf
// https://blog.magnusmontin.net/2013/08/26/data-validation-in-wpf/
internal class TranslationSession : BindableBase, ITranslationSession, IEquatable<ITranslationSession?> {
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
    /// <summary>
    ///     local <see cref="DateTime"/> !!!
    ///     <br/>
    ///     gets converted to universal time within <see cref="TranslationsDB" />
    /// </summary>
    public DateTime Started { get; set; }


    private DateTime _LastEdited;
    /// <summary>
    ///     local <see cref="DateTime"/>!!!
    ///     <br/>
    ///     gets converted to universal time within <see cref="TranslationsDB"/>
    /// </summary>
    public DateTime LastEdited {
        get => this._LastEdited;
        set => this.SetProperty(ref this._LastEdited, value, this.ChangeRefresh);
    }


    private string? _MergeLocalizationFileName = AppConfigurationManager.LeadingLocFileName;
    public string? MergeLocalizationFileName {
        get => this._MergeLocalizationFileName;
        set => this.SetProperty(ref this._MergeLocalizationFileName, value);
    }


    public string? MergeLanguageCode => this.MergeLocalizationFileName?.Split(".")[0];


    private string? _OverwriteLocalizationFileName;
    public string? OverwriteLocalizationFileName {
        get => this._OverwriteLocalizationFileName;
        set => this.SetProperty(ref this._OverwriteLocalizationFileName, value);
    }


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


    public bool AreBaseAndMergeLocalizationFilesDifferent => this.OverwriteLocalizationFileName != this.MergeLocalizationFileName;
    public ObservableCollection<ILocalizationDictionaryEntry> LocalizationDictionary { get; } = [];


    private string? _DisplayName;
    public string? DisplayName {
        get => this._DisplayName;
        set => this.SetProperty(ref this._DisplayName, value);
    }


    public TranslationSession() {
        this.Started = DateTime.Now;
        this.LastEdited = this.Started;
    }

    private void ChangeRefresh() {
        this.DisplayName = $"{nameof(this.ID)}: {this.ID} - {nameof(this.Name)}: {this.Name} - {nameof(this.Started)}: {this.Started} - {nameof(this.LastEdited)}: {this.LastEdited}";
    }


    public string Error => String.Empty;

    public string this[string columnName] {
        get {
            switch (columnName) {
                case nameof(this.Name):
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Name)) {
                        return I18NSessions.InputWarningNotEmptyOrWhitespace;
                    }
                    break;
                case nameof(this.OverwriteLocalizationNameEN):
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.OverwriteLocalizationNameEN)) {
                        return I18NSessions.InputWarningNotEmptyOrWhitespace;
                    }
                    Regex regex = new Regex("^[a-zA-Z]+$");
                    if (!regex.IsMatch(this.OverwriteLocalizationNameEN)) {
                        return I18NSessions.InputWarningConsistOfCharacters;
                    }
                    break;
                case nameof(this.OverwriteLocalizationNameLocalized):
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.OverwriteLocalizationNameLocalized)) {
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
                case nameof(this.MergeLocalizationFileName):
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.MergeLocalizationFileName)) {
                        // cannot be empty
                    } else if (this.OverwriteLocalizationFileName == this.MergeLocalizationFileName) {
                        return I18NSessions.InputOverwriteFileWarningDifferentMergeFile;
                    }
                    break;
            }
            return String.Empty;
        }
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as TranslationSession);
    }

    public bool Equals(ITranslationSession? other) {
        return other is not null &&
               this.ID == other.ID;
    }

    public override int GetHashCode() {
        return HashCode.Combine(this.ID);
    }

    public static bool operator ==(TranslationSession? left, TranslationSession? right) {
        return EqualityComparer<TranslationSession>.Default.Equals(left, right);
    }

    public static bool operator !=(TranslationSession? left, TranslationSession? right) {
        return !(left == right);
    }
}