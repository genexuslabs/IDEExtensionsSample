﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeneXus.Packages.SupportTools {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GeneXus.Packages.SupportTools.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to no need to rename.
        /// </summary>
        internal static string NotRenamed {
            get {
                return ResourceManager.GetString("NotRenamed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to    Processing {0} &apos;{1}&apos;... .
        /// </summary>
        internal static string ProcessingObject {
            get {
                return ResourceManager.GetString("ProcessingObject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to renamed to &apos;{0}&apos;.
        /// </summary>
        internal static string Renamed {
            get {
                return ResourceManager.GetString("Renamed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to    - Renamed {0}/{1} {2}.
        /// </summary>
        internal static string RenamedObjects {
            get {
                return ResourceManager.GetString("RenamedObjects", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Renaming {0} to a maximum name length of {1}.
        /// </summary>
        internal static string RenamingObjects {
            get {
                return ResourceManager.GetString("RenamingObjects", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Shorten Names.
        /// </summary>
        internal static string ShortenNames {
            get {
                return ResourceManager.GetString("ShortenNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shorten &amp;Attribute names to first {0} characters.
        /// </summary>
        internal static string ShortenNamesAttributesOption {
            get {
                return ResourceManager.GetString("ShortenNamesAttributesOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shorten &amp;Object names to first {0} characters.
        /// </summary>
        internal static string ShortenNamesObjectsOption {
            get {
                return ResourceManager.GetString("ShortenNamesObjectsOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shorten Names.
        /// </summary>
        internal static string ShortenNamesSection {
            get {
                return ResourceManager.GetString("ShortenNamesSection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shorten &amp;Table &amp;&amp; Index names to first {0} characters.
        /// </summary>
        internal static string ShortenNamesTablesOption {
            get {
                return ResourceManager.GetString("ShortenNamesTablesOption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This tool allows you to shorten the names of attributes, tables, indexes, and other objects in the KB to the current significant name length in the corresponding category.
        ///
        ///For example, if the &apos;Significant attribute name length&apos; property is currently set to {0}, all attributes with longer names will be truncated to their first {0} characters. In the same way, names of tables and indexes will be truncated to the lenght specified by the &apos;Significant table name length&apos; property, and other objects to the leng [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ShortenNamesToolDescription {
            get {
                return ResourceManager.GetString("ShortenNamesToolDescription", resourceCulture);
            }
        }
    }
}
