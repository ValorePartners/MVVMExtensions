package md5520d6fce9e82da752c437f890a0bd7ba;


public class PeopleViewHolder
	extends md5520d6fce9e82da752c437f890a0bd7ba.ViewHolder_1
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MobileRef.MVVM.Android.PeopleViewHolder, MobileRef.MVVM.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PeopleViewHolder.class, __md_methods);
	}


	public PeopleViewHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PeopleViewHolder.class)
			mono.android.TypeManager.Activate ("MobileRef.MVVM.Android.PeopleViewHolder, MobileRef.MVVM.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
