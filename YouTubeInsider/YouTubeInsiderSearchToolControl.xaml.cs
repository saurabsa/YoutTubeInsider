//------------------------------------------------------------------------------
// <copyright file="YouTubeInsiderSearchToolControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
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

            this.SearchResultsListBox.Items.Add(new YouTubeVideoElement(this.InitialContent, "", Constants.YouTubeInsiderIconUrl));

            this.OpenedVideoPlayers = new List<YouTubeInsiderMediaPlayer>();
        }

        private string BuildContent()
        {
            return "Search YouTube Videos ...";
        }

        public void reset()
        {
            this.SearchResultsListBox.Items.Clear();
        }

        private void resultsTextList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = resultsTextList.SelectedIndex;
            if (index > 0)
            {
                YouTubeVideoElement item = (YouTubeVideoElement) resultsTextList.SelectedItem;
                if (item != null)
                {
                    foreach (YouTubeInsiderMediaPlayer player in OpenedVideoPlayers)
                    {
                        player.Dispose();
                    }
                    System.Windows.Forms.Application.EnableVisualStyles();
                    //System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                    //System.Windows.Forms.Application.Run(new YouTubeInsiderMediaPlayer(videoIdsArray[index - 1]));
                    YouTubeInsiderMediaPlayer mediaPlayer = new YouTubeInsiderMediaPlayer(item.VideoId, item.VideoName);
                    mediaPlayer.Show();
                    OpenedVideoPlayers.Add(mediaPlayer);
                }
            }
        }
    }

    public class YouTubeVideoElement
    {
        public string VideoName { get; set; }
        public string VideoId { get; set; }
        public string ImageUrl { get; set; }

        public YouTubeVideoElement(string VideoName, string VideoId, string ImageUrl)
        {
            this.VideoName = VideoName;
            this.VideoId = VideoId;
            this.ImageUrl = ImageUrl;
        }
    }
}