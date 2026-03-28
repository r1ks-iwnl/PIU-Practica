using System.Diagnostics.CodeAnalysis;
using AdministrareDateModel = AdministrareDate.AdministrareDate;

namespace Conducator
{
	public class Conducator
	{
		public required string Nume { get; init; }
		public required string DataNastere { get; init; }
		public required string DataAngajare { get; init; }
		private readonly List<string> _masiniConduse = new();
		public int DistCondusa { get; }

		[SetsRequiredMembers]
		public Conducator(string nume, string dataNastere, string dataAngajare)
		{
			Nume = nume;
			DataNastere = dataNastere;
			DataAngajare = dataAngajare;
		}

		public static Conducator AdaugaConducator()
		{
			Console.WriteLine("Introdu nume: ");
			string nume = Console.ReadLine();
			Console.WriteLine("Introdu data nastere: ");
			string dataNastere = Console.ReadLine();
			Console.WriteLine("Introdu data angajare: ");
			string dataAngajare = Console.ReadLine();
			return new Conducator(nume, dataNastere, dataAngajare);
		}

		public static Conducator? AfisareConducatori(List<Conducator> conducatori)
		{
			AdministrareDateModel.AfiseazaListaDateMembre(conducatori, c => $"{c.Nume} - {c.DataAngajare} - {c.DataNastere}");

			if (AdministrareDateModel.TryCautareFromClassListByMember(conducatori, "conducatori", c => c.Nume, out List<Conducator> rezultat))
			{
				AdministrareDateModel.AfiseazaListaDateMembre(rezultat, c => $"{c.Nume} - {c.DataAngajare} - {c.DataNastere}");
				return SelectareConducator(rezultat);
			}
			return SelectareConducator(conducatori);
		}
		public static Conducator? SelectareConducator(List<Conducator> conducatori)
		{
			if(!AdministrareDateModel.TrySelectFromClassList(conducatori, "conducator", out var conducator))
			{
				return null;
			}
			return conducator;
		}

		public void MasinaNoua(string modelMasina)
		{
			_masiniConduse.Add(modelMasina);
		}
	}
}

