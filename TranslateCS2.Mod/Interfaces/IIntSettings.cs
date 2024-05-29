using Game.Settings;

namespace TranslateCS2.Mod.Interfaces;
/// <summary>
///     <see cref="InterfaceSettings"/>
/// </summary>
internal interface IIntSettings {
    /// <summary>
    ///     <see cref="InterfaceSettings.currentLocale"/>
    /// </summary>
    string CurrentLocale { get; set; }
    /// <summary>
    ///     <see cref="InterfaceSettings.locale"/>
    /// </summary>
    string Locale { get; set; }
    /// <summary>
    ///     <see cref="Setting.ApplyAndSave"/>
    /// </summary>
    void ApplyAndSave();
    /// <summary>
    ///     <see cref="Setting.onSettingsApplied"/>
    /// </summary>
    void SubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAndSaveAlso);
    /// <summary>
    ///     <see cref="Setting.onSettingsApplied"/>
    /// </summary>
    void UnSubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAndSaveAlso);
}
