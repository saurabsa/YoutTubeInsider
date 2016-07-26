//------------------------------------------------------------------------------
// <copyright file="YouTubeInsiderSearchTool.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace YouTubeInsider
{

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("596ab61c-b789-4d17-8be3-1bab1066ed15")]
    public class YouTubeInsiderSearchTool : ToolWindowPane
    {
        public override bool SearchEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YouTubeInsiderSearchTool"/> class.
        /// </summary>
        public YouTubeInsiderSearchTool() : base(null)
        {
            this.Caption = "YouTubeInsider Search";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new YouTubeInsiderSearchToolControl();
        }

        public override IVsSearchTask CreateSearch(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback)
        {
            if (pSearchQuery == null || pSearchCallback == null)
                return null;
            return new YouTubeInsiderSearchToolTask(dwCookie, pSearchQuery, pSearchCallback, this);
        }

        public override void ClearSearch()
        {
            YouTubeInsiderSearchToolControl control = (YouTubeInsiderSearchToolControl)this.Content;
            control.reset();
            control.SearchResultsListBox.Items.Add(new YouTubeVideoElement(control.InitialContent, "", ""));
        }

        public override void ProvideSearchSettings(IVsUIDataSource pSearchSettings)
        {
            Utilities.SetValue(pSearchSettings,
                SearchSettingsDataSource.SearchStartTypeProperty.Name,
                 (uint)VSSEARCHSTARTTYPE.SST_ONDEMAND);
            Utilities.SetValue(pSearchSettings,
                SearchSettingsDataSource.SearchProgressTypeProperty.Name,
                 (uint)VSSEARCHPROGRESSTYPE.SPT_DETERMINATE);
        }

        private IVsEnumWindowSearchOptions m_optionsEnum;
        public override IVsEnumWindowSearchOptions SearchOptionsEnum
        {
            get
            {
                if (m_optionsEnum == null)
                {
                    List<IVsWindowSearchOption> list = new List<IVsWindowSearchOption>();

                    list.Add(this.VideoIdOption);

                    m_optionsEnum = new WindowSearchOptionEnumerator(list) as IVsEnumWindowSearchOptions;
                }
                return m_optionsEnum;
            }
        }

        private WindowSearchBooleanOption m_videoIdOptionOption;
        public WindowSearchBooleanOption VideoIdOption
        {
            get
            {
                if (m_videoIdOptionOption == null)
                {
                    m_videoIdOptionOption = new WindowSearchBooleanOption("Video ID search", "Type in YouTube video ID search", false);
                }
                return m_videoIdOptionOption;
            }
        }
    }

    public class YouTubeInsiderSearchToolTask : VsSearchTask
    {
        private YouTubeInsiderSearchTool m_toolWindow;

        // Determine the results. 
        private uint progress = 0;

        private uint maxResults = 50;

        private string searchString;

        public YouTubeInsiderSearchToolTask(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback, YouTubeInsiderSearchTool toolwindow)
            : base(dwCookie, pSearchQuery, pSearchCallback)
        {
            m_toolWindow = toolwindow;
        }

        public async System.Threading.Tasks.Task Run()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAJze2R6uaM9kA_U8YnwVzJWo_hX3O2O3s",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = searchString; // Replace with your search term.
            searchListRequest.MaxResults = maxResults;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        SearchCallback.ReportProgress(this, progress++, maxResults);
                        var thumbnail = searchResult.Snippet.Thumbnails;
                        //string video = String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId);
                        ThreadHelper.Generic.Invoke(() =>
                        {
                            ((List<String>)((YouTubeInsiderSearchToolControl)m_toolWindow.Content).SearchResultVideoIDs).Add(searchResult.Id.VideoId);
                            ((ListBox)((YouTubeInsiderSearchToolControl)m_toolWindow.Content).SearchResultsListBox).Items.Add(new YouTubeVideoElement(searchResult.Snippet.Title, searchResult.Id.VideoId, thumbnail.Medium.Url.ToString()));
                        });
                        break;
                }
            }
        }

        protected override void OnStartSearch()
        {
            // Get the search option. 
            bool videoIdSearch = false;
            videoIdSearch = m_toolWindow.VideoIdOption.Value;

            // Set variables that are used in the finally block.
            this.ErrorCode = VSConstants.S_OK;

            try
            {
                searchString = this.SearchQuery.SearchString;
                if (videoIdSearch == true)
                {
                    ThreadHelper.Generic.Invoke(() =>
                    {
                        foreach (YouTubeInsiderMediaPlayer player in ((YouTubeInsiderSearchToolControl)m_toolWindow.Content).OpenedVideoPlayers)
                        {
                            player.Dispose();
                        }
                        YouTubeInsiderMediaPlayer mediaPlayer = new YouTubeInsiderMediaPlayer(searchString);
                        mediaPlayer.Show();
                        ((YouTubeInsiderSearchToolControl)m_toolWindow.Content).OpenedVideoPlayers.Add(mediaPlayer);
                        progress = 1;
                        ((YouTubeInsiderSearchToolControl)m_toolWindow.Content).reset();
                        ((ListBox)((YouTubeInsiderSearchToolControl)m_toolWindow.Content).SearchResultsListBox).Items.Add(new YouTubeVideoElement("Opening Video ...", "", ""));
                    });
                }
                else
                {
                    try
                    {
                        ThreadHelper.Generic.Invoke(() =>
                        {
                            ((YouTubeInsiderSearchToolControl)m_toolWindow.Content).reset();
                            ((ListBox)((YouTubeInsiderSearchToolControl)m_toolWindow.Content).SearchResultsListBox).Items.Add(new YouTubeVideoElement("Search Results ...", "", ""));
                        });
                        Run().Wait();
                    }
                    catch (AggregateException ex)
                    {
                        foreach (var e in ex.InnerExceptions)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                    }
                }

                // Uncomment the following line to demonstrate the progress bar. 
                // System.Threading.Thread.Sleep(100);
            }
            catch (Exception e)
            {
                this.ErrorCode = VSConstants.E_FAIL;
            }
            finally
            {
                if (progress == 0)
                {
                    string noResult = String.Format("No results for \"{0}\".", searchString);
                    ThreadHelper.Generic.Invoke(() =>
                    {
                        ((YouTubeInsiderSearchToolControl)m_toolWindow.Content).reset();
                        ((ListBox)((YouTubeInsiderSearchToolControl)m_toolWindow.Content).SearchResultsListBox).Items.Add(new YouTubeVideoElement(noResult, "", ""));
                    });
                }
                this.SearchResults = progress;
            }

            // Call the implementation of this method in the base class. 
            // This sets the task status to complete and reports task completion. 
            base.OnStartSearch();
        }

        protected override void OnStopSearch()
        {
            this.SearchResults = 0;
        }
    }
}
