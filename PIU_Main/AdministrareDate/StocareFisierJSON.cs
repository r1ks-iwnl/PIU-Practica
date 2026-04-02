namespace AdministrareDate
{
	public class StocareFisierJSON<T> : IStocareData<T> where T : class
	{
		private ManipulareJSON manipulareJSON;

		public StocareFisierJSON(string numeFisier)
		{
			manipulareJSON = new ManipulareJSON(numeFisier);
		}

		public void AdaugaElement(T element)
		{
			List<T> elementeCurente = ObtineToateElementele();
			elementeCurente.Add(element);
			RescrieDate(elementeCurente);
		}

		public void EliminaElement(T element) //Clasele au override la Equal() pentru a facilita Remove()
		{
			List<T> elementeCurente = ObtineToateElementele();
			elementeCurente.Remove(element);
			RescrieDate(elementeCurente);
		}

		public void ActualizeazaElement(T elementModificat) //Prin crearea unui obiect nou dar cu IDul obiectului vechi
		{
			List<T> elementeCurente = ObtineToateElementele();
			int index = elementeCurente.IndexOf(elementModificat);
			if (index != -1)
			{
				elementeCurente[index] = elementModificat;
				RescrieDate(elementeCurente);
			}
		}

		public List<T> ObtineToateElementele()
		{
			return manipulareJSON.ExtrageDinFisier<T>();
		}

		public void RescrieDate(List<T> elemente)
		{
			manipulareJSON.SalveazaFisier<T>(elemente);
		}
	}
}