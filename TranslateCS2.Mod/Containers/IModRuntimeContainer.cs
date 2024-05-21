using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers;
public interface IModRuntimeContainer {
    ILocManager LocManager { get; }
    IIntSettings IntSettings { get; }




    IMyLogger Logger { get; }
    Paths Paths { get; }
    Locales Locales { get; }
    ErrorMessages ErrorMessages { get; }
    MyLanguages Languages { get; }
    DropDownItems DropDownItems { get; }
}
