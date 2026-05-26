using System.Diagnostics.CodeAnalysis;

namespace Conducator
{
	public class Conducator
	{
		public Guid Id { get; init; } = Guid.NewGuid();
		public required string Nume { get; init; }
		public required string DataNastere { get; init; }
		public required string DataAngajare { get; init; }
		public string DataExpirarePermis { get; init; } = string.Empty;
		private readonly List<string> _masiniConduse = new();
		public int DistCondusa { get; }

		[SetsRequiredMembers]
		public Conducator(string nume, string dataNastere, string dataAngajare, string dataExpirarePermis = "")
		{
			Nume = nume;
			DataNastere = dataNastere;
			DataAngajare = dataAngajare;
			DataExpirarePermis = dataExpirarePermis;
		}

		public void MasinaNoua(string modelMasina)
		{
			_masiniConduse.Add(modelMasina);
		}

		public Conducator CreeazaCopieModificata(string nume, string dataNastere, string dataAngajare, string? dataExpirarePermis = null)
		{
			string dataExp = dataExpirarePermis ?? DataExpirarePermis;
			Conducator conducatorModificat = new Conducator(nume, dataNastere, dataAngajare, dataExp)
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
