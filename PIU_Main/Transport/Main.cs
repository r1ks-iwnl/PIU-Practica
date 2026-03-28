using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;
using CursaModel = Cursa.Cursa;

namespace Main
{
	class Program
	{
		static void Main()
		{
			MasinaModel? masina = null;
			ConducatorModel? conducator = null;
			CursaModel? cursa = null;
			List<ConducatorModel> listaConducatori = new();
			List<MasinaModel> listaMasini = new();
			List<CursaModel> listaCursa = new();
			do
			{
				Console.WriteLine("A. Adauga masina");
				Console.WriteLine("B. Adauga conducator");
				Console.WriteLine("C. Adauga cursa");
				Console.WriteLine("D. Afisare conducatori/masini/curse");

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
						cursa = CursaModel.AdaugaCursa(masina, conducator);
						listaCursa.Add(cursa);
						break;

					case "D":
						Console.WriteLine("0. Afisare/Cautare/Selectare conducatori");
						Console.WriteLine("1. Afisare/Cautare/Selectare masini");
						Console.WriteLine("2. Afisare/Cautare/Selectare curse");
						int.TryParse(Console.ReadLine(), out int selectie);
						switch (selectie)
						{
							case 0:
								conducator = ConducatorModel.AfisareConducatori(listaConducatori);
								break;
							case 1:
								masina = MasinaModel.AfisareMasini(listaMasini);
								break;
							case 2:
								cursa = CursaModel.AfisareCurse(listaCursa);
								break;
							default:
								Console.WriteLine("Optiune inexistenta");
								break;
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
