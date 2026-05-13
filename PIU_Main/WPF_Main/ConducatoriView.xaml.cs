using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ConducatorModel = Conducator.Conducator;
using AdministrareDate;

namespace WPF_Main
{
	/// <summary>
	/// Interaction logic for ConducatoriView.xaml
	/// </summary>
	public partial class ConducatoriView : UserControl
	{
		private static ConducatorModel? conducatorCurent = null;
		private static IStocareData<ConducatorModel> adminConducatori = StocareFactory.GetAdministratorStocare<ConducatorModel>();

		// ObservableCollection automatically updates the UI when items are added/removed
		private ObservableCollection<ConducatorModel> conducatoriList;


		public ConducatoriView()
		{
			InitializeComponent();
			IncarcaDate();
		}

		private void IncarcaDate()
		{
			// Load existing data from JSON / Memory using the factory
			var dateExistente = adminConducatori.ObtineToateElementele();
			conducatoriList = new ObservableCollection<ConducatorModel>(dateExistente ?? new List<ConducatorModel>());

			// Bind the list to the DataGrid
			ConducatoriDataGrid.ItemsSource = conducatoriList;
		}

		private void btnAdauga_Click(object sender, RoutedEventArgs e)
		{
			// Read values from UI controls
			string numeComplet = $"{tbNume.Text} {tbPrenume.Text}".Trim();
			string dataNastere = pickDataNastere.SelectedDate?.ToString("d") ?? string.Empty;
			string dataAngajare = pickDataAngajare.SelectedDate?.ToString("d") ?? string.Empty;

			// Validate inputs
			if (string.IsNullOrWhiteSpace(numeComplet) || string.IsNullOrWhiteSpace(dataNastere) || string.IsNullOrWhiteSpace(dataAngajare))
			{
				AfiseazaMesaj("Vă rugăm să completați toate câmpurile.", true);
				return;
			}

			try
			{
				// Create the new driver
				ConducatorModel nouConducator = new ConducatorModel(numeComplet, dataNastere, dataAngajare);
				adminConducatori.AdaugaElement(nouConducator);

				IncarcaDate();

				// Save the newly created driver
				// Assuming 'Add' or a similar method exists inside IStocareData
				// adminConducatori.Add(nouConducator); 

				AfiseazaMesaj("Conducătorul a fost adăugat cu succes!", false);

				// Clear the form after saving
				tbNume.Clear();
				tbPrenume.Clear();
				pickDataNastere.SelectedDate = null;
				pickDataAngajare.SelectedDate = null;
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare la adăugarea conducătorului: {ex.Message}", true);
			}
		}

		private void AfiseazaMesaj(string mesaj, bool esteEroare)
		{
			tbMesajStatus.Text = mesaj;
			tbMesajStatus.Foreground = esteEroare ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGreen);
			tbMesajStatus.Visibility = Visibility.Visible;
		}
	}
}
