using System.Diagnostics.CodeAnalysis;
using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;

namespace Cursa
{
	public class Cursa
	{
		public required int Distanta { get; init; }
		public required MasinaModel Vehicul { get; init; }
		public required ConducatorModel Sofer { get; init; }
		private bool _finalizare; //Bazat pe compararea timpului sistemului salvat in txt

		[SetsRequiredMembers]
		public Cursa(int distanta, MasinaModel vehicul, ConducatorModel sofer)
		{
			Distanta = distanta;
			Vehicul = vehicul;
			Sofer = sofer;
		}
	}
}

