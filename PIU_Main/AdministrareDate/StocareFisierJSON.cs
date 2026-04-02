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