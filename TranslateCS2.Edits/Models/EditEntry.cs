using System;
using System.Collections.Generic;
using System.ComponentModel;

using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Edits.Properties.I18N;
using TranslateCS2.Inf;
using TranslateCS2.Inf.Keyz;
using TranslateCS2.Inf.Models;

namespace TranslateCS2.Edits.Models;
internal class EditEntry : IDataErrorInfo {
    private readonly ITranslationSessionManager sessionManager;
    public IMyKey Key { get; }

    public string? KeySetter {
        get => this.Key.Key;
        set => this.Key.Key = value?.Trim();
    }
    public string? Translation { get; set; }

    public HashSet<string> Keys { get; } = [];
    public int Count => this.Keys.Count;
    public string? KeyOrigin { get; }
    public string? Value { get; }
    public string? ValueMerge { get; }
    public bool IsDeleteAble { get; }

    public string Error => String.Empty;


    public EditEntry(ITranslationSessionManager sessionManager,
                     IAppLocFileEntry entry) {
        this.sessionManager = sessionManager;
        this.Key = new MyKey(entry.Key.Key);
        this.Keys = entry.Keys;
        this.KeyOrigin = entry.KeyOrigin;
        this.Value = entry.Value;
        this.ValueMerge = entry.ValueMerge;
        this.Translation = entry.Translation;
        this.IsDeleteAble = entry.IsDeleteAble;
    }

    internal void ApplyChangesTo(IAppLocFileEntry original) {
        original.Key.Key = this.KeySetter;
        original.Translation = this.Translation;
    }

    public string this[string columnName] {
        get {
            switch (columnName) {
                case nameof(this.KeySetter):
                    if (this.IsDeleteAble) {
                        if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.KeySetter)) {
                            return I18NEdits.InputWarningKeyEmpty;
                            //} else if (value.Contains(' ')) {
                            //    throw new Exception(I18NEdits.InputWarningSpaces);
                        } else if (this.KeySetter != this.KeyOrigin) {
                            if (this.sessionManager.ExistsKeyInCurrentTranslationSession(this.KeySetter)) {
                                return I18NEdits.InputWarningKeyDuplicate;
                            } else if (IndexCountHelper.IndexMatcher.IsMatch(this.KeySetter)) {
                                IndexCountHelperValidationResult? result = this.sessionManager.IsIndexKeyValid(this.KeySetter, this.KeyOrigin);
                                if (result is not null
                                    && !result.IsValid) {
                                    return String.Format(I18NEdits.InputWarningKeyIndex, result.NextFreeIndex);
                                }
                            }
                        }
                    }
                    break;
                case nameof(this.Translation):
                    if (this.IsDeleteAble) {
                        if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation)) {
                            return I18NEdits.InputWarningTranslationEmpty;
                        }
                    }
                    break;
            }
            return String.Empty;
        }
    }
}
