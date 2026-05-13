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
using ComponentModel = System.ComponentModel;

namespace WPF_Main
{
	public class CursaFormDraft : ComponentModel.INotifyPropertyChanged, ComponentModel.IDataErrorInfo
	{
		private const int MIN_DISTANTA = 1;

		private string _distantaText = "";
		public string DistantaText
		{
			get => _distantaText;
			set { _distantaText = value; PropertyChanged?.Invoke(this, new ComponentModel.PropertyChangedEventArgs(nameof(DistantaText))); }
		}

		private MasinaModel _masinaSelectata;
		public MasinaModel MasinaSelectata
		{
			get => _masinaSelectata;
			set { _masinaSelectata = value; PropertyChanged?.Invoke(this, new ComponentModel.PropertyChangedEventArgs(nameof(MasinaSelectata))); }
		}

		private ConducatorModel _conducatorSelectat;
		public ConducatorModel ConducatorSelectat
		{
			get => _conducatorSelectat;
			set { _conducatorSelectat = value; PropertyChanged?.Invoke(this, new ComponentModel.PropertyChangedEventArgs(nameof(ConducatorSelectat))); }
		}

		public string Error => null;

		public string this[string columnName]
		{
			get
			{
				if (columnName == nameof(DistantaText))
				{
					if (string.IsNullOrWhiteSpace(DistantaText)) return "Distanța este obligatorie.";
					if (!int.TryParse(DistantaText, out int dist)) return "Trebuie să introduceți un număr întreg.";
					if (dist < MIN_DISTANTA) return $"Distanța minimă este de {MIN_DISTANTA} km.";
				}
				if (columnName == nameof(MasinaSelectata))
				{
					if (MasinaSelectata == null) return "Selectați o mașină.";
				}
				if (columnName == nameof(ConducatorSelectat))
				{
					if (ConducatorSelectat == null) return "Selectați un conducător.";
				}
				return null;
			}
		}

		public bool IsValid => string.IsNullOrEmpty(this[nameof(DistantaText)]) && string.IsNullOrEmpty(this[nameof(MasinaSelectata)]) && string.IsNullOrEmpty(this[nameof(ConducatorSelectat)]);

		public event ComponentModel.PropertyChangedEventHandler PropertyChanged;
	}

	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class CurseView : UserControl
	{
		private static IStocareData<CursaModel> adminCurse = StocareFactory.GetAdministratorStocare<CursaModel>();
		private static IStocareData<MasinaModel> adminMasini = StocareFactory.GetAdministratorStocare<MasinaModel>();
		private static IStocareData<ConducatorModel> adminConducatori = StocareFactory.GetAdministratorStocare<ConducatorModel>();

		private ObservableCollection<CursaModel> curseList;
		private CursaModel? cursaSelectata;
		public CursaFormDraft Draft { get; set; } = new CursaFormDraft();

		public CurseView()
		{
			InitializeComponent();
			DataContext = Draft;
			IncarcaDate();
		}

		private void IncarcaDate()
		{
			var curseExistente = adminCurse.ObtineToateElementele();
			curseList = new ObservableCollection<CursaModel>(curseExistente ?? new List<CursaModel>());
			CurseDataGrid.ItemsSource = curseList;

			cmbMasina.ItemsSource = adminMasini.ObtineToateElementele();
			cmbConducator.ItemsSource = adminConducatori.ObtineToateElementele();

			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(CurseDataGrid.ItemsSource);
			view.Filter = CursaFiltru;
		}

		private void tbCautareCursa_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(CurseDataGrid.ItemsSource).Refresh();
		}

		private bool CursaFiltru(object item)
		{
			if (string.IsNullOrEmpty(tbCautareCursa.Text))
				return true;

			var cursa = (CursaModel)item;
			string search = tbCautareCursa.Text;

			bool matchMasina = cursa.Masina?.Model?.Contains(search, StringComparison.OrdinalIgnoreCase) == true;
			bool matchConducator = cursa.Conducator?.Nume?.Contains(search, StringComparison.OrdinalIgnoreCase) == true;

			return matchMasina || matchConducator;
		}

		private void Adauga_Click(object sender, RoutedEventArgs e)
		{
			MesajStatus.Visibility = Visibility.Collapsed;

			if (!Draft.IsValid)
			{
				AfiseazaMesaj("Vă rugăm să selectați entitățile și o distanță validă.", true);
				return;
			}

			try
			{
				int dist = int.Parse(Draft.DistantaText);
				CursaModel nouaCursa = new CursaModel(dist, Draft.MasinaSelectata, Draft.ConducatorSelectat);
				adminCurse.AdaugaElement(nouaCursa);
				curseList.Add(nouaCursa);

				AfiseazaMesaj("Cursă adăugată cu succes!", false);

				Draft.DistantaText = string.Empty;
				Draft.MasinaSelectata = null;
				Draft.ConducatorSelectat = null;
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare: {ex.Message}", true);
			}
		}

		private void Modifica_Click(object sender, RoutedEventArgs e)
		{
			MesajStatus.Visibility = Visibility.Collapsed;

			if (cursaSelectata == null)
			{
				AfiseazaMesaj("Selectați o cursă din listă pentru modificare.", true);
				return;
			}

			if (!Draft.IsValid)
			{
				AfiseazaMesaj("Vă rugăm să selectați entitățile și o distanță validă.", true);
				return;
			}

			try
			{
				int dist = int.Parse(Draft.DistantaText);
				CursaModel cursaModificata = cursaSelectata.CreeazaCopieModificata(dist, Draft.MasinaSelectata, Draft.ConducatorSelectat);
				adminCurse.ActualizeazaElement(cursaModificata);
				IncarcaDate();
				cursaSelectata = cursaModificata;
				CurseDataGrid.SelectedItem = curseList.FirstOrDefault(c => c.Id == cursaModificata.Id);

				AfiseazaMesaj("Cursa a fost modificată cu succes!", false);
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare la modificarea cursei: {ex.Message}", true);
			}
		}

		private void CurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			cursaSelectata = CurseDataGrid.SelectedItem as CursaModel;
			if (cursaSelectata == null)
			{
				return;
			}

			Draft.DistantaText = cursaSelectata.Distanta.ToString();
			Draft.MasinaSelectata = cursaSelectata.Masina;
			Draft.ConducatorSelectat = cursaSelectata.Conducator;
		}

		private void AfiseazaMesaj(string mesaj, bool esteEroare)
		{
			MesajStatus.Text = mesaj;
			MesajStatus.Foreground = esteEroare ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGreen);
			MesajStatus.Visibility = Visibility.Visible;
		}
	}
}
