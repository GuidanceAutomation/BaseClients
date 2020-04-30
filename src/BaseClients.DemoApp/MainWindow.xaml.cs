using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BaseClients.Controls;

namespace BaseClients.DemoApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IsConnectedControl nullControl = null;

		public MainWindow()
		{
			InitializeComponent();

			string html = Markdown.ToHtml(Properties.Resources.MainWindowDescription);
			webBrowser.NavigateToString(html);
		}

		private void RandomizeButton_Click(object sender, RoutedEventArgs e)
		{
			FooCallbackClient client = FindResource("fooCallbackClient") as FooCallbackClient;
			client.Randomize();
		}
	}
}
