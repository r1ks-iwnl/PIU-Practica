namespace Main
{
	class Program
	{
		static void Main()
		{
			Conducator c = new Conducator("Asd Wes", "16.03.2006", "16.03.2026");
			Console.WriteLine(c.nume + " " + c.dataNastere + " " + c.dataAngajare);
			Console.ReadKey();
		}
	}
}
