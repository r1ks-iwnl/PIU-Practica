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
using CursaModel = Cursa.Cursa;
using AdministrareDate;
using ComponentModel = System.ComponentModel;

namespace WPF_Main
{
	public class ConducatorFormDraft : ComponentModel.INotifyPropertyChanged, ComponentModel.IDataErrorInfo
	{
		private const int MIN_LUNGIME_NUME = 2;
		private const int VARSTA_MINIMA = 18;

		private string _numeText = "";
		public string NumeText
		{
			get => _numeText;
			set { _numeText = value; PropertyChanged?.Invoke(this, new ComponentModel.PropertyChangedEventArgs(nameof(NumeText))); }
		}

		private string _prenumeText = "";
		public string PrenumeText
		{
			get => _prenumeText;
			set { _prenumeText = value; PropertyChanged?.Invoke(this, new ComponentModel.PropertyChangedEventArgs(nameof(PrenumeText))); }
		}

		private DateTime? _dataNastere;
		public DateTime? DataNastere
		{
			get => _dataNastere;
			set { _dataNastere = value; PropertyChanged?.Invoke(this, new ComponentModel.PropertyChangedEventArgs(nameof(DataNastere))); }
		}

		private DateTime? _dataAngajare;
		public DateTime? DataAngajare
		{
			get => _dataAngajare;
			set { _dataAngajare = value; PropertyChanged?.Invoke(this, new ComponentModel.PropertyChangedEventArgs(nameof(DataAngajare))); }
		}

		public string Error => null;

		public string this[string columnName]
		{
			get
			{
				if (columnName == nameof(NumeText))
				{
					if (string.IsNullOrWhiteSpace(NumeText)) return "Numele este obligatoriu.";
					if (NumeText.Trim().Length < MIN_LUNGIME_NUME) return $"Numele trebuie să aibă cel puțin {MIN_LUNGIME_NUME} caractere.";
				}
				if (columnName == nameof(PrenumeText))
				{
					if (string.IsNullOrWhiteSpace(PrenumeText)) return "Prenumele este obligatoriu.";
					if (PrenumeText.Trim().Length < MIN_LUNGIME_NUME) return $"Prenumele trebuie să aibă cel puțin {MIN_LUNGIME_NUME} caractere.";
				}
				if (columnName == nameof(DataNastere))
				{
					if (!DataNastere.HasValue) return "Selectați data nașterii.";

					int varsta = DateTime.Today.Year - DataNastere.Value.Year;
					if (DataNastere.Value.Date > DateTime.Today.AddYears(-varsta)) varsta--;

					if (varsta < VARSTA_MINIMA) return $"Conducătorul trebuie să aibă cel puțin {VARSTA_MINIMA} ani.";
				}
				if (columnName == nameof(DataAngajare))
				{
					if (!DataAngajare.HasValue) return "Selectați data angajării.";
					if (DataAngajare.Value > DateTime.Today) return "Data angajării nu poate fi în viitor.";

					if (DataNastere.HasValue)
					{
						if (DataAngajare.Value < DataNastere.Value.AddYears(VARSTA_MINIMA))
							return $"Angajarea necesită împlinirea vârstei de {VARSTA_MINIMA} ani (la data de {DataNastere.Value.AddYears(VARSTA_MINIMA):d}).";
					}
				}
				return null;
			}
		}

		public bool IsValid => string.IsNullOrEmpty(this[nameof(NumeText)]) && string.IsNullOrEmpty(this[nameof(PrenumeText)]) && string.IsNullOrEmpty(this[nameof(DataNastere)]) && string.IsNullOrEmpty(this[nameof(DataAngajare)]);

		public event ComponentModel.PropertyChangedEventHandler PropertyChanged;
	}

	/// <summary>
	/// Interaction logic for ConducatoriView.xaml
	/// </summary>
	public partial class ConducatoriView : UserControl
	{
		private static ConducatorModel? conducatorCurent = null;
		private static IStocareData<ConducatorModel> adminConducatori = StocareFactory.GetAdministratorStocare<ConducatorModel>();
		private static IStocareData<CursaModel> adminCurse = StocareFactory.GetAdministratorStocare<CursaModel>();

		private ObservableCollection<ConducatorModel> conducatoriList;
		private ConducatorModel? conducatorSelectat;
		public ConducatorFormDraft Draft { get; set; } = new ConducatorFormDraft();

		public ConducatoriView()
		{
			InitializeComponent();
			DataContext = Draft;
			IncarcaDate();
		}

		private void IncarcaDate()
		{
			// Incarca datele prin Factory
			var dateExistente = adminConducatori.ObtineToateElementele();
			conducatoriList = new ObservableCollection<ConducatorModel>(dateExistente ?? new List<ConducatorModel>());

			// Ataseaza lista la datagrid
			ConducatoriDataGrid.ItemsSource = conducatoriList;

			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ConducatoriDataGrid.ItemsSource);
			view.Filter = ConducatorFiltru;
		}

