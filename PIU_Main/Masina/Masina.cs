using System.Diagnostics.CodeAnalysis;
using ConducatorModel = Conducator.Conducator;

namespace Masina
{
	public enum CuloareMasina
	{
		Rosu = 1,
		Alb = 2,
		Negru = 3
	}

	[Flags]
	public enum OptiuniMasina
	{
		Niciuna = 0,
		AerConditionat = 1,
		Navigatie = 2,
		CutieAutomata = 4,
		SenzoriParcare = 8,
		CameraMarsarier = 16
	}

	public class Masina
	{
		public Guid Id { get; init; } = Guid.NewGuid();
		public required string Model { get; init; }
		public required int An { get; init; }
		public CuloareMasina Culoare { get; init; }
		public OptiuniMasina Optiuni { get; init; }
		public int DistParcursa { get; }
		private readonly List<ConducatorModel> _condDisp = new();

		[SetsRequiredMembers]
		public Masina(string model, int an, CuloareMasina culoare, OptiuniMasina optiuni)
		{
			Model = model;
			An = an;
			Culoare = culoare;
			Optiuni = optiuni;
		}

		public void AdaugaConducator(ConducatorModel condNou)
		{
			_condDisp.Add(condNou);
		}

		public override bool Equals(object? obj)
		{
			if (obj is Masina other)
			{
				return this.Id == other.Id;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}

