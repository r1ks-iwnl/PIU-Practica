using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Main
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void btnConducator_Click(object sender, RoutedEventArgs e)
		{
            // Set the content area to your new view
			MainContent.Content = new ConducatoriView();
		}

		private void btnMasina_Click(object sender, RoutedEventArgs e)
		{
			MainContent.Content = new MasiniView();
		}

		private void btnCursa_Click(object sender, RoutedEventArgs e)
		{
			MainContent.Content = new CurseView();
		}
	}
}