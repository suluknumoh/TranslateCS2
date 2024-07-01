using TranslateCS2.Inf.Services.Localizations;

namespace TranslateCS2.Mod.Interfaces;
internal interface IReLoadAble {
    void ReLoad(LocFileService<string> locFileService);
}
