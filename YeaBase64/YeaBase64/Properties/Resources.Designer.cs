﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.237
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace YeaBase64.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("YeaBase64.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 File existed, overwrite? 的本地化字符串。
        /// </summary>
        internal static string MainForm_fileExistedAlert {
            get {
                return ResourceManager.GetString("MainForm_fileExistedAlert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Overwrite? 的本地化字符串。
        /// </summary>
        internal static string MainForm_fileExistedAlertTitle {
            get {
                return ResourceManager.GetString("MainForm_fileExistedAlertTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Invalid base64 string.
        ///Note that the base64 string file must be plain text file encoded in ANSI. 的本地化字符串。
        /// </summary>
        internal static string MainForm_invalidBase64Alert {
            get {
                return ResourceManager.GetString("MainForm_invalidBase64Alert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Invalid path. 的本地化字符串。
        /// </summary>
        internal static string MainForm_invalidPathAlert {
            get {
                return ResourceManager.GetString("MainForm_invalidPathAlert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Real-time converting for long content may causes lag, are you sure want to continue? 的本地化字符串。
        /// </summary>
        internal static string Option_realtimeConvertThresholdAlert {
            get {
                return ResourceManager.GetString("Option_realtimeConvertThresholdAlert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Invalid value, threshold of content length must be integer 的本地化字符串。
        /// </summary>
        internal static string Option_realtimeConvertThresholdInvalid {
            get {
                return ResourceManager.GetString("Option_realtimeConvertThresholdInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Are you sure want to reset all the settings? 的本地化字符串。
        /// </summary>
        internal static string Option_resetAllAlertText {
            get {
                return ResourceManager.GetString("Option_resetAllAlertText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Reset All? 的本地化字符串。
        /// </summary>
        internal static string Option_resetAllAlertTitle {
            get {
                return ResourceManager.GetString("Option_resetAllAlertTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Oops, I can&apos;t save config file correctly... 的本地化字符串。
        /// </summary>
        internal static string Option_saveFailMessage {
            get {
                return ResourceManager.GetString("Option_saveFailMessage", resourceCulture);
            }
        }
    }
}
