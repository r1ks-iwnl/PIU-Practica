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
		public class CursaFormDraft : FormDraftBase
	{
		private const int MIN_DISTANTA = 1;

		private string _distantaText = "";
		public string DistantaText
		{
			get => _distantaText;
				set => SetField(ref _distantaText, value, nameof(DistantaText));
		}

		private MasinaModel? _masinaSelectata;
		public MasinaModel? MasinaSelectata
		{
			get => _masinaSelectata;
				set => SetField(ref _masinaSelectata, value, nameof(MasinaSelectata));
		}

		private ConducatorModel? _conducatorSelectat;
		public ConducatorModel? ConducatorSelectat
		{
			get => _conducatorSelectat;
				set => SetField(ref _conducatorSelectat, value, nameof(ConducatorSelectat));
		}

		private DateTime? _dataStart;
		public DateTime? DataStart
		{
			get => _dataStart;
			set => SetField(ref _dataStart, value, nameof(DataStart));
		}

		private string _oraStartText = "";
		public string OraStartText
		{
			get => _oraStartText;
			set => SetField(ref _oraStartText, value, nameof(OraStartText));
		}

			public override string this[string columnName]
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
				if (columnName == nameof(DataStart))
				{
					if (!DataStart.HasValue) return "Selectați data de început a cursei.";
					if (DataStart.Value.Year < 2000) return "Introduceți o dată validă.";
				}
				if (columnName == nameof(OraStartText))
				{
					if (string.IsNullOrWhiteSpace(OraStartText)) return "Ora este obligatorie.";
					if (!TimeSpan.TryParse(OraStartText, out _)) return "Ora trebuie să fie în format valid (ex. 14:30).";
				}
				return string.Empty;
			}
		}

			public bool IsValid => AreValid(nameof(DistantaText), nameof(MasinaSelectata), nameof(ConducatorSelectat), nameof(DataStart), nameof(OraStartText));
	}

	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class CurseView : UserControl
	{
		private static IStocareData<CursaModel> adminCurse = StocareFactory.GetAdministratorStocare<CursaModel>();
		private static IStocareData<MasinaModel> adminMasini = StocareFactory.GetAdministratorStocare<MasinaModel>();
		private static IStocareData<ConducatorModel> adminConducatori = StocareFactory.GetAdministratorStocare<ConducatorModel>();

		private ObservableCollection<CursaModel> curseList = new();
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
				AfiseazaMesaj(Draft.GetFirstError(nameof(Draft.MasinaSelectata), nameof(Draft.ConducatorSelectat), nameof(Draft.DistantaText), nameof(Draft.DataStart), nameof(Draft.OraStartText)) ?? "Vă rugăm să corectați erorile de pe formular.", true);
				return;
			}

			try
			{
				int dist = int.Parse(Draft.DistantaText);
				TimeSpan ora = TimeSpan.Parse(Draft.OraStartText);
				DateTime startDateTime = Draft.DataStart.GetValueOrDefault().Date + ora;

				CursaModel nouaCursa = new CursaModel(dist, Draft.MasinaSelectata!, Draft.ConducatorSelectat!, startDateTime);
				adminCurse.AdaugaElement(nouaCursa);
				curseList.Add(nouaCursa);

				AfiseazaMesaj("Cursă adăugată cu succes!", false);

				Draft.DistantaText = string.Empty;
				Draft.MasinaSelectata = null;
				Draft.ConducatorSelectat = null;
				Draft.DataStart = null;
				Draft.OraStartText = string.Empty;
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
				AfiseazaMesaj(Draft.GetFirstError(nameof(Draft.MasinaSelectata), nameof(Draft.ConducatorSelectat), nameof(Draft.DistantaText), nameof(Draft.DataStart), nameof(Draft.OraStartText)) ?? "Vă rugăm să corectați erorile de pe formular.", true);
				return;
			}

			try
			{
				int dist = int.Parse(Draft.DistantaText);
				TimeSpan ora = TimeSpan.Parse(Draft.OraStartText);
				DateTime startDateTime = Draft.DataStart.GetValueOrDefault().Date + ora;

				CursaModel cursaModificata = cursaSelectata.CreeazaCopieModificata(dist, Draft.MasinaSelectata!, Draft.ConducatorSelectat!, startDateTime);
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

		private void Sterge_Click(object sender, RoutedEventArgs e)
		{
			MesajStatus.Visibility = Visibility.Collapsed;

			if (cursaSelectata == null)
			{
				AfiseazaMesaj("Selectați o cursă din listă pentru ștergere.", true);
				return;
			}

			CustomDialog confirmDialog = new CustomDialog($"Sigur doriți să ștergeți cursa pentru mașina '{cursaSelectata.Masina?.Model}'?");
			confirmDialog.Owner = Window.GetWindow(this);

			if (confirmDialog.ShowDialog() != true)
			{
				return;
			}

			try
			{
				adminCurse.EliminaElement(cursaSelectata);
				IncarcaDate();
				cursaSelectata = null;
				CurseDataGrid.SelectedItem = null;

				Draft.DistantaText = string.Empty;
				Draft.MasinaSelectata = null;
				Draft.ConducatorSelectat = null;
				Draft.DataStart = null;
				Draft.OraStartText = string.Empty;

				AfiseazaMesaj("Cursa a fost ștearsă cu succes!", false);
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare la ștergerea cursei: {ex.Message}", true);
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
			Draft.DataStart = cursaSelectata.DataStart;
			Draft.OraStartText = cursaSelectata.DataStart.ToString("HH:mm");
		}

		private void AfiseazaMesaj(string mesaj, bool esteEroare)
		{
			MesajStatus.Text = mesaj;
			MesajStatus.Foreground = esteEroare ? (SolidColorBrush)FindResource("MDNError") : new SolidColorBrush(Colors.LightGreen);
			MesajStatus.Visibility = Visibility.Visible;
		}
	}
}
