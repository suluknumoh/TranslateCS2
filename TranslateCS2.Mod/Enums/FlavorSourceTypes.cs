
namespace TranslateCS2.Mod.Enums;
internal enum FlavorSourceTypes : uint {
    /// <summary>
    ///     <see cref="TranslateCS2.Mod.Containers.Items.FlavorSource"/> that is provided within this <see cref="TranslateCS2.Mod.Mod"/>s data-directory
    /// </summary>
    THIS = 0,
    /// <summary>
    ///     <see cref="TranslateCS2.Mod.Containers.Items.FlavorSource"/> that is provided via another <see cref="TranslateCS2.Mod.Mod"/>s directory (direct and specific directory within the mod itself)
    /// </summary>
    OTHER = 1
}