		private void tbCautareNume_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(ConducatoriDataGrid.ItemsSource).Refresh();
		}

		private bool ConducatorFiltru(object item)
		{
			if (string.IsNullOrEmpty(tbCautareNume.Text))
				return true;

			var cond = (ConducatorModel)item;
			return cond.Nume != null && cond.Nume.Contains(tbCautareNume.Text, StringComparison.OrdinalIgnoreCase);
		}

		private void btnAdauga_Click(object sender, RoutedEventArgs e)
		{
			tbMesajStatus.Visibility = Visibility.Collapsed;

			if (!Draft.IsValid)
			{
				AfiseazaMesaj("Vă rugăm să corectați erorile de pe formular.", true);
				return;
			}

			try
			{
				string numeComplet = $"{Draft.NumeText.Trim()} {Draft.PrenumeText.Trim()}";
				string dataNastere = Draft.DataNastere?.ToString("d") ?? string.Empty;
				string dataAngajare = Draft.DataAngajare?.ToString("d") ?? string.Empty;

				ConducatorModel nouConducator = new ConducatorModel(numeComplet, dataNastere, dataAngajare);
				adminConducatori.AdaugaElement(nouConducator);

				IncarcaDate();

				AfiseazaMesaj("Conducătorul a fost adăugat cu succes!", false);

				Draft.NumeText = string.Empty;
				Draft.PrenumeText = string.Empty;
				Draft.DataNastere = null;
				Draft.DataAngajare = null;
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare la adăugarea conducătorului: {ex.Message}", true);
			}
		}

		private void btnModifica_Click(object sender, RoutedEventArgs e)
		{
			tbMesajStatus.Visibility = Visibility.Collapsed;

			if (conducatorSelectat == null)
			{
				AfiseazaMesaj("Selectați un conducător din listă pentru modificare.", true);
				return;
			}

			if (!Draft.IsValid)
			{
				AfiseazaMesaj("Vă rugăm să corectați erorile de pe formular.", true);
				return;
			}

			try
			{
				string numeComplet = $"{Draft.NumeText.Trim()} {Draft.PrenumeText.Trim()}";
				string dataNastere = Draft.DataNastere?.ToString("d") ?? string.Empty;
				string dataAngajare = Draft.DataAngajare?.ToString("d") ?? string.Empty;

				ConducatorModel conducatorModificat = conducatorSelectat.CreeazaCopieModificata(numeComplet, dataNastere, dataAngajare);
				adminConducatori.ActualizeazaElement(conducatorModificat);
				IncarcaDate();
				conducatorSelectat = conducatorModificat;
				ConducatoriDataGrid.SelectedItem = conducatoriList.FirstOrDefault(c => c.Id == conducatorModificat.Id);

				AfiseazaMesaj("Conducătorul a fost modificat cu succes!", false);
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare la modificarea conducătorului: {ex.Message}", true);
			}
		}

		private void btnSterge_Click(object sender, RoutedEventArgs e)
		{
			tbMesajStatus.Visibility = Visibility.Collapsed;

			if (conducatorSelectat == null)
			{
				AfiseazaMesaj("Selectați un conducător din listă pentru ștergere.", true);
				return;
			}

			var curse = adminCurse.ObtineToateElementele();
			if (curse.Any(c => c.Conducator != null && c.Conducator.Id == conducatorSelectat.Id))
			{
				AfiseazaMesaj("Nu se poate șterge conducătorul deoarece există curse asociate.", true);
				return;
			}

			MessageBoxResult confirmare = MessageBox.Show(
				$"Sigur doriți să ștergeți conducătorul '{conducatorSelectat.Nume}'?",
				"Confirmare ștergere",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (confirmare != MessageBoxResult.Yes)
			{
				return;
			}

			try
			{
				adminConducatori.EliminaElement(conducatorSelectat);
				IncarcaDate();
				conducatorSelectat = null;
				ConducatoriDataGrid.SelectedItem = null;

				Draft.NumeText = string.Empty;
				Draft.PrenumeText = string.Empty;
				Draft.DataNastere = null;
				Draft.DataAngajare = null;

				AfiseazaMesaj("Conducătorul a fost șters cu succes!", false);
			}
			catch (Exception ex)
			{
				AfiseazaMesaj($"Eroare la ștergerea conducătorului: {ex.Message}", true);
			}
		}

		private void ConducatoriDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			conducatorSelectat = ConducatoriDataGrid.SelectedItem as ConducatorModel;
			if (conducatorSelectat == null)
			{
				return;
			}

			(string nume, string prenume) = DesparteNume(conducatorSelectat.Nume);
			Draft.NumeText = nume;
			Draft.PrenumeText = prenume;
			Draft.DataNastere = ParseDate(conducatorSelectat.DataNastere);
			Draft.DataAngajare = ParseDate(conducatorSelectat.DataAngajare);
		}

		private static (string Nume, string Prenume) DesparteNume(string numeComplet)
		{
			if (string.IsNullOrWhiteSpace(numeComplet))
			{
				return (string.Empty, string.Empty);
			}

			string[] parti = numeComplet.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
			if (parti.Length == 1)
			{
				return (parti[0], string.Empty);
			}

			return (parti[0], parti[1]);
		}

		private static DateTime? ParseDate(string? value)
		{
			if (DateTime.TryParse(value, out DateTime parsedDate))
			{
				return parsedDate;
			}

			return null;
		}

		private void AfiseazaMesaj(string mesaj, bool esteEroare)
		{
			tbMesajStatus.Text = mesaj;
			tbMesajStatus.Foreground = esteEroare ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.LightGreen);
			tbMesajStatus.Visibility = Visibility.Visible;
		}
	}
}
