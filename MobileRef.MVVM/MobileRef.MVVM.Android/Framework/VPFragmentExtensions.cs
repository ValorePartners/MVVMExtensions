using System;
using Android.App;
using Android.Runtime;

namespace MobileRef.MVVM.Android
{

	[Preserve (AllMembers = true)]
	public static class VPFragmentExtensions
	{

		public static void NavigateActivity<T> (this Fragment context, bool finish = false)where T : Activity
		{
			context.Activity.NavigateActivity<T> (finish);
		}

		public static void NavigateActivity<T> (this Fragment context, string propName, object objValue, bool finish = false) where T : Activity
		{
			context.Activity.NavigateActivity<T> (propName, objValue, finish);
		}

		public static void NavigateActivity<T> (this Fragment context, Action<T> action, bool finish = false) where T : Activity
		{
			context.Activity.NavigateActivity<T> (action, finish);
		}

		public static void NavigateActivityByExtra<T> (this Fragment context, string putName, string putValue, bool finish = false) where T : Activity
		{
			context.Activity.NavigateActivityByExtra<T> (putName, putValue, finish);
		}

	}

	// THIS IS THE REACHABILITY PORTION OF THE CODE

}
