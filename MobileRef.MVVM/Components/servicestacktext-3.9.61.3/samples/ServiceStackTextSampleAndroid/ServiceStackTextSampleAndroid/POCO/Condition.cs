using System;
using Android.Runtime;

namespace ServiceStackTextSampleAndroid
{
	// Needed attribute to avoid linker remove code called dynamically
	// http://docs.xamarin.com/guides/ios/advanced_topics/linker/
	[Preserve (AllMembers = true)]
	public class Condition
	{
		public string Code { get; set; }
		public string Date { get; set; }
		public string Temp { get; set; }
		public string Text { get; set; }
	}
}

