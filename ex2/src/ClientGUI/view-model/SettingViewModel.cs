using ClientGUI.view_model;
using System.ComponentModel;
using ClientGUI.model;

namespace ClientGUI
{
    class SettingViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// The model
		/// </summary>
		private readonly ISettingModel _model;
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// Initializes a new instance of the <see cref="SettingViewModel"/> class.
		/// </summary>
		/// <param name="model">The model.</param>
		public SettingViewModel(ISettingModel model)
        {
            _model = model;
        }
		/// <summary>
		/// Gets or sets the server ip.
		/// </summary>
		/// <value>
		/// The server ip.
		/// </value>
		public string ServerIp
        {
            get { return _model.ServerIp; }
            set
            {
               _model.ServerIp = value;
                OnPropertyChanged("ServerIP");
            }
        }
		/// <summary>
		/// Gets or sets the server port.
		/// </summary>
		/// <value>
		/// The server port.
		/// </value>
		public int ServerPort
        {
            get { return _model.ServerPort; }
            set
            {
                _model.ServerPort = value;
                OnPropertyChanged("ServerPort");
            }
        }
		/// <summary>
		/// Gets or sets the maze rows.
		/// </summary>
		/// <value>
		/// The maze rows.
		/// </value>
		public int MazeRows
        {
            get { return _model.MazeRows; }
            set
            {
                _model.MazeRows = value;
                OnPropertyChanged("MazeRows");
            }
        }
		/// <summary>
		/// Gets or sets the maze cols.
		/// </summary>
		/// <value>
		/// The maze cols.
		/// </value>
		public int MazeCols
        {
            get { return _model.MazeCols; }
            set
            {
                _model.MazeCols = value;
                OnPropertyChanged("MazeCols");
            }
        }
		/// <summary>
		/// Gets or sets the search algorithm.
		/// </summary>
		/// <value>
		/// The search algorithm.
		/// </value>
		public int SearchAlgorithm
        {
            get { return _model.SearchAlgorithm; }
            set
            {
                _model.SearchAlgorithm = value;
                OnPropertyChanged("SearchAlgorithm");
            }
        }
		/// <summary>
		/// Called when [property changed].
		/// </summary>
		/// <param name="name">The name.</param>
		private void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
		/// <summary>
		/// Saves the setting.
		/// </summary>
		public void SaveSetting()
        {
            _model.SaveSettings();
        }
    }
}
