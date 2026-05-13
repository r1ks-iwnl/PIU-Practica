using System;
using AdministrareDate;
using Masina;
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
using MasinaModel = Masina.Masina;

namespace WPF_Main
{
	/// <summary>
	/// Interaction logic for MasiniView.xaml
	/// </summary>
	public partial class MasiniView : UserControl
	{
		private static IStocareData<MasinaModel> adminMasini = StocareFactory.GetAdministratorStocare<MasinaModel>();
		private ObservableCollection<MasinaModel> masiniList;

		public MasiniView()
		{
			InitializeComponent();

			int anCurrent = DateTime.Now.Year;
			for (int an = anCurrent; an >= 1970; an--)
			{
				cmbAn.Items.Add(an);
			}
			cmbAn.SelectedItem = anCurrent;

			IncarcaDate();
		}

		private void IncarcaDate()
		{
			var dateExistente = adminMasini.ObtineToateElementele();
			masiniList = new ObservableCollection<MasinaModel>(dateExistente ?? new List<MasinaModel>());
			MasiniDataGrid.ItemsSource = masiniList;
		}

		private void Adauga_Click(object sender, RoutedEventArgs e)
		{
			MesajStatus.Visibility = Visibility.Collapsed;

			string model = Nume.Text.Trim();
			int an = (int)(cmbAn.SelectedItem ?? DateTime.Now.Year);

			if (string.IsNullOrWhiteSpace(model))
			{
				AfiseazaMesaj("Vă rugăm să introduceți un model.", true);
				return;
			}

			CuloareMasina culoare = CuloareMasina.Alb; // Default fallback
			if (rbRosu?.IsChecked == true) culoare = CuloareMasina.Rosu;
			else if (rbAlb?.IsChecked == true) culoare = CuloareMasina.Alb;
			else if (rbNegru?.IsChecked == true) culoare = CuloareMasina.Negru;

			OptiuniMasina optiuni = OptiuniMasina.Niciuna;
			if (cbAerCond?.IsChecked == true) optiuni |= OptiuniMasina.AerConditionat;
			if (cbNavigatie?.IsChecked == true) optiuni |= OptiuniMasina.Navigatie;
			if (cbCutieAutom?.IsChecked == true) optiuni |= OptiuniMasina.CutieAutomata;
			if (cbSenzoriParc?.IsChecked == true) optiuni |= OptiuniMasina.SenzoriParcare;
			if (cbCameraMarș?.IsChecked == true) optiuni |= OptiuniMasina.CameraMarsarier;

			try
			{
				MasinaModel nouaMasina = new MasinaModel(model, an, culoare, optiuni);

				adminMasini.AdaugaElement(nouaMasina);
				masiniList.Add(nouaMasina);

				AfiseazaMesaj("Mașină adăugată cu succes!", false);
				Nume.Clear();
				cmbAn.SelectedItem = DateTime.Now.Year;
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
