using System.Text.Json;

namespace AdministrareDate
{
	public class ManipulareJSON
	{
		private string numeFisier;

		public ManipulareJSON(string numeFisier)
		{
			this.numeFisier = numeFisier;

			if (!File.Exists(numeFisier))
			{
				File.WriteAllText(numeFisier, "[]");
			}
		}

		public void SalveazaFisier<T>(List<T> elemente)
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true
			};

			string jsonString = JsonSerializer.Serialize(elemente, options);
			File.WriteAllText(numeFisier, jsonString);
		}

		public List<T> ExtrageDinFisier<T>()
		{
			if (!File.Exists(numeFisier))
			{
				return new List<T>();
			}

			string jsonString = File.ReadAllText(numeFisier);

			if (string.IsNullOrWhiteSpace(jsonString))
			{
				return new List<T>();
			}

			try
			{
				var elemente = JsonSerializer.Deserialize<List<T>>(jsonString);
				return elemente ?? new List<T>();
			}
			catch (JsonException)
			{
				return new List<T>();
			}
		}
	}
}
