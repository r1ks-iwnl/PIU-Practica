using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;
using CursaModel = Cursa.Cursa;

namespace Main
{
	class Program
	{
		static void Main()
		{
			MasinaModel masina = null;
			ConducatorModel conducator = null;
			List<ConducatorModel> listaConducatori = new();
			List<MasinaModel> listaMasini = new();
			List<CursaModel> listaCursa = new();
			do
			{
				Console.WriteLine("A. Adauga masina");
				Console.WriteLine("B. Adauga conducator");
				Console.WriteLine("C. Adauga cursa");
				Console.WriteLine("D. Afisare conducatori/masini/curse");
				Console.WriteLine("E. Cauta masina (dupa model)");
				Console.WriteLine("F. Cauta conducator (dupa nume)");

				string optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

				switch (optiune)
				{
					case "A":
						masina = MasinaModel.AdaugaMasina();
						listaMasini.Add(masina);
						break;

					case "B":
						conducator = ConducatorModel.AdaugaConducator();
						listaConducatori.Add(conducator);
						break;

					case "C":
						CursaModel cursa = CursaModel.AdaugaCursa(masina, conducator);
						listaCursa.Add(cursa);
						break;

					case "D":
						Console.WriteLine("0. Afisare conducatori");
						Console.WriteLine("1. Afisare masini");
						Console.WriteLine("2. Afisare curse");
						int.TryParse(Console.ReadLine(), out int selectie);
						switch (selectie)
						{
							case 0:
								ConducatorModel.AfisareConducatori(listaConducatori);
								break;
							case 1:
								MasinaModel.AfisareMasini(listaMasini);
								break;
							case 2:
								CursaModel.AfisareCurse(listaCursa);
								break;
							default:
								Console.WriteLine("Optiune inexistenta");
								break;
						}
						break;

					case "E":
						Console.Write("Introdu modelul masinii cautate: ");
						string modelCautat = Console.ReadLine() ?? string.Empty;
						var masiniGasite = listaMasini.Where(m => m.Model.Contains(modelCautat, StringComparison.OrdinalIgnoreCase)).ToList();
						if (masiniGasite.Count > 0)
						{
							Console.WriteLine("Masini gasite:");
							MasinaModel.AfisareMasini(masiniGasite);
						}
						else
						{
							Console.WriteLine("Nu s-au gasit masini.");
						}
						break;

					case "F":
						Console.Write("Introdu numele conducatorului cautat: ");
						string numeCautat = Console.ReadLine() ?? string.Empty;
						var conducatoriGasiti = listaConducatori.Where(c => c.Nume.Contains(numeCautat, StringComparison.OrdinalIgnoreCase)).ToList();
						if (conducatoriGasiti.Count > 0)
						{
							Console.WriteLine("Conducatori gasiti:");
							ConducatorModel.AfisareConducatori(conducatoriGasiti);
						}
						else
						{
							Console.WriteLine("Nu s-au gasit conducatori.");
						}
						break;

					default:
						Console.WriteLine("Optiune inexistenta");
						break;
				}
			}
			while (true);
		}
	}
}

