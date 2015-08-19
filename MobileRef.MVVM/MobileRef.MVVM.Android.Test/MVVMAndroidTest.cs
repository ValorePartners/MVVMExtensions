using NUnit.Framework;
using Xamarin.UITest;
using System.Linq;

namespace MobileRef.MVVM.Android.Test
{
	[TestFixture]
	public class MVVMAndroidTest
	{
		private IApp app;
		private const string pathToBinary = "/Users/Les/Documents/MVVMExtensions/MobileRef.MVVM/MobileRef.MVVM.Android/bin/Release/MobileRef.MVVM.Android.apk";

		[SetUp]
		public void Setup ()
		{
			
			app = ConfigureApp
				.Android
				.DeviceIp("10.71.34.101")
				.ApiKey("7cf3315072fd4fa8a4f90a121b4a0e5a")
				.ApkFile (pathToBinary)
				.StartApp ();

		}

		[TestCase (TestName = "Verify the app launches")]
		public void AndroidExtensionsTest ()
		{
			var elements = app.WaitForElement (e => e.All (), "Nothing has loaded on the screen.");
		
			app.Screenshot ("Home screen is displayed");

			app.WaitForElement (e => e.Id ("btnSimpleMessage"));
			app.Tap (e => e.Id ("btnSimpleMessage"));
			app.Screenshot ("Simple message pressed");
			app.Tap (e => e.Text ("Okay"));

			app.WaitForElement (e => e.Id ("btnMultipleMessage"));
			app.Tap (e => e.Id ("btnMultipleMessage"));
			app.Screenshot ("Multiple message pressed");
			app.Tap (e => e.Text ("Oranges"));

			app.WaitForElement (e => e.Id ("btnInputDialog"));
			app.Tap (e => e.Id ("btnInputDialog"));
			app.Screenshot ("Input dialog pressed");
	
			app.EnterText (e => e.Id ("custom"), "23");
			app.Tap (e => e.Text ("Save"));
			app.Screenshot ("Age has been saved");
			app.Tap (e => e.Text ("Okay"));

			app.WaitForElement (e => e.Id ("btnNavigateParams"));
			app.Tap (e => e.Id ("btnNavigateParams"));
			app.Screenshot ("Navigated with parameters");

			app.WaitForElement (e => e.Id ("txtHello"));
			app.EnterText (e => e.Id ("txtHello"), "This is a UX test");
			app.Screenshot ("Databinding example");

			app.WaitForElement (e => e.Id ("btnNavigate"));
			app.Tap (e => e.Id ("btnNavigate"));
			app.Screenshot ("Test ICommand Execution");

			app.WaitForElement (e => e.Id ("btnTableBinding"));
			app.Tap (e => e.Id ("btnTableBinding"));
			app.Screenshot ("Table navigation");

			app.WaitForElement (e => e.Id ("txtSearch"));
			app.EnterText (e => e.Id ("txtSearch"), "london");
			app.Screenshot ("Weather Criteria Entered");

			app.WaitForElement (e => e.Id ("btnSearch"));
			app.Tap (e => e.Id ("btnSearch"));
			app.Screenshot ("Weather Search Performed");

			app.WaitForElement (e => e.Id ("lstWeather"));
			app.Tap (x => x.Id ("lstWeather").Child (2));
			app.Screenshot ("Row Selected");
			app.Tap (e => e.Text ("Okay"));

			app.Tap (e => e.Id ("home"));
			app.Tap (e => e.Id ("home"));

			app.WaitForElement (e => e.Id ("btnNavigate"));
			app.Tap (e => e.Id ("btnNavigate"));
			app.Screenshot ("Navigate NoSQL");

			app.WaitForElement (e => e.Id ("btnSearch"));
			app.Tap (e => e.Id ("btnSearch"));
			app.Screenshot ("Perform Empty Search");

			app.WaitForElement (e => e.Id ("txtSearch"));
			app.EnterText (e => e.Id ("txtSearch"), "so");
			app.Tap (e => e.Id ("btnSearch"));
			app.Screenshot ("Perform Criteria Search");

			app.Tap (e => e.Id ("home"));

			//app.Repl ();    then use -> tree
		}
	}
}

