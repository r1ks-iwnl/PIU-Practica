using System.Diagnostics.CodeAnalysis;

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

		public static void AfisareConducatori(List<Conducator> conducatori)
		{
			int i = 0;
			foreach(Conducator conducator in conducatori)
			{
				Console.WriteLine(i + ": " + conducator.Nume + " " + conducator.DataAngajare + " " + conducator.DataNastere);
				i++;
			}
		}
		public void MasinaNoua(string modelMasina)
		{
			_masiniConduse.Add(modelMasina);
		}
	}
}

