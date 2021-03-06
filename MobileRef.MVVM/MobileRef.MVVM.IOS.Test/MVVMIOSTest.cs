﻿using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace MobileRef.MVVM.IOS.Test
{
	[TestFixture ()]
	public class MVVMIOSTest
	{
		iOSApp app;

		public string PathToIPA { get; private set; }

		[SetUp]
		public void BeforeEachTest ()
		{
						PathToIPA = "../../../MobileRef.MVVM.IOS/bin/iPhoneSimulator/Debug/MobileRefMVVMIOS.app";
						app = ConfigureApp.iOS
								.EnableLocalScreenshots ()
								.ApiKey("7cf3315072fd4fa8a4f90a121b4a0e5a")
								.AppBundle (PathToIPA)
								.StartApp ();
		}

//		[TestFixtureSetUp]
//		public void TestFixtureSetup ()
//		{
//			PathToIPA = "../../../MobileRef.MVVM.IOS/bin/iPhoneSimulator/Debug/MobileRefMVVMIOS.app";
//			app = ConfigureApp.iOS
//					.EnableLocalScreenshots ()
//					.AppBundle (PathToIPA)
//					.StartApp ();
//		}

		[TestCase (TestName = "Verify the app launches")]
		public void IOSExtensionsTest ()
		{
			var elements = app.WaitForElement (e => e.All (), "Nothing has loaded on the screen.");

			app.Screenshot ("Home screen is displayed");

			app.WaitForElement (e => e.Marked ("btnSimpleMessage"));
			app.Tap (e => e.Id ("btnSimpleMessage"));
			app.Screenshot ("Simple message pressed");
			app.Tap (e => e.Text ("Okay"));

			app.WaitForElement (e => e.Id ("btnMultipleMessage"));
			app.Tap (e => e.Id ("btnMultipleMessage"));
			app.Screenshot ("Multiple message pressed");
			app.Tap (e => e.Text ("Oranges"));

			app.WaitForElement (e => e.Id ("btnActionSheet"));
			app.Tap (e => e.Id ("btnActionSheet"));
			app.Screenshot ("ActionSheet pressed");
			app.Tap (e => e.Text ("Melons"));
				


//			app.EnterText (e => e.Id ("custom"), "23");
//			app.Tap (e => e.Text ("Save"));
//			app.Screenshot ("Age has been saved");
//			app.Tap (e => e.Text ("Okay"));
//
//			app.WaitForElement (e => e.Id ("btnNavigateParams"));
//			app.Tap (e => e.Id ("btnNavigateParams"));
//			app.Screenshot ("Navigated with parameters");
//
//			app.WaitForElement (e => e.Id ("txtHello"));
//			app.EnterText (e => e.Id ("txtHello"), "This is a UX test");
//			app.Screenshot ("Databinding example");
//
//			app.WaitForElement (e => e.Id ("btnNavigate"));
//			app.Tap (e => e.Id ("btnNavigate"));
//			app.Screenshot ("Test ICommand Execution");
//
//			app.WaitForElement (e => e.Id ("btnTableBinding"));
//			app.Tap (e => e.Id ("btnTableBinding"));
//			app.Screenshot ("Table navigation");
//
//			app.WaitForElement (e => e.Id ("txtSearch"));
//			app.EnterText (e => e.Id ("txtSearch"), "london");
//			app.Screenshot ("Weather Criteria Entered");
//
//			app.WaitForElement (e => e.Id ("btnSearch"));
//			app.Tap (e => e.Id ("btnSearch"));
//			app.Screenshot ("Weather Search Performed");
//
//			app.WaitForElement (e => e.Id ("lstWeather"));
//			app.Tap (x => x.Id ("lstWeather").Child (2));
//			app.Screenshot ("Row Selected");
//			app.Tap (e => e.Text ("Okay"));
//
//			app.Tap (e => e.Id ("home"));
//			app.Tap (e => e.Id ("home"));
//
//			app.WaitForElement (e => e.Id ("btnNavigate"));
//			app.Tap (e => e.Id ("btnNavigate"));
//			app.Screenshot ("Navigate NoSQL");
//
//			app.WaitForElement (e => e.Id ("btnSearch"));
//			app.Tap (e => e.Id ("btnSearch"));
//			app.Screenshot ("Perform Empty Search");
//
//			app.WaitForElement (e => e.Id ("txtSearch"));
//			app.EnterText (e => e.Id ("txtSearch"), "so");
//			app.Tap (e => e.Id ("btnSearch"));
//			app.Screenshot ("Perform Criteria Search");
//
//			app.Tap (e => e.Id ("home"));
		}
	}
}

