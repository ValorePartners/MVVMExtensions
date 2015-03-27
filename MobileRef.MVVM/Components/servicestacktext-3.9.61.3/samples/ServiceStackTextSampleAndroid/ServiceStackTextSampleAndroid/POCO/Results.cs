using System;
using Android.Runtime;

namespace ServiceStackTextSampleAndroid
{
	// Needed attribute to avoid linker remove code called dynamically
	// http://docs.xamarin.com/guides/ios/advanced_topics/linker/
	[Preserve (AllMembers = true)]
	public class Results
	{
		public Channel Channel { get; set; }
	}
}

