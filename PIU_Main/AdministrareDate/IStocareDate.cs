using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;
using CursaModel = Cursa.Cursa;
using System.Text.Json.Nodes;

namespace AdministrareDate
{
	public interface IStocareData<T> where T : class
	{
		void AdaugaElement(T element);
		List<T> ObtineToateElementele();
		void RescrieDate(List<T> elemente);
	}
	public static class StocareFactory
	{
		public static IStocareData<T> GetAdministratorStocare<T>() where T : class
		{
			string formatSalvare = "memory";
			string numeFisier = "Date";
			string configPath = "config.json";

			if(File.Exists(configPath))
			{
				try
				{
					string jsonString = File.ReadAllText(configPath);
					JsonNode configNode = JsonNode.Parse(jsonString);
					if (configNode != null)
					{
						formatSalvare = (string)configNode["FormatSalvare"] ?? formatSalvare;
						numeFisier = (string)configNode["NumeFisier"] ?? numeFisier;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Eroare citind config: {ex.Message}");
				}
			}
			if(formatSalvare != null)
			{
				string fileNameForType = $"{numeFisier}_{typeof(T).Name}.json";

				switch (formatSalvare.ToLower())
				{
					default:
					case "memorie":
						return new StocareMemorie<T>();
					case "json":
						return new StocareFisierJSON<T>(fileNameForType);
				}
			}
			return new StocareMemorie<T>();
		}
	}
}
