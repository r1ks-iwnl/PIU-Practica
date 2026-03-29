using Helper;
using Masina;
using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;
using CursaModel = Cursa.Cursa;

namespace Main
{
	public static class UserInterface
	{
		private static List<ConducatorModel> listaConducatori = new();
		private static List<MasinaModel> listaMasini = new();
		private static List<CursaModel> listaCursa = new();
		private static MasinaModel? masinaCurenta = null;
		private static ConducatorModel? conducatorCurent = null;
		private static CursaModel? cursaCurenta = null;

		public static void StartMenu()
		{
			do
			{
				Console.WriteLine("\n--- MENIU PRINCIPAL ---");
				Console.WriteLine("A. Adauga masina");
				Console.WriteLine("B. Adauga conducator");
				Console.WriteLine("C. Adauga cursa");
				Console.WriteLine("D. Afisare conducatori/masini/curse");
				Console.WriteLine("X. Iesire");
				Console.Write("Alege o optiune: ");

				string optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

				switch (optiune)
				{
					case "A":
						masinaCurenta = AdaugaMasina();
						listaMasini.Add(masinaCurenta);
						break;

					case "B":
						conducatorCurent = AdaugaConducator();
						listaConducatori.Add(conducatorCurent);
						break;

					case "C":
						cursaCurenta = AdaugaCursa(masinaCurenta, conducatorCurent);
						if (cursaCurenta != null)
						{
							listaCursa.Add(cursaCurenta);
						}
						break;

					case "D":
						MeniuAfisare();
						break;

					case "X":
						Console.WriteLine("La revedere!");
						return;

					default:
						Console.WriteLine("Optiune inexistenta");
						break;
				}
			}
			while (true);
		}

		private static void MeniuAfisare()
		{
			Console.WriteLine("\n--- MENIU AFISARE ---");
			Console.WriteLine("0. Afisare/Cautare/Selectare conducatori");
			Console.WriteLine("1. Afisare/Cautare/Selectare masini");
			Console.WriteLine("2. Afisare/Cautare/Selectare curse");
			Console.Write("Alege o optiune: ");
			
			if (!int.TryParse(Console.ReadLine(), out int selectie))
			{
				Console.WriteLine("Optiune invalida.");
				return;
			}

			switch (selectie)
			{
				case 0:
					conducatorCurent = AfisareConducatori(listaConducatori) ?? conducatorCurent;
					break;
				case 1:
					masinaCurenta = AfisareMasini(listaMasini) ?? masinaCurenta;
					break;
				case 2:
					cursaCurenta = AfisareCurse(listaCursa) ?? cursaCurenta;
					break;
				default:
					Console.WriteLine("Optiune inexistenta");
					break;
			}
		}

		// ------------------------- MASINA UI -------------------------
		public static MasinaModel AdaugaMasina()
		{
			Console.WriteLine("Introdu model: ");
			string model = Console.ReadLine() ?? string.Empty;
			Console.WriteLine("Introdu an: ");
			int.TryParse(Console.ReadLine(), out int an);
			CuloareMasina culoare = CitesteCuloare();
			OptiuniMasina optiuni = CitesteOptiuni();
			return new MasinaModel(model, an, culoare, optiuni);
		}

		public static MasinaModel? AfisareMasini(List<MasinaModel> masini)
		{
			ConsoleHelpers.AfiseazaListaDateMembre(masini, m => $"{m.Model} - {m.An} - {m.Culoare} - {m.Optiuni} - {m.DistParcursa}km");

			if (ConsoleHelpers.TryCautareFromClassListByMember(masini, "masini", m => m.Model, out List<MasinaModel> rezultat))
			{
				ConsoleHelpers.AfiseazaListaDateMembre(rezultat, m => $"{m.Model} - {m.An} - {m.Culoare} - {m.Optiuni} - {m.DistParcursa}km");
				return SelectareMasina(rezultat);
			}
			return SelectareMasina(masini);
		}

		public static MasinaModel? SelectareMasina(List<MasinaModel> masini)
		{
			if (!ConsoleHelpers.TrySelectFromClassList(masini, "masina", out var masina))
			{
				return null;
			}
			return masina;
		}

