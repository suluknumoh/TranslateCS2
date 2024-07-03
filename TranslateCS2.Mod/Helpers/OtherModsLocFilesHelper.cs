using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal.IO.AssetDatabase;

using Game.Modding;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Enums;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Helpers;
[MyExcludeFromCoverage]
internal static class OtherModsLocFilesHelper {
    public static IList<ModInfoLocFiles> GetOtherModsLocFiles(IModRuntimeContainer runtimeContainer) {
        List<ModInfoLocFiles> fileInfos = [];
        ModManager? modManager = runtimeContainer.ModManager;
        ExecutableAsset? modAsset = runtimeContainer.ModAsset;
        if (modManager is null
            || modAsset is null) {
            return [];
        }
        // 
        // 
        // 
        // proof of concept to obtain mods in their load-order or to gather their load order somehow
        // requires a reference to the following:
        // Colossal.PSI.PdxSdk
        // PDX.SDK
        // 
        // GetModsInActivePlayset() somewhere orders mods by their load order, but the load order itself can not be retrieved...
        // 
        //PdxSdkPlatform platform = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
        //List<PDX.SDK.Contracts.Service.Mods.Models.Mod>? modsEnabledInActivePlaysetOrderedByLoadOrder =
        //    platform.GetModsInActivePlayset().GetAwaiter().GetResult();
        //
        //
        //
        IEnumerator<ModManager.ModInfo> enumerator = modManager.GetEnumerator();
        while (enumerator.MoveNext()) {
            ModManager.ModInfo currentModInfo = enumerator.Current;
            ExecutableAsset asset = currentModInfo.asset;
            if (IsToSkip(currentModInfo, asset)) {
                continue;
            }
            Colossal.PSI.Common.Mod currentMod = asset.mod;
            string modPath = currentMod.path;
            if (asset.isLocal) {
                // for online-mods, modPath (currentMod.path) is correct
                // if a mod is local, the modPath includes the dll-name
                modPath = GetModPath(asset);
            }
            string specificDirectoryPath = Path.Combine(modPath, ModConstants.OtherModsLocFilePath);
            DirectoryInfo directoryInfo = new DirectoryInfo(specificDirectoryPath);
            if (directoryInfo.Exists) {
                IEnumerable<FileInfo> files = directoryInfo.EnumerateFiles(ModConstants.JsonSearchPattern);
                if (!files.Any()) {
                    continue;
                }
                int modId = currentMod.id;
                string modName = currentMod.displayName;
                ModInfoLocFiles otherModsLocFileModInfo = new ModInfoLocFiles(modId,
                                                                              modName,
                                                                              asset.version,
                                                                              asset.isLocal,
                                                                              FlavorSourceTypes.OTHER,
                                                                              files);
                fileInfos.Add(otherModsLocFileModInfo);
            }
        }
        return fileInfos;
    }

    private static string GetModPath(ExecutableAsset asset) {
        int index = asset.path.IndexOf(asset.subPath);
        string modsSubscribedPath = asset.path.Substring(0, index);
        string modPath = Path.Combine(modsSubscribedPath, asset.subPath);
        return modPath;
    }

    private static bool IsToSkip(ModManager.ModInfo current, ExecutableAsset asset) {
        // no need to care about
        // current.isLoaded / !current.isLoaded
        // a mod could be loaded after this one

        // no need to care about
        // current.isBursted

        if (!current.isValid
            || !asset.isMod
            || !asset.canBeLoaded) {
            // invalid
            // or no mod (additional libraries within mod)
            // or can not be loaded
            return true;
        }
        if (asset.isDirty
            || asset.isDummy) {
            return true;
        }
        if (!asset.isLocal
            && !asset.isEnabled
            && !asset.isInActivePlayset
            ) {
            // neither local
            // nor enabled in active playset
            return true;
        }
        return false;
    }
}
