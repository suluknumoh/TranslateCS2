using Colossal.PSI.Environment;

using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;

namespace TranslateCS2.Mod.Containers;
internal class ModRuntimeContainer : AModRuntimeContainer {
    public ModRuntimeContainer(GameManager gameManager,
                               IMyLogProvider logProvider) : base(gameManager,
                                                                   logProvider,
                                                                   new Paths(true,
                                                                   EnvPath.kStreamingDataPath,
                                                                   EnvPath.kUserDataPath)) { }
}
