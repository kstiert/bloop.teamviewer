using System.Windows.Controls;

namespace Bloop.TeamViewer
{
    /// <summary>
    /// Interaction logic for GitHubPluginSettings.xaml
    /// </summary>
    public partial class TeamViewerPluginSettings : UserControl
    {
        public TeamViewerPluginSettings()
        {
            InitializeComponent();
            Token.Text = PluginSettings.Instance.Token;
        }

        private void Token_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            PluginSettings.Instance.Token = textBox.Text;
            PluginSettings.Instance.Save();
        }
    }
}