		private static CuloareMasina CitesteCuloare()
		{
			while (true)
			{
				Console.WriteLine("Alege culoare: 1.Rosu 2.Alb 3.Negru");
				if (int.TryParse(Console.ReadLine(), out int selectie) && Enum.IsDefined(typeof(CuloareMasina), selectie))
				{
					return (CuloareMasina)selectie;
				}
				Console.WriteLine("Valoare invalida. Incearca din nou.");
			}
		}

		private static OptiuniMasina CitesteOptiuni()
		{
			Console.WriteLine("Alege optiuni separate prin virgula (Enter pentru niciuna):");
			Console.WriteLine("1.AerConditionat 2.Navigatie 3.CutieAutomata 4.SenzoriParcare 5.CameraMarsarier");

			string? input = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(input))
			{
				return OptiuniMasina.Niciuna;
			}

			OptiuniMasina optiuni = OptiuniMasina.Niciuna;
			foreach (string token in input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			{
				if (!int.TryParse(token, out int selectie))
				{
					continue;
				}

				optiuni |= selectie 
				switch
				{
					1 => OptiuniMasina.AerConditionat,
					2 => OptiuniMasina.Navigatie,
					3 => OptiuniMasina.CutieAutomata,
					4 => OptiuniMasina.SenzoriParcare,
					5 => OptiuniMasina.CameraMarsarier,
					_ => OptiuniMasina.Niciuna
				};
			}

			return optiuni;
		}

		// ------------------------- CONDUCATOR UI -------------------------
		public static ConducatorModel AdaugaConducator()
		{
			Console.WriteLine("Introdu nume: ");
			string nume = Console.ReadLine() ?? string.Empty;
			Console.WriteLine("Introdu data nastere: ");
			string dataNastere = Console.ReadLine() ?? string.Empty;
			Console.WriteLine("Introdu data angajare: ");
			string dataAngajare = Console.ReadLine() ?? string.Empty;
			return new ConducatorModel(nume, dataNastere, dataAngajare);
		}

		public static ConducatorModel? AfisareConducatori(List<ConducatorModel> conducatori)
		{
			ConsoleHelpers.AfiseazaListaDateMembre(conducatori, c => $"{c.Nume} - {c.DataAngajare} - {c.DataNastere}");

			if (ConsoleHelpers.TryCautareFromClassListByMember(conducatori, "conducatori", c => c.Nume, out List<ConducatorModel> rezultat))
			{
				ConsoleHelpers.AfiseazaListaDateMembre(rezultat, c => $"{c.Nume} - {c.DataAngajare} - {c.DataNastere}");
				return SelectareConducator(rezultat);
			}
			return SelectareConducator(conducatori);
		}

		public static ConducatorModel? SelectareConducator(List<ConducatorModel> conducatori)
		{
			if(!ConsoleHelpers.TrySelectFromClassList(conducatori, "conducator", out var conducator))
			{
				return null;
			}
			return conducator;
		}

		// ------------------------- CURSA UI -------------------------
		public static CursaModel? AdaugaCursa(MasinaModel? masina, ConducatorModel? conducator)
		{
			if (masina == null || conducator == null)
			{
				Console.WriteLine("Conducator sau masina inexistenta");
				return null;
			}
			Console.WriteLine("Introdu distanta: ");
			int.TryParse(Console.ReadLine(), out int distanta);
			return new CursaModel(distanta, masina, conducator);
		}

		public static CursaModel? AfisareCurse(List<CursaModel> curse)
		{
			int i = 0;
			foreach (CursaModel cursa in curse)
			{
				Console.WriteLine(i + ": " + cursa.Distanta + " " + cursa.Conducator.Nume + " " + cursa.Masina.Model);
				i++;
			}
			return SelectareCursa(curse);
		}

		public static CursaModel? SelectareCursa(List<CursaModel> curse)
		{
			if (!ConsoleHelpers.TrySelectFromClassList(curse, "cursa", out var cursa))
			{
				return null;
			}
			return cursa;
		}
	}
}
