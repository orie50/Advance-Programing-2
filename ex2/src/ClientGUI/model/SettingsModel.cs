﻿namespace ClientGUI.model
{
    public class SettingsModel : ISettingModel
    {
        /// <summary>
        /// Gets or sets the server ip.
        /// </summary>
        /// <value>
        /// The server ip.
        /// </value>
        public string ServerIp
        {
            get { return Properties.Settings.Default.ServerIP; }
            set { Properties.Settings.Default.ServerIP = value; }
        }

        /// <summary>
        /// Gets or sets the server port.
        /// </summary>
        /// <value>
        /// The server port.
        /// </value>
        public int ServerPort
        {
            get { return Properties.Settings.Default.ServerPort; }
            set { Properties.Settings.Default.ServerPort = value; }
        }

        /// <summary>
        /// Gets or sets the maze rows.
        /// </summary>
        /// <value>
        /// The maze rows.
        /// </value>
        public int MazeRows
        {
            get { return Properties.Settings.Default.MazeRows; }
            set { Properties.Settings.Default.MazeRows = value; }
        }

        /// <summary>
        /// Gets or sets the maze cols.
        /// </summary>
        /// <value>
        /// The maze cols.
        /// </value>
        public int MazeCols
        {
            get { return Properties.Settings.Default.MazeCols; }
            set { Properties.Settings.Default.MazeCols = value; }
        }
        /// <summary>
        /// Gets or sets the search algorithm.
        /// </summary>
        /// <value>
        /// The search algorithm.
        /// </value>
        public int SearchAlgorithm
        {
            get { return Properties.Settings.Default.SearchAlgorithm; }
            set { Properties.Settings.Default.SearchAlgorithm = value; }
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
    }
}
