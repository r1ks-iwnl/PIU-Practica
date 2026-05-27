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
		public int DistCondusa { get; }

		[SetsRequiredMembers]
		public Conducator(string nume, string dataNastere, string dataAngajare, string dataExpirarePermis = "")
		{
			Nume = nume;
			DataNastere = dataNastere;
			DataAngajare = dataAngajare;
			DataExpirarePermis = dataExpirarePermis;
		}

		public Conducator CreeazaCopieModificata(string nume, string dataNastere, string dataAngajare, string? dataExpirarePermis = null)
		{
			string dataExp = dataExpirarePermis ?? DataExpirarePermis;
			Conducator conducatorModificat = new Conducator(nume, dataNastere, dataAngajare, dataExp)
			{
				Id = Id
			};
			return conducatorModificat;
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
