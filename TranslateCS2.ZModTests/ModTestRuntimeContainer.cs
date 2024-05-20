using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Containers;
using TranslateCS2.ZZZTestLib;

namespace TranslateCS2.ZModDevHelper;
internal class ModTestRuntimeContainer : AModRuntimeContainer {
    private static readonly Paths paths = new Paths(true,
                                                    CS2PathsHelper.GetStreamingDataPathFromProps(),
                                                    CS2PathsHelper.GetUserDataPathFromEnvironment());


    public ModTestRuntimeContainer(IMyLogProvider logProvider) : base(logProvider, paths) { }



}
