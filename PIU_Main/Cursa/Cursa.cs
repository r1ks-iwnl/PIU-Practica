using Masina;
using System.Diagnostics.CodeAnalysis;
using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;

namespace Cursa
{
	public enum StareCursa
	{
		Planificata,
		InDesfasurare,
		Finalizata,
		Anulata
	}
	public class Cursa
	{
		public Guid Id { get; init; } = Guid.NewGuid();
		public required int Distanta { get; init; }
		public required MasinaModel Masina { get; init; }
		public required ConducatorModel Conducator { get; init; }
		public StareCursa Stare { get; }

		[SetsRequiredMembers]
		public Cursa(int distanta, MasinaModel masina, ConducatorModel conducator)
		{
			Distanta = distanta;
			Masina = masina;
			Conducator = conducator;
			Stare = StareCursa.Planificata;
		}

		public override bool Equals(object? obj)
		{
			if (obj is Cursa other)
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

//Afisare curse ce contin un conducator/o masina
//Selectare conducator/masina la initializare
