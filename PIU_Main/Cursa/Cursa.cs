using Conducator;
using System.Diagnostics.CodeAnalysis;
using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;
using AdministrareDateModel = AdministrareDate.AdministrareDate;

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

		public static Cursa? AdaugaCursa(MasinaModel masina, ConducatorModel conducator)
		{
			if (masina == null || conducator == null)
			{
				Console.WriteLine("Conducator sau masina inexistenta");
				return null;
			}
			Console.WriteLine("Introdu distanta: ");
			int.TryParse(Console.ReadLine(), out int distanta);
			return new Cursa(distanta, masina, conducator);
		}
		public static Cursa? AfisareCurse(List<Cursa> curse)
		{
			int i = 0;
			foreach (Cursa cursa in curse)
			{
				Console.WriteLine(i + ": " + cursa.Distanta + " " + cursa.Conducator.Nume + " " + cursa.Masina.Model);
				i++;
			}
			return SelectareCursa(curse);
		}
		public static Cursa? SelectareCursa(List<Cursa> curse)
		{
			if (!AdministrareDateModel.TrySelectFromClassList(curse, "cursa", out var cursa))
			{
				return null;
			}
			return cursa;
		}
	}
}

//Afisare curse ce contin un conducator/o masina