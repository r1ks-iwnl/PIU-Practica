namespace Salariu
{
	class Program
	{
		static void Main()
		{
			double tarifOra;
			int ore;
			double salariu;

			Console.WriteLine("Tarif pe ora: ");
			tarifOra = double.Parse(Console.ReadLine());

			Console.WriteLine("Ore: ");
			ore = int.Parse(Console.ReadLine());

			salariu = ore * tarifOra;
			Console.WriteLine("Salariu: " + salariu);

			if (salariu >= 3000)
			{
				Console.WriteLine("Salariu mare");
			}
			else
			{
				Console.WriteLine("Ati lucrat prea putine ore pentru a avea un salariu mare!.");
			}
			Console.ReadKey();
		}
	}
}

