using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Containers;
using TranslateCS2.ZZZModTestLib.Containers.Items;
using TranslateCS2.ZZZTestLib;

namespace TranslateCS2.ZZZModTestLib.Containers;
public class ModTestRuntimeContainer : ModRuntimeContainer {
    private static readonly Paths paths = new Paths(true,
                                                    CS2PathsHelper.GetStreamingDataPathFromProps(),
                                                    CS2PathsHelper.GetUserDataPathFromEnvironment());
    private static readonly TestIntSettings testIntSettings = new TestIntSettings();
    private static readonly TestLocManager testLocManager = new TestLocManager();
    public TestLocManager TestLocManager { get; } = testLocManager;
    public TestIntSettings TestIntSettings { get; } = testIntSettings;

    public ModTestRuntimeContainer(IMyLogProvider logProvider) : base(logProvider,
                                                                      testLocManager,
                                                                      testIntSettings,
                                                                      paths) { }



}
