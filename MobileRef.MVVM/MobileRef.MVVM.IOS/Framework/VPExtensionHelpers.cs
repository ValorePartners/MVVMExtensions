using System;
using System.Collections;
using UIKit;
using System.Collections.Generic;
using System.ComponentModel;
using Foundation;

namespace MobileRef.MVVM.IOS
{
	[System.AttributeUsage (System.AttributeTargets.Property)]
	public class RegisterEvent : System.Attribute
	{
		public string[] EventNames{ get; set; }

		public RegisterEvent (string eventName)
		{
			EventNames = new string[]{ eventName };
		}

		public RegisterEvent (string[] eventNames)
		{
			EventNames = eventNames;
		}
	}

	public interface IHandlers
	{
		void ViewEventHandler (object e, EventArgs args);

		void ControlsHandler (object sender, EventArgs args);
	}

	public class ControlUtil
	{
		public static List<string> ControlList = new List<string> (){ "UIButton" };

		public static void HookUIControl (object ctrl, IHandlers handler)
		{
			if (ctrl != null) {
				var contrlType = ctrl.GetType ().Name;
				switch (contrlType) {
				case "UIButton":
					((UIButton)ctrl).TouchUpInside += handler.ControlsHandler;
					break;
				}
			}
		}

		public static void UnHookUIControll (object ctrl, IHandlers handler)
		{
			if (ctrl != null) {
				var contrlType = ctrl.GetType ().Name;
				switch (contrlType) {
				case "UIButton":
					((UIButton)ctrl).TouchUpInside -= handler.ControlsHandler;
					break;
				}
			}
		}

	}

	public interface IViewCell<T>
	{
		void Bind (T item);
	}

	public interface IBaseSource
	{
		void UpdateCollection (ICollection collection);

		UITableView TableView{ get; set; }

		object SelectedItem{ get; set; }

		event EventHandler RowSelectedEvent;

		nint Tag{ get; set; }
	}

	public class RowSelectedEventArgs: EventArgs
	{

	}

	public class TableSource<T, K> :UITableViewSource, IBaseSource
		where T:class
		where K: IViewCell<T>
	{

		public event EventHandler RowSelectedEvent;

		public nint Tag{ get; set; }

		public T[] Items{ get; set; }

		public object SelectedItem{ get; set; }

		string cellIdentifier;

		public UITableView TableView { get; set; }

		public TableSource (T[] items, string cellId)
		{
			Items = items;
			cellIdentifier = cellId;
		}

		public void UpdateCollection (ICollection collection)
		{
			var List = new List<T> ();
			foreach (var item in collection) {
				List.Add ((T)item);
			}
			Items = List.ToArray ();
			TableView.ReloadData ();
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Items.Length;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (TableView == null)
				TableView = tableView;

			var cell = tableView.DequeueReusableCell (cellIdentifier);
			if (cell == null) {
				var cellName = typeof(K).Name;
				var obj = UINib.FromName (cellName, NSBundle.MainBundle).Instantiate (null, null) [0];
				cell = (UITableViewCell)obj;
			}
			((IViewCell<T>)cell).Bind (Items [indexPath.Row]);
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (TableView == null)
				TableView = tableView;

			SelectedItem = Items [indexPath.Row];
			if (RowSelectedEvent != null) {
				RowSelectedEvent (this, new RowSelectedEventArgs ());
			}
		}
	}


	public static class TConverter
	{
		public static T ChangeType<T> (object value)
		{
			return (T)ChangeType (typeof(T), value);
		}

		public static object ChangeType (Type t, object value)
		{
			var tc = TypeDescriptor.GetConverter (t);
			return tc.ConvertFrom (value);
		}

		public static void RegisterTypeConverter<T, TC> () where TC : TypeConverter
		{

			TypeDescriptor.AddAttributes (typeof(T), new TypeConverterAttribute (typeof(TC)));
		}
	}

	public class ValueConverter
	{
		public static object ChangeType (string typeName, string stringValue)
		{
			object returnValue = null;
			switch (typeName) {
			case "Int32":
				int intValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!int.TryParse (stringValue, out intValue))
						return null;
				} else {
					intValue = 0;
				}
				returnValue = intValue;
				break;
			case "Boolean":
				bool boolValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!bool.TryParse (stringValue, out boolValue))
						return null;
				} else {
					boolValue = false;
				}
				returnValue = boolValue;
				break;
			case "Single":
				float floatValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!float.TryParse (stringValue, out floatValue))
						return null;
				} else {
					floatValue = 0.0f;
				}
				returnValue = floatValue;
				break;
			case "Double":
				double doubleValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!double.TryParse (stringValue, out doubleValue))
						return null;
				} else {
					doubleValue = 0.0d;
				}
				returnValue = doubleValue;
				break;
			case "DateTime":
				DateTime dtValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!DateTime.TryParse (stringValue, out dtValue))
						return null;
				} else {
					return null;
				}
				returnValue = dtValue;
				break;
			default:
				returnValue = stringValue;
				break;
			}

			return returnValue;
		}
	}

}

