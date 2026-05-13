using AdministrareDate;
using Cursa;
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
using MasinaModel = Masina.Masina;
using CursaModel = Cursa.Cursa;

namespace WPF_Main
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class CurseView : UserControl
	{
		private static IStocareData<CursaModel> adminCurse = StocareFactory.GetAdministratorStocare<CursaModel>();
		private static IStocareData<MasinaModel> adminMasini = StocareFactory.GetAdministratorStocare<MasinaModel>();
		private static IStocareData<ConducatorModel> adminConducatori = StocareFactory.GetAdministratorStocare<ConducatorModel>();

		private ObservableCollection<CursaModel> curseList;

		public CurseView()
		{
			InitializeComponent();
			IncarcaDate();
		}

		private void IncarcaDate()
		{
			var curseExistente = adminCurse.ObtineToateElementele();
			curseList = new ObservableCollection<CursaModel>(curseExistente ?? new List<CursaModel>());
			CurseDataGrid.ItemsSource = curseList;

			cmbMasina.ItemsSource = adminMasini.ObtineToateElementele();
			cmbConducator.ItemsSource = adminConducatori.ObtineToateElementele();
		}

		private void Adauga_Click(object sender, RoutedEventArgs e)
		{
			MesajStatus.Visibility = Visibility.Collapsed;

			var masinaSelectata = cmbMasina.SelectedItem as MasinaModel;
			var conducatorSelectat = cmbConducator.SelectedItem as ConducatorModel;

			if (masinaSelectata == null || conducatorSelectat == null || !int.TryParse(Distanta.Text, out int dist))
			{
				AfiseazaMesaj("Selectați o mașină, un conducător și introduceți o distanță validă.", true);
				return;
			}

			try
			{
				CursaModel nouaCursa = new CursaModel(dist, masinaSelectata, conducatorSelectat);
				adminCurse.AdaugaElement(nouaCursa);
				curseList.Add(nouaCursa);

				AfiseazaMesaj("Cursă adăugată cu succes!", false);

				Distanta.Clear();
				cmbMasina.SelectedItem = null;
				cmbConducator.SelectedItem = null;
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare: {ex.Message}", true);
			}
		}

		private void AfiseazaMesaj(string mesaj, bool esteEroare)
		{
			MesajStatus.Text = mesaj;
			MesajStatus.Foreground = esteEroare ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGreen);
			MesajStatus.Visibility = Visibility.Visible;
		}
	}
}
