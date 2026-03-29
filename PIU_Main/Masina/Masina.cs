using Conducator;
using System.Diagnostics.CodeAnalysis;
using AdministrareDateModel = AdministrareDate.AdministrareDate;
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

		public static Masina AdaugaMasina()
		{
			Console.WriteLine("Introdu model: ");
			string model = Console.ReadLine() ?? string.Empty;
			Console.WriteLine("Introdu an: ");
			int.TryParse(Console.ReadLine(), out int an);
			CuloareMasina culoare = CitesteCuloare();
			OptiuniMasina optiuni = CitesteOptiuni();
			return new Masina(model, an, culoare, optiuni);
		}
		public static Masina? AfisareMasini(List<Masina> masini)
		{
			AdministrareDateModel.AfiseazaListaDateMembre(masini, m => $"{m.Model} - {m.An} - {m.Culoare} - {m.Optiuni} - {m.DistParcursa}");

			if (AdministrareDateModel.TryCautareFromClassListByMember(masini, "masini", m => m.Model, out List<Masina> rezultat))
			{
				AdministrareDateModel.AfiseazaListaDateMembre(rezultat, m => $"{m.Model} - {m.An} - {m.Culoare} - {m.Optiuni} - {m.DistParcursa}km");
				return SelectareMasina(rezultat);
			}
			return SelectareMasina(masini);
		}
		public static Masina? SelectareMasina(List<Masina> masini)
		{
			if (!AdministrareDateModel.TrySelectFromClassList(masini, "masina", out var masina))
			{
				return null;
			}
			return masina;
		}

		public void AdaugaConducator(ConducatorModel condNou)
		{
			_condDisp.Add(condNou);
		}

		private static CuloareMasina CitesteCuloare()
		{
			while (true)
			{
				Console.WriteLine("Alege culoare: 1.Rosu 2.Alb 3.Negru");
				if (int.TryParse(Console.ReadLine(), out int selectie) && Enum.IsDefined(typeof(CuloareMasina), selectie))
				{
					return (CuloareMasina)selectie;
				}
				Console.WriteLine("Valoare invalida. Incearca din nou.");
			}
		}

		private static OptiuniMasina CitesteOptiuni()
		{
			Console.WriteLine("Alege optiuni separate prin virgula (Enter pentru niciuna):");
			Console.WriteLine("1.AerConditionat 2.Navigatie 3.CutieAutomata 4.SenzoriParcare 5.CameraMarsarier");

			string? input = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(input))
			{
				return OptiuniMasina.Niciuna;
			}

			OptiuniMasina optiuni = OptiuniMasina.Niciuna;
			foreach (string token in input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			{
				if (!int.TryParse(token, out int selectie))
				{
					continue;
				}

				optiuni |= selectie 
				switch
				{
					1 => OptiuniMasina.AerConditionat,
					2 => OptiuniMasina.Navigatie,
					3 => OptiuniMasina.CutieAutomata,
					4 => OptiuniMasina.SenzoriParcare,
					5 => OptiuniMasina.CameraMarsarier,
					_ => OptiuniMasina.Niciuna
				};
			}

			return optiuni;
		}
	}
}

