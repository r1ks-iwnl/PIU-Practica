namespace AdministrareDate
{
	public class StocareMemorie<T> : IStocareData<T> where T : class
	{
		private List<T> _elemente = new List<T>();
		public void AdaugaElement(T element) => _elemente.Add(element);
		public void EliminaElement(T element) => _elemente.Remove(element);
		public List<T> ObtineToateElementele() => new List<T>(_elemente); //Returneaza o copie, nu o referinta
		public void RescrieDate(List<T> elemente) => _elemente = new List<T>(elemente);
	}
}
