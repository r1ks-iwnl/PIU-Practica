using Conducator;
using System.Diagnostics.CodeAnalysis;
using AdministrareDateModel = AdministrareDate.AdministrareDate;
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
		public static Masina? AfisareMasini(List<Masina> masini)
		{
			AdministrareDateModel.AfiseazaListaDateMembre(masini, m => $"{m.Model} - {m.An} - {m.DistParcursa}");

			if (AdministrareDateModel.TryCautareFromClassListByMember(masini, "masini", m => m.Model, out List<Masina> rezultat))
			{
				AdministrareDateModel.AfiseazaListaDateMembre(rezultat, m => $"{m.Model} - {m.An} - {m.DistParcursa}");
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
	}
}

