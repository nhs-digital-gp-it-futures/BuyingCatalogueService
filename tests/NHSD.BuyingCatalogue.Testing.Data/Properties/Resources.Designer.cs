﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NHSD.BuyingCatalogue.Testing.Data.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NHSD.BuyingCatalogue.Testing.Data.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to DELETE FROM [FrameworkCapabilities]
        ///DELETE FROM [FrameworkSolutions]
        ///UPDATE [Solution] SET SolutionDetailId = NULL
        ///DELETE FROM [SolutionDetail]
        ///DELETE FROM [SolutionEpic]
        ///DELETE FROM [SolutionCapability]
        ///DELETE FROM [Solution]
        ///DELETE FROM [Epic]
        ///DELETE FROM [Capability]
        ///DELETE FROM [Supplier]
        ///DELETE FROM [SupplierContact]
        ///.
        /// </summary>
        internal static string Clear {
            get {
                return ResourceManager.GetString("Clear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SET ANSI_NULLS ON
        ///GO
        ///
        ///SET QUOTED_IDENTIFIER ON
        ///GO
        ///
        ////*-----------------------------------------------------------------------
        ///--
        ///-- CompliancyLevel
        ///--
        ///------------------------------------------------------------------------*/
        ///CREATE TABLE [dbo].[CompliancyLevel](
        ///	[Id] [int] NOT NULL,
        ///	[Name] [varchar](16) NOT NULL,
        /// CONSTRAINT [PK_CompliancyLevel] PRIMARY KEY CLUSTERED
        ///(
        ///	[Id] ASC
        ///)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LO [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Create {
            get {
                return ResourceManager.GetString("Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE [FrameworkCapabilities]
        ///DROP TABLE [FrameworkSolutions]
        ///DROP TABLE [Capability]
        ///DROP TABLE [CapabilityStatus]
        ///DROP TABLE [CapabilityCategory]
        ///DROP TABLE [Framework]
        ///DROP TABLE [SolutionDetail]
        ///DROP TABLE [SolutionCapability]
        ///DROP TABLE [Solution]
        ///DROP TABLE [SolutionCapabilityStatus]
        ///DROP TABLE [Supplier]
        ///DROP TABLE [SolutionSupplierStatus]
        ///.
        /// </summary>
        internal static string Drop {
            get {
                return ResourceManager.GetString("Drop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT [dbo].[Framework] ([Id], [Name], [Description], [Owner], [ActiveDate], [ExpiryDate]) VALUES (N&apos;NHSDGP001&apos;, N&apos;NHS Digital GP Futures Framework 1&apos;, NULL, NULL, NULL, NULL)
        ///
        ///INSERT INTO [dbo].[CapabilityStatus] ([Id] ,[Name]) VALUES (1,&apos;Effective&apos;)
        ///
        ///INSERT INTO [dbo].[CapabilityCategory] ([Id] ,[Name]) VALUES (0,&apos;Undefined&apos;)
        ///
        ///INSERT INTO [dbo].[SolutionCapabilityStatus] ([Id], [Name], [Pass]) VALUES (1, &apos;Passed&apos;, 1);
        ///INSERT INTO [dbo].[SolutionCapabilityStatus] ([Id], [Name], [Pass]) VALUES (2, &apos; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ReferenceData {
            get {
                return ResourceManager.GetString("ReferenceData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE LOGIN NHSD WITH PASSWORD = &apos;DisruptTheMarket1!&apos;
        ///CREATE USER NHSD FOR LOGIN NHSD
        ///.
        /// </summary>
        internal static string User {
            get {
                return ResourceManager.GetString("User", resourceCulture);
            }
        }
    }
}
