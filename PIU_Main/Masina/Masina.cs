using System.Diagnostics.CodeAnalysis;
using ConducatorModel = Conducator.Conducator;

namespace Masina
{
	public class Masina
	{
		public required string Model { get; init; }
		public required int An { get; init; }
		public int DistParcursa { get; }
		private readonly List<ConducatorModel> _condDisp = new();

		[SetsRequiredMembers]
		public Masina(string model, int an)
		{
			Model = model;
			An = an;
		}

		public static Masina AdaugaMasina()
		{
			Console.WriteLine("Introdu model: ");
			string model = Console.ReadLine();
			Console.WriteLine("Introdu an: ");
			int.TryParse(Console.ReadLine(), out int an);
			return new Masina(model, an);
		}
		public void AdaugaConducator(ConducatorModel condNou)
		{
			_condDisp.Add(condNou);
		}
	}
}

