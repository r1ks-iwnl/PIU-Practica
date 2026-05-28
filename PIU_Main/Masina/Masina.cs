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
		public Guid Id { get; init; } = Guid.NewGuid(); // adauga numar inmatriculare
		public required string Model { get; init; }
		public required int An { get; init; }
		public CuloareMasina Culoare { get; init; }
		public OptiuniMasina Optiuni { get; init; }
		[System.Text.Json.Serialization.JsonIgnore]
		public int DistParcursa { get; set; }
		public string NumarInmatriculare { get; init; } = string.Empty;
		private readonly List<ConducatorModel> _condDisp = new();

		[SetsRequiredMembers]
		public Masina(string model, int an, CuloareMasina culoare, OptiuniMasina optiuni, string numarInmatriculare = "")
		{
			Model = model;
			An = an;
			Culoare = culoare;
			Optiuni = optiuni;
			NumarInmatriculare = numarInmatriculare;
		}

		public void AdaugaConducator(ConducatorModel condNou)
		{
			_condDisp.Add(condNou);
		}

		public Masina CreeazaCopieModificata(string model, int an, CuloareMasina culoare, OptiuniMasina optiuni, string? numarInmatriculare = null)
		{
			string numar = numarInmatriculare ?? NumarInmatriculare;
			Masina masinaModificata = new Masina(model, an, culoare, optiuni, numar)
			{
				Id = Id
			};
			masinaModificata._condDisp.AddRange(_condDisp);
			return masinaModificata;
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

