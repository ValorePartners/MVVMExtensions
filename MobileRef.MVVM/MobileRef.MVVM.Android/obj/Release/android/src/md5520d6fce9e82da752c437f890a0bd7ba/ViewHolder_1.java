package md5520d6fce9e82da752c437f890a0bd7ba;


public abstract class ViewHolder_1
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MobileRef.MVVM.Android.ViewHolder`1, MobileRef.MVVM.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ViewHolder_1.class, __md_methods);
	}


	public ViewHolder_1 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ViewHolder_1.class)
			mono.android.TypeManager.Activate ("MobileRef.MVVM.Android.ViewHolder`1, MobileRef.MVVM.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
