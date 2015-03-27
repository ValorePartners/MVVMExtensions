using System;
#if __UNIFIED__
using Foundation;
#else
using MonoTouch.Foundation;
#endif

namespace ServiceStackTextSampleiOS
{
	// Needed attribute to avoid linker remove code called dynamically
	// http://docs.xamarin.com/guides/ios/advanced_topics/linker/
	[Preserve (AllMembers = true)]
	public class RootObject
	{
		public Query Query { get; set; }
	}
}

