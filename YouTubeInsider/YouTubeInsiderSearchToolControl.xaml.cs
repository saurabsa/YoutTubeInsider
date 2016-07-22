//------------------------------------------------------------------------------
// <copyright file="YouTubeInsiderSearchToolControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace YouTubeInsider
{
    /// <summary>
    /// Interaction logic for YouTubeInsiderSearchToolControl.
    /// </summary>
    public partial class YouTubeInsiderSearchToolControl : UserControl
    {
        public ListBox SearchResultsListBox { get; set; }
        public string InitialContent { get; set; }
        public List<String> SearchResultVideoIDs { get; set; }

        public List<YouTubeInsiderMediaPlayer> OpenedVideoPlayers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YouTubeInsiderSearchToolControl"/> class.
        /// </summary>
        public YouTubeInsiderSearchToolControl()
        {
            this.InitializeComponent();

            this.InitialContent = BuildContent();

            this.SearchResultsListBox = resultsTextList;
            resultsTextList.ScrollIntoView(resultsTextList);
            this.SearchResultsListBox.Items.Clear();
            this.SearchResultsListBox.Items.Add(this.InitialContent);
            this.SearchResultVideoIDs = new List<String>();
            this.OpenedVideoPlayers = new List<YouTubeInsiderMediaPlayer>();
        }

        private string BuildContent()
        {
            return "Search YouTube Videos ...";
        }

        public void reset()
        {
            this.SearchResultsListBox.Items.Clear();
            this.SearchResultVideoIDs.Clear();
        }

        private void resultsTextList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = resultsTextList.SelectedIndex;
            if (index > 0)
            {
                object item = resultsTextList.SelectedItem;
                if (item != null && SearchResultVideoIDs.Count > 0)
                {
                    foreach (YouTubeInsiderMediaPlayer player in OpenedVideoPlayers)
                    {
                        player.Dispose();
                    }
                    String[] videoIdsArray = SearchResultVideoIDs.ToArray();
                    YouTubeInsiderMediaPlayer mediaPlayer = new YouTubeInsiderMediaPlayer(videoIdsArray[index-1]);
                    mediaPlayer.Show();
                    OpenedVideoPlayers.Add(mediaPlayer);
                }
            }
        }
    }
}