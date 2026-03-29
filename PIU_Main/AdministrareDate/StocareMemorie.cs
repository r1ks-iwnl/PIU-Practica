namespace AdministrareDate
{
	public class StocareMemorie<T> : IStocareData<T> where T : class
	{
		private List<T> _elemente = new List<T>();
		void AdaugaElement(T element);
		List<T> ObtineToateElementele();
		void RescrieDate(List<T> elemente);
	}
}
