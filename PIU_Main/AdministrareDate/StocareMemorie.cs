namespace AdministrareDate
{
	public class StocareMemorie<T> : IStocareData<T> where T : class
	{
		private List<T> _elemente = new List<T>();
		public void AdaugaElement(T element) => _elemente.Add(element);
		//Returneaza o copie, nu o referinta
		public List<T> ObtineToateElementele() => new List<T>(_elemente);
		public void RescrieDate(List<T> elemente) => _elemente = new List<T>(elemente);
	}
}
