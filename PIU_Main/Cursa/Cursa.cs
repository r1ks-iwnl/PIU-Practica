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
		public required DateTime DataStart { get; init; }

		public StareCursa Stare
		{
			get
			{
				if (DateTime.Now < DataStart)
					return StareCursa.Planificata;

				double ore = Distanta / 60.0;
				DateTime dataFinal = DataStart.AddHours(ore);

				if (DateTime.Now >= DataStart && DateTime.Now <= dataFinal)
					return StareCursa.InDesfasurare;

				return StareCursa.Finalizata;
			}
		}

		[SetsRequiredMembers]
		public Cursa(int distanta, MasinaModel masina, ConducatorModel conducator, DateTime dataStart)
		{
			Distanta = distanta;
			Masina = masina;
			Conducator = conducator;
			DataStart = dataStart;
		}

		[SetsRequiredMembers]
		private Cursa(Cursa sursa, int distanta, MasinaModel masina, ConducatorModel conducator, DateTime dataStart)
		{
			Id = sursa.Id;
			Distanta = distanta;
			Masina = masina;
			Conducator = conducator;
			DataStart = dataStart;
		}

		public Cursa CreeazaCopieModificata(int distanta, MasinaModel masina, ConducatorModel conducator, DateTime dataStart)
		{
			return new Cursa(this, distanta, masina, conducator, dataStart);
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
