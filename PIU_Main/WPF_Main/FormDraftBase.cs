using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WPF_Main
{
	public abstract class FormDraftBase : INotifyPropertyChanged, IDataErrorInfo
	{
		public virtual string Error => string.Empty;

		public abstract string this[string columnName] { get; }

		public event PropertyChangedEventHandler? PropertyChanged;

		protected bool SetField<T>(ref T field, T value, string propertyName)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
			{
				return false;
			}

			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}

		protected bool AreValid(params string[] propertyNames)
		{
			return propertyNames.All(propertyName => string.IsNullOrEmpty(this[propertyName]));
		}

		public string? GetFirstError(params string[] propertyNames)
		{
			foreach (string propertyName in propertyNames)
			{
				string message = this[propertyName];
				if (!string.IsNullOrEmpty(message))
				{
					return message;
				}
			}

			return null;
		}
	}
}