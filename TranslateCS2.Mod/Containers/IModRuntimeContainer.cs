using Colossal.Localization;
using Colossal.Logging;

using Game.Settings;

using TranslateCS2.Mod.Helpers;

namespace TranslateCS2.Mod.Containers;
public interface IModRuntimeContainer {
    ILog? Logger { get; }
    LocalizationManager? LocManager { get; }
    InterfaceSettings? IntSetting { get; }
    LocaleHelper LocaleHelper { get; }
    FileSystemHelper FileSystemHelper { get; }
    ErrorMessageHelper ErrorMessageHelper { get; }
    string UserDataPath { get; }
    string StreamingDataPath { get; }
}
