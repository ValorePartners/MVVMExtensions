using System;
using Android.Runtime;
using Android.Views;
using System.Collections;
using Android.Widget;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.ComponentModel;
using System.Linq;

namespace MobileRef.MVVM.Android
{
	[Preserve (AllMembers = true)]
	public static class ActivityBridge
	{
		public static Action<object> FinishedLoading { get; set; }
	}

	[Preserve (AllMembers = true)]
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

	[Preserve (AllMembers = true)]
	public abstract class ViewHolder<T>:Java.Lang.Object
		where T:class
	{
		public abstract int CellResourceId{ get; }

		public View CreateView (LayoutInflater inflator)
		{
			return inflator.Inflate (CellResourceId, null);
		}

		public abstract void Init (View view);

		public abstract void Populate (T item);
	}

	public interface IBaseAdapter
	{
		void UpdateCollection (ICollection collection);

	}

	[Preserve (AllMembers = true)]
	public class HolderAdapter<T,K>:BaseAdapter<T>,IBaseAdapter
		where T: class
		where K : ViewHolder<T>, new()
	{
		public LayoutInflater Inflator { get; set; }

		public T[] Items{ get; set; }

		public HolderAdapter (LayoutInflater inflator, T[] items)
		{
			this.Inflator = inflator;
			this.Items = items;
		}

		#region IBaseAdapter implementation

		public void UpdateCollection (ICollection collection)
		{
			var List = new List<T> ();
			foreach (var item in collection) {
				List.Add ((T)item);
			}
			Items = List.ToArray ();
			this.NotifyDataSetChanged ();
		}

		#endregion

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			K holder;
			var item = Items [position];
			var view = convertView;
			if (view == null) {
				holder = new K ();
				view = holder.CreateView (Inflator);
				holder.Init (view);
				view.Tag = holder;
			} else {
				holder = view.Tag as K;
			}
			holder.Populate (item);
			return view;
		}

		public override int Count {
			get {
				return Items.Count ();
			}
		}

		public override T this [int index] {
			get {
				return Items [index];
			}
		}



	}

	[Preserve (AllMembers = true)]
	public class ControlUtil
	{
		//public static string[] ControlList = { "Button", "ListView", "Spinner", "ImageButton", "RadioGroup" };

		public static StringDictionary ActionDictionary = new StringDictionary () {
			{ "Button", "Click" },
			{ "ListView", "ItemClick" },
			{ "Spinner", "ItemSelected" },
			{ "ImageButton", "Click" },
			{ "RadioGroup", "CheckedChange" },
		};

		public static void HookUIControl (object ctrl, IHandlers handler)
		{
			if (ctrl != null) {
				var contrlType = ctrl.GetType ().Name;
				if (ActionDictionary.ContainsKey (contrlType)) {
					var eventName = ActionDictionary [contrlType];
					var eventInstance = ctrl.GetType ().GetEvent (eventName);
					if (eventInstance != null) {
						var tDelegate = eventInstance.EventHandlerType;
						var miHandler = handler.GetType ().GetMethod ("ControlsHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
						if (miHandler != null) {
							var addHandler = eventInstance.GetAddMethod ();
							var d = Delegate.CreateDelegate (tDelegate, handler, miHandler);
							System.Object[] addHandlerArgs = { d };
							addHandler.Invoke (ctrl, addHandlerArgs);
						}
					}
				}
			}

		}

		public static void UnHookUIControll (object ctrl, IHandlers handler)
		{
			if (ctrl != null) {
				var contrlType = ctrl.GetType ().Name;
				if (ActionDictionary.ContainsKey (contrlType)) {
					var eventName = ActionDictionary [contrlType];
					var eventInstance = ctrl.GetType ().GetEvent (eventName);
					if (eventInstance != null) {
						var tDelegate = eventInstance.EventHandlerType;
						var miHandler = handler.GetType ().GetMethod ("ControlsHandler", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
						if (miHandler != null) {
							var addHandler = eventInstance.GetRemoveMethod ();
							var d = Delegate.CreateDelegate (tDelegate, handler, miHandler);
							System.Object[] addHandlerArgs = { d };
							addHandler.Invoke (ctrl, addHandlerArgs);
						}
					}
				}
			}

		}

		public static int GetResourceId (string controlName)
		{
			var rType = typeof(Resource.Id); 
			FieldInfo field = null;
			foreach (var obj in rType.GetFields()) {
				if (obj.Name.Equals (controlName)) {
					field = obj;
					return (int)field.GetValue (controlName);
				}
			}
			return 0;
		}

	}

	[Preserve (AllMembers = true)]
	public interface IHandlers
	{
		void ViewEventHandler (object e, EventArgs args);

		void ControlsHandler (object sender, EventArgs args);
	}

	public static class TConverter
	{
		public static T ChangeType<T> (object value)
		{
			return (T)ChangeType (typeof(T), value);
		}

		public static object ChangeType (Type t, object value)
		{
			try {
				var tc = TypeDescriptor.GetConverter (t);
				return tc.ConvertFrom (value);
			} catch {
				return null;
			}
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
			object obj = null;
			switch (typeName) {
			case "Int32":
				int intValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!int.TryParse (stringValue, out intValue))
						obj = null;
				} else {
					intValue = 0;
				}
				obj = intValue;
				break;
			case "Boolean":
				bool boolValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!bool.TryParse (stringValue, out boolValue))
						return null;
				} else {
					boolValue = false;
				}
				obj = boolValue;
				break;
			case "Single":
				float floatValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!float.TryParse (stringValue, out floatValue))
						obj = null;
				} else {
					floatValue = 0.0f;
				}
				obj = floatValue;
				break;
			case "Double":
				double doubleValue;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!double.TryParse (stringValue, out doubleValue))
						obj = null;
				} else {
					doubleValue = 0.0d;
				}
				obj = doubleValue;
				break;
			case "DateTime":
				DateTime dtValue = DateTime.Now;
				if (!string.IsNullOrEmpty (stringValue)) {
					if (!DateTime.TryParse (stringValue, out dtValue))
						obj = null;
					else {
						obj = dtValue;
					}
				} else {
					obj = null;
				}
				break;
			default:
				obj = stringValue;
				break;
			}
			return obj;	
		}
	}
}

