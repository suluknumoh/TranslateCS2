using System;

using Colossal.IO.AssetDatabase;

using Game.Modding;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers;
internal interface IModRuntimeContainer {
    LocManager LocManager { get; }
    IntSettings IntSettings { get; }
    IIndexCountsProvider IndexCountsProvider { get; }
    IMyLogger Logger { get; }
    Paths Paths { get; }
    Locales Locales { get; }
    ErrorMessages ErrorMessages { get; }
    MyLanguages Languages { get; }
    IMod Mod { get; }
    ModSettings Settings { get; }
    ModManager? ModManager { get; }
    ExecutableAsset? ModAsset { get; }
    ISettingsSaver? SettingsSaver { get; }
    IBuiltInLocaleIdProvider BuiltInLocaleIdProvider { get; }
    void Init(Action<string, object, object?>? loadSettings = null, bool register = false);
    void Dispose(bool unregister = false);
}
