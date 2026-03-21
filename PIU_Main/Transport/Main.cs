using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;
using CursaModel = Cursa.Cursa;

namespace Main
{
	class Program
	{
		static void Main()
		{
			do
			{
				Console.WriteLine("A. Adauga masina");
				Console.WriteLine("B. Adauga conducator");
				Console.Write("C. Adauga cursa");

				string optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

				switch (optiune)
				{
					case "A":
						MasinaModel masina = MasinaModel.AdaugaMasina();
						break;
					case "B":
						ConducatorModel conducator = ConducatorModel.AdaugaConducator();
						break;
					case "C":
						CursaModel cursa = CursaModel.AdaugaCursa(masina, conducator);
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

