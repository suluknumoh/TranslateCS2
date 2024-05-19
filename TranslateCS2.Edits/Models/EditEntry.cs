using System;
using System.Collections.Generic;

using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Edits.Properties.I18N;
using TranslateCS2.Inf;
using TranslateCS2.Inf.Keyz;
using TranslateCS2.Inf.Models;

namespace TranslateCS2.Edits.Models;
internal class EditEntry {
    private readonly ITranslationSessionManager sessionManager;
    public IMyKey Key { get; }

    public string? KeySetter {
        get => this.Key.Key;
        set {
            string? work = value?.Trim();
            if (this.IsDeleteAble) {
                if (StringHelper.IsNullOrWhiteSpaceOrEmpty(work)) {
                    throw new Exception(I18NEdits.InputWarningKeyEmpty);
                    //} else if (value.Contains(' ')) {
                    //    throw new Exception(I18NEdits.InputWarningSpaces);
                } else if (value != this.KeyOrigin) {
                    if (this.sessionManager.ExistsKeyInCurrentTranslationSession(work)) {
                        throw new Exception(I18NEdits.InputWarningKeyDuplicate);
                    } else if (IndexCountHelper.IndexMatcher.IsMatch(work)) {
                        IndexCountHelperValidationResult? result = this.sessionManager.IsIndexKeyValid(work, this.KeyOrigin);
                        if (result != null && !result.IsValid) {
                            throw new Exception(String.Format(I18NEdits.InputWarningKeyIndex, result.NextFreeIndex));
                        }
                    }
                }
            }
            this.Key.Key = work;
        }
    }
    private string? _Translation;
    public string? Translation {
        get => this._Translation;
        set {
            if (this.IsDeleteAble) {
                if (StringHelper.IsNullOrWhiteSpaceOrEmpty(value)) {
                    throw new Exception(I18NEdits.InputWarningTranslationEmpty);
                }
            }
            this._Translation = value;
        }
    }

    public HashSet<string> Keys { get; } = [];
    public int Count => this.Keys.Count;
    public string? KeyOrigin { get; }
    public string? Value { get; }
    public string? ValueMerge { get; }
    public bool IsDeleteAble { get; }
    public EditEntry(ITranslationSessionManager sessionManager,
                     IAppLocFileEntry entry) {
        this.sessionManager = sessionManager;
        this.Key = new MyKey(entry.Key.Key);
        this.Keys = entry.Keys;
        this.KeyOrigin = entry.KeyOrigin;
        this.Value = entry.Value;
        this.ValueMerge = entry.ValueMerge;
        this._Translation = entry.Translation;
        this.IsDeleteAble = entry.IsDeleteAble;
    }

    internal void ApplyChangesTo(IAppLocFileEntry original) {
        original.Key.Key = this.KeySetter;
        original.Translation = this.Translation;
    }
}
