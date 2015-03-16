using System;
using System.Collections.Generic;
#if __ANDROID__
using Android.Runtime;
#elif __IOS__
using Foundation;
#endif
namespace MobileRef.MVVM.Shared
{
	[Preserve(AllMembers=true)]
	public class Person
	{
		public int Id{ get; set; }

		public string FirstName{ get; set; }

		public string LastName{ get; set; }

		public string UserName{ get; set; }

		public string Password{ get; set; }

		public List<Contact> Addresses{ get; set; }

		public Person ()
		{
			Addresses = new List<Contact> ();
		}
	}

	[Preserve(AllMembers=true)]
	public class Contact
	{
		public int Id{ get; set; }

		public int AccountId{ get; set; }

		public string Address{ get; set; }

		public string City{ get; set; }

		public string State{ get; set; }

		public int Zip{ get; set; }
	}
}

