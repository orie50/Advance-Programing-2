using System.Windows;

namespace ClientGUI.view
{
    /// <summary>
    /// Interaction logic for Error.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        /// <summary>
        /// constructor of the <see cref="MessageWindow"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public MessageWindow(string msg)
        {
            InitializeComponent();
			Message.Text = msg;
        }
    }
}
