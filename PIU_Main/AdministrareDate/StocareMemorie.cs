namespace AdministrareDate
{
	public class StocareMemorie<T> : IStocareData<T> where T : class
	{
		private List<T> _elemente = new List<T>();
		public void AdaugaElement(T element) => _elemente.Add(element);
		public List<T> ObtineToateElementele() => _elemente;
		public void RescrieDate(List<T> elemente) => _elemente = new List<T>(elemente);
	}
}
