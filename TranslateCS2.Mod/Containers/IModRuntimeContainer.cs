using System;

using Game.Modding;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers.Items;

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
    ModSettingsLocale SettingsLocale { get; }
    void Init(Action<string, object, object?>? loadSettings = null, bool register = false);
    void Dispose(bool unregister = false);
}
