using System.Diagnostics.CodeAnalysis;

namespace Conducator
{
	public class Conducator
	{
		public Guid Id { get; init; } = Guid.NewGuid();
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

		public void MasinaNoua(string modelMasina)
		{
			_masiniConduse.Add(modelMasina);
		}

		public Conducator CreeazaCopieModificata(string nume, string dataNastere, string dataAngajare)
		{
			Conducator conducatorModificat = new Conducator(nume, dataNastere, dataAngajare)
			{
				Id = Id
			};
			conducatorModificat._masiniConduse.AddRange(_masiniConduse);
			return conducatorModificat;
		}

		public string MasiniConduseDisplay()
		{
			if (_masiniConduse == null || !_masiniConduse.Any())
				return "Niciuna";
			return string.Join(", ", _masiniConduse);
		}

		public override bool Equals(object? obj)
		{
			if (obj is Conducator other)
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
