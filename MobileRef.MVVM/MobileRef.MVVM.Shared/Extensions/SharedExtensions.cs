using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace  MobileRef.MVVM.Shared
{
	public static class SharedExtensions
	{
		public static ObservableCollection<T> ToObservable<T> (this IEnumerable<T> list)
		{
			var collection = new ObservableCollection<T> ();
			foreach (var item in list)
				collection.Add (item);
			return collection;
		}
	}
}

