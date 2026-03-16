using System.Diagnostics.CodeAnalysis;

namespace Main
{
	class Cursa
	{
		public required int distanta { get; init; }
		public required Masina vehicul { get; init; }
		public required Conducator sofer { get; init; }
		private bool finalizare; //Bazat pe compararea timpului sistemului salvat in txt

		[SetsRequiredMembers]
		public Cursa(int distanta, Masina vehicul, Conducator sofer)
		{
			this.distanta = distanta;
			this.vehicul = vehicul;
			this.sofer = sofer;
		}
	}
}
