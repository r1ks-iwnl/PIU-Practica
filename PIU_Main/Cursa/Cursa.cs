using System.Diagnostics.CodeAnalysis;
using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;

namespace Cursa
{
	public class Cursa
	{
		public required int Distanta { get; init; }
		public required MasinaModel Masina { get; init; }
		public required ConducatorModel Sofer { get; init; }
		private bool _finalizare; //Bazat pe compararea timpului sistemului salvat in txt

		[SetsRequiredMembers]
		public Cursa(int distanta, MasinaModel masina, ConducatorModel sofer)
		{
			Distanta = distanta;
			Masina = masina;
			Sofer = sofer;
		}

		public Cursa AdaugaCursa(MasinaModel masina, ConducatorModel sofer)
		{
			Console.WriteLine("Introdu distanta: ");
			int.TryParse(Console.ReadLine(), out int distanta);
			return new Cursa(distanta, masina, sofer);
		}
	}
}

