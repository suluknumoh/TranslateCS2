using Colossal.Localization;
using Colossal.Logging;

using Game.Settings;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;

namespace TranslateCS2.Mod.Containers;
public interface IModRuntimeContainer {
    ILog? Logger { get; }
    LocalizationManager? LocManager { get; }
    InterfaceSettings? IntSetting { get; }
    bool DoubleCheck { get; }




    Paths Paths { get; }
    Locales Locales { get; }
    ErrorMessages ErrorMessages { get; }
    MyLanguages Languages { get; }
    DropDownItems DropDownItems { get; }
}
