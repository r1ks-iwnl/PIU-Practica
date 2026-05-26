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
using CursaModel = Cursa.Cursa;

namespace WPF_Main
{
	using ComponentModel = System.ComponentModel;

		public class MasinaFormDraft : FormDraftBase
	{
		private string _modelText = "";
		public string ModelText
		{
			get => _modelText;
				set => SetField(ref _modelText, value, nameof(ModelText));
		}

			public override string this[string columnName]
		{
			get
			{
				if (columnName == nameof(ModelText))
				{
					if (string.IsNullOrWhiteSpace(ModelText)) return "Vă rugăm să introduceți un model.";
					if (ModelText.Length < 2) return "Modelul trebuie să aibă cel puțin 2 caractere.";
				}
				return null;
			}
		}

			public bool IsValid => AreValid(nameof(ModelText));
	}

	/// <summary>
	/// Interaction logic for MasiniView.xaml
	/// </summary>
	public partial class MasiniView : UserControl
	{
		private static IStocareData<MasinaModel> adminMasini = StocareFactory.GetAdministratorStocare<MasinaModel>();
		private static IStocareData<CursaModel> adminCurse = StocareFactory.GetAdministratorStocare<CursaModel>();
		private ObservableCollection<MasinaModel> masiniList;
		private MasinaModel? masinaSelectata;

		public MasinaFormDraft Draft { get; set; } = new MasinaFormDraft();

		public MasiniView()
		{
			InitializeComponent();
			Nume.DataContext = Draft;

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

			// Seteaza filtrarea a CollectionView
			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(MasiniDataGrid.ItemsSource);
			view.Filter = MasinaFiltru;
		}

		private void tbCautareModel_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(MasiniDataGrid.ItemsSource).Refresh();
		}

		private bool MasinaFiltru(object item)
		{
			if (string.IsNullOrEmpty(tbCautareModel.Text))
				return true;
			else
				return ((MasinaModel)item).Model.Contains(tbCautareModel.Text, StringComparison.OrdinalIgnoreCase);
		}

		private void Adauga_Click(object sender, RoutedEventArgs e)
		{
			MesajStatus.Visibility = Visibility.Collapsed;

			// Validarea e bazata pe obiectul Draft si DataBindings
			if (!Draft.IsValid)
			{
				AfiseazaMesaj("Vă rugăm să introduceți corect datele modelului.", true);
				return;
			}

			string model = Draft.ModelText.Trim();
			int an = (int)(cmbAn.SelectedItem ?? DateTime.Now.Year);

			CuloareMasina culoare = CuloareMasina.Alb; // default
			if (rbRosu?.IsChecked == true) culoare = CuloareMasina.Rosu;
			else if (rbAlb?.IsChecked == true) culoare = CuloareMasina.Alb;
			else if (rbNegru?.IsChecked == true) culoare = CuloareMasina.Negru;

			OptiuniMasina optiuni = OptiuniMasina.Niciuna;
			if (cbAerCond?.IsChecked == true) optiuni |= OptiuniMasina.AerConditionat;
			if (cbNavigatie?.IsChecked == true) optiuni |= OptiuniMasina.Navigatie;
			if (cbCutieAutom?.IsChecked == true) optiuni |= OptiuniMasina.CutieAutomata;
			if (cbSenzoriParc?.IsChecked == true) optiuni |= OptiuniMasina.SenzoriParcare;
			if (cbCameraMars?.IsChecked == true) optiuni |= OptiuniMasina.CameraMarsarier;

			try
			{
				MasinaModel nouaMasina = new MasinaModel(model, an, culoare, optiuni);

				adminMasini.AdaugaElement(nouaMasina);
				masiniList.Add(nouaMasina);

				AfiseazaMesaj("Mașină adăugată cu succes!", false);
				Draft.ModelText = string.Empty;
				cmbAn.SelectedItem = DateTime.Now.Year;
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare: {ex.Message}", true);
			}
		}

		private void Modifica_Click(object sender, RoutedEventArgs e)
		{
			MesajStatus.Visibility = Visibility.Collapsed;

			if (masinaSelectata == null)
			{
				AfiseazaMesaj("Selectați o mașină din listă pentru modificare.", true);
				return;
			}

			if (!Draft.IsValid)
			{
				AfiseazaMesaj("Vă rugăm să introduceți corect datele modelului.", true);
				return;
			}

			try
			{
				string model = Draft.ModelText.Trim();
				int an = (int)(cmbAn.SelectedItem ?? DateTime.Now.Year);

				CuloareMasina culoare = CitesteCuloareSelectata();
				OptiuniMasina optiuni = CitesteOptiuniSelectate();

				MasinaModel masinaModificata = masinaSelectata.CreeazaCopieModificata(model, an, culoare, optiuni);
				adminMasini.ActualizeazaElement(masinaModificata);
				IncarcaDate();
				masinaSelectata = masinaModificata;
				MasiniDataGrid.SelectedItem = masiniList.FirstOrDefault(m => m.Id == masinaModificata.Id);

				AfiseazaMesaj("Mașina a fost modificată cu succes!", false);
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare la modificarea mașinii: {ex.Message}", true);
			}
		}

