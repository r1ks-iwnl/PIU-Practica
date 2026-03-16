using System.Diagnostics.CodeAnalysis;

namespace Main
{
	class Masina
	{
		public required string model { get; init; }
		public required int an { get; init; }
		public int distParcursa { get; }
		private List<Conducator> condDisp;

		[SetsRequiredMembers]
		public Masina(string model, int an)
		{
			this.model = model;
			this.an = an;
		}

		public void AdaugaConducator(Conducator condNou)
		{
			condDisp.Add(condNou);
		}
	}
}
