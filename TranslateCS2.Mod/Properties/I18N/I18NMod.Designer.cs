﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TranslateCS2.Mod.Properties.I18N {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class I18NMod {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal I18NMod() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TranslateCS2.Mod.Properties.I18N.I18NMod", typeof(I18NMod).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to flavors.
        /// </summary>
        internal static string GroupFlavorTitle {
            get {
                return ResourceManager.GetString("GroupFlavorTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reloads the language file(s) that existed at startup. Does not unload language files. Does not load new language files..
        /// </summary>
        internal static string GroupReloadButtonReloadDescription {
            get {
                return ResourceManager.GetString("GroupReloadButtonReloadDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to reload language files.
        /// </summary>
        internal static string GroupReloadButtonReloadLabel {
            get {
                return ResourceManager.GetString("GroupReloadButtonReloadLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you really want to reload the language file(s)?.
        /// </summary>
        internal static string GroupReloadButtonReloadWarning {
            get {
                return ResourceManager.GetString("GroupReloadButtonReloadWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to reload.
        /// </summary>
        internal static string GroupReloadTitle {
            get {
                return ResourceManager.GetString("GroupReloadTitle", resourceCulture);
            }
        }
    }
}