		private void Sterge_Click(object sender, RoutedEventArgs e)
		{
			MesajStatus.Visibility = Visibility.Collapsed;

			if (masinaSelectata == null)
			{
				AfiseazaMesaj("Selectați o mașină din listă pentru ștergere.", true);
				return;
			}

			var curse = adminCurse.ObtineToateElementele();
			if (curse.Any(c => c.Masina != null && c.Masina.Id == masinaSelectata.Id))
			{
				AfiseazaMesaj("Nu se poate șterge mașina deoarece există curse asociate.", true);
				return;
			}

			MessageBoxResult confirmare = MessageBox.Show(
				$"Sigur doriți să ștergeți mașina '{masinaSelectata.Model}'?",
				"Confirmare ștergere",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (confirmare != MessageBoxResult.Yes)
			{
				return;
			}

			try
			{
				adminMasini.EliminaElement(masinaSelectata);
				IncarcaDate();
				masinaSelectata = null;
				MasiniDataGrid.SelectedItem = null;

				Draft.ModelText = string.Empty;
				cmbAn.SelectedItem = DateTime.Now.Year;
				rbRosu.IsChecked = false;
				rbAlb.IsChecked = false;
				rbNegru.IsChecked = false;
				cbAerCond.IsChecked = false;
				cbNavigatie.IsChecked = false;
				cbCutieAutom.IsChecked = false;
				cbSenzoriParc.IsChecked = false;
				cbCameraMars.IsChecked = false;

				AfiseazaMesaj("Mașina a fost ștearsă cu succes!", false);
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare la ștergerea mașinii: {ex.Message}", true);
			}
		}

		private void MasiniDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			masinaSelectata = MasiniDataGrid.SelectedItem as MasinaModel;
			if (masinaSelectata == null)
			{
				return;
			}

			Draft.ModelText = masinaSelectata.Model;
			cmbAn.SelectedItem = masinaSelectata.An;
			SeteazaCuloare(masinaSelectata.Culoare);
			SeteazaOptiuni(masinaSelectata.Optiuni);
		}

		private void SeteazaCuloare(CuloareMasina culoare)
		{
			rbRosu.IsChecked = culoare == CuloareMasina.Rosu;
			rbAlb.IsChecked = culoare == CuloareMasina.Alb;
			rbNegru.IsChecked = culoare == CuloareMasina.Negru;
		}

		private CuloareMasina CitesteCuloareSelectata()
		{
			if (rbRosu?.IsChecked == true) return CuloareMasina.Rosu;
			if (rbAlb?.IsChecked == true) return CuloareMasina.Alb;
			if (rbNegru?.IsChecked == true) return CuloareMasina.Negru;
			return CuloareMasina.Alb;
		}

		private void SeteazaOptiuni(OptiuniMasina optiuni)
		{
			cbAerCond.IsChecked = optiuni.HasFlag(OptiuniMasina.AerConditionat);
			cbNavigatie.IsChecked = optiuni.HasFlag(OptiuniMasina.Navigatie);
			cbCutieAutom.IsChecked = optiuni.HasFlag(OptiuniMasina.CutieAutomata);
			cbSenzoriParc.IsChecked = optiuni.HasFlag(OptiuniMasina.SenzoriParcare);
			cbCameraMars.IsChecked = optiuni.HasFlag(OptiuniMasina.CameraMarsarier);
		}

		private OptiuniMasina CitesteOptiuniSelectate()
		{
			OptiuniMasina optiuni = OptiuniMasina.Niciuna;
			if (cbAerCond?.IsChecked == true) optiuni |= OptiuniMasina.AerConditionat;
			if (cbNavigatie?.IsChecked == true) optiuni |= OptiuniMasina.Navigatie;
			if (cbCutieAutom?.IsChecked == true) optiuni |= OptiuniMasina.CutieAutomata;
			if (cbSenzoriParc?.IsChecked == true) optiuni |= OptiuniMasina.SenzoriParcare;
			if (cbCameraMars?.IsChecked == true) optiuni |= OptiuniMasina.CameraMarsarier;
			return optiuni;
		}

		private void AfiseazaMesaj(string mesaj, bool esteEroare)
		{
			MesajStatus.Text = mesaj;
			MesajStatus.Foreground = esteEroare ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGreen);
			MesajStatus.Visibility = Visibility.Visible;
		}
	}
}
