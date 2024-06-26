using System;
using System.Collections.Generic;
using System.IO;

using Colossal.IO.AssetDatabase;

using Game.Modding;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Enums;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Helpers;
[MyExcludeFromCoverage]
internal class OtherModsLocFilesHelper {
    public static IList<ModInfoLocFiles> GetOtherModsLocFiles(IModRuntimeContainer runtimeContainer) {
        List<ModInfoLocFiles> fileInfos = [];
        ModManager? modManager = runtimeContainer.ModManager;
        ExecutableAsset? modAsset = runtimeContainer.ModAsset;
        if (modManager is null
            || modAsset is null) {
            return [];
        }
        IEnumerator<ModManager.ModInfo> enumerator = modManager.GetEnumerator();
        while (enumerator.MoveNext()) {
            ModManager.ModInfo current = enumerator.Current;
            ExecutableAsset asset = current.asset;
            if (IsToSkip(current, asset)) {
                continue;
            }
            string specificDirectoryPath = GetSpecificDirectoryPath(asset);
            DirectoryInfo directoryInfo = new DirectoryInfo(specificDirectoryPath);
            if (directoryInfo.Exists) {
                IEnumerable<FileInfo> files = directoryInfo.EnumerateFiles(ModConstants.JsonSearchPattern);
                string modId = ExtractIdFromSubPath(asset.subPath);
                ModInfoLocFiles otherModsLocFileModInfo = new ModInfoLocFiles(modId,
                                                                              asset.name,
                                                                              asset.version,
                                                                              asset.isLocal,
                                                                              FlavorSourceTypes.OTHER,
                                                                              files);
                fileInfos.Add(otherModsLocFileModInfo);
            }
        }
        return fileInfos;
    }

    private static string GetSpecificDirectoryPath(ExecutableAsset asset) {
        int index = asset.path.IndexOf(asset.subPath);
        string modsSubscribedPath = asset.path.Substring(0, index);
        string modPath = Path.Combine(modsSubscribedPath, asset.subPath);
        string specificDirectoryPath = Path.Combine(modPath, ModConstants.OtherModsLocFilePath);
        return specificDirectoryPath;
    }

    private static bool IsToSkip(ModManager.ModInfo current, ExecutableAsset asset) {
        // no need to care about
        // current.isLoaded / !current.isLoaded
        // a mod could be loaded after this one

        // no need to care about
        // current.isBursted

        if (!current.isValid
            || !asset.isMod) {
            // invalid or no mod (additional libraries within mod)
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

    /// <summary>
    ///     
    /// </summary>
    /// <param name="subPath">
    ///     <see cref="ExecutableAsset"/>
    ///     <br/>
    ///     <see cref="AssetData.subPath"/>
    /// </param>
    /// <returns>
    ///     the extracted id
    ///     <br/>
    ///     or
    ///     <br/>
    ///     <see cref="StringConstants.LocalMod"/>
    ///     <br/>
    ///     if id could not be extracted
    /// </returns>
    private static string ExtractIdFromSubPath(string subPath) {
        int startIndex = subPath.LastIndexOf(StringConstants.ForwardSlash) + 1;
        string idStringPre = subPath.Substring(startIndex);
        string idString = idStringPre.Split(StringConstants.UnderscoreChar)[0];
        bool parsed = Int32.TryParse(idString, out int id);
        if (parsed
            && id > 0) {
            return idString;
        }
        return StringConstants.LocalMod;
    }
}
