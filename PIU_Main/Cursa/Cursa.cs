using Conducator;
using System.Diagnostics.CodeAnalysis;
using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;

namespace Cursa
{
	public class Cursa
	{
		public required int Distanta { get; init; }
		public required MasinaModel Masina { get; init; }
		public required ConducatorModel Conducator { get; init; }
		private bool _finalizare; //Bazat pe compararea timpului sistemului salvat in txt

		[SetsRequiredMembers]
		public Cursa(int distanta, MasinaModel masina, ConducatorModel conducator)
		{
			Distanta = distanta;
			Masina = masina;
			Conducator = conducator;
		}

		public static Cursa AdaugaCursa(MasinaModel masina, ConducatorModel conducator)
		{
			if (masina == null || conducator == null)
			{
				Console.WriteLine("Conducator sau masina inexistenta");
			}
			Console.WriteLine("Introdu distanta: ");
			int.TryParse(Console.ReadLine(), out int distanta);
			return new Cursa(distanta, masina, conducator);
		}
		public static void AfisareCurse(List<Cursa> curse)
		{
			int i = 0;
			foreach (Cursa cursa in curse)
			{
				Console.WriteLine(i + ": " + cursa.Distanta + " " + cursa.Conducator.Nume + " " + cursa.Masina.Model);
				i++;
			}
		}
	}
}

//Afisare curse ce contin un conducator/o masina