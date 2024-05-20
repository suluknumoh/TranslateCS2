using Colossal.Localization;

using Game.Settings;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Containers.Items;

namespace TranslateCS2.Mod.Containers;
public interface IModRuntimeContainer {
    LocalizationManager? LocManager { get; }
    InterfaceSettings? IntSetting { get; }




    IMyLogger Logger { get; }
    Paths Paths { get; }
    Locales Locales { get; }
    ErrorMessages ErrorMessages { get; }
    MyLanguages Languages { get; }
    DropDownItems DropDownItems { get; }
}
