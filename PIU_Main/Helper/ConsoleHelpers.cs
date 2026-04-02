namespace Helper
{
	public static class ConsoleHelpers
	{
		public static void AfiseazaListaDateMembre<TItem>(IReadOnlyList<TItem> items, Func<TItem, string> formatter)
		{
			if (items.Count == 0)
			{
				Console.WriteLine("Lista este goala.");
				return;
			}

			for (int i = 0; i < items.Count; i++)
			{
				Console.WriteLine($"{i}: {formatter(items[i])}");
			}
		}

		public static bool TrySelectFromClassList<TItem>(IReadOnlyList<TItem> items, 
			string itemName, 
			out TItem? selected) where TItem : class
		{
			selected = null;

			if (items.Count == 0)
			{
				Console.WriteLine($"Nu exista {itemName} disponibil.");
				return false;
			}

			Console.WriteLine($"Selecteaza {itemName} dupa numar (Enter pentru a anula): ");
			string input = Console.ReadLine() ?? string.Empty;

			if (string.IsNullOrWhiteSpace(input))
			{
				return false;
			}

			if (!int.TryParse(input, out int index))
			{
				return false;
			}

			if (index < 0 || index >= items.Count)
			{
				return false;
			}

			selected = items[index];
			return true;
		}

		public static bool TryCautareFromClassListByMember<TItem>(
			IReadOnlyList<TItem> items, 
			string itemName, 
			Func<TItem, string> propertySelector, 
			out List<TItem> selectedItems) where TItem : class
		{
			selectedItems = new List<TItem>();

			if (items.Count == 0)
			{
				Console.WriteLine($"Nu exista {itemName} disponibili.");
				return false;
			}

			Console.Write($"Introdu textul cautat pentru {itemName} (Enter pentru a anula): ");
			string searchTerm = Console.ReadLine() ?? string.Empty;

			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				return false;
			}

			selectedItems = items.Where(item => 
				{
					string propertyValue = propertySelector(item);
					return propertyValue != null && 
						   propertyValue.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
				}).ToList();

			if (selectedItems.Count == 0)
			{
				Console.WriteLine($"Nu a fost gasit niciun {itemName} cu acest criteriu.");
				return false;
			}

			return true;
		}
	}
}