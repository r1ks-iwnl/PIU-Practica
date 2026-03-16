using System.Diagnostics.CodeAnalysis;

namespace Main
{
	class Conducator
	{
		public required string nume { get; init;}
		public required string dataNastere { get; init;}
		public required string dataAngajare { get; init;}
		private List<Masina> masiniConduse;
		public int distCondusa { get; }

		[SetsRequiredMembers]
		public Conducator(string nume, string dataNastere, string dataAngajare)
		{
			this.nume = nume;
			this.dataNastere = dataNastere;
			this.dataAngajare = dataAngajare;
		}

		public void MasinaNoua(Masina masinaNoua)
		{
			masiniConduse.Add(masinaNoua);
		}
	}
}
