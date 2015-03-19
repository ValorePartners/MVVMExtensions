using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

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

		public static T[] ToArray<T> (this ICollection collection)where T:new()
		{
			var list = new List<T> ();
			foreach (var item in collection) {
				list.Add ((T)item);
			}
			return list.ToArray ();
		}
	}
}
