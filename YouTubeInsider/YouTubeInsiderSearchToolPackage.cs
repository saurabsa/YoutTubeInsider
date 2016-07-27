﻿//------------------------------------------------------------------------------
// <copyright file="YouTubeInsiderSearchToolPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace YouTubeInsider
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(YouTubeInsiderSearchTool))]
    [Guid(YouTubeInsiderSearchToolPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class YouTubeInsiderSearchToolPackage : Package
    {
        /// <summary>
        /// YouTubeInsiderSearchToolPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "1b3fdb50-a562-46b0-9ec8-6306cf2ab2a2";

        /// <summary>
        /// Initializes a new instance of the <see cref="YouTubeInsiderSearchTool"/> class.
        /// </summary>
        public YouTubeInsiderSearchToolPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        private async void InitializeTesseractData()
        {
            if (!Directory.Exists(Constants.TessDataFolderPath))
            {
                WebClient Client = new WebClient();
                string tempTessDataZipFilePath = Path.Combine(Constants.VisualStudioFolderPath, "tessData.zip");
                Client.DownloadFile(Constants.TessDataUrl, tempTessDataZipFilePath);
                ZipFile.ExtractToDirectory(tempTessDataZipFilePath, Constants.VisualStudioFolderPath);
                File.Delete(tempTessDataZipFilePath);
                string temp = Path.Combine(Constants.VisualStudioFolderPath, "tessdata-3.04.00");
                Directory.Move(temp, Constants.TessDataFolderPath);
            }
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            YouTubeInsiderSearchToolCommand.Initialize(this);
            InitializeTesseractData();
            base.Initialize();
        }

        #endregion
    }
}
