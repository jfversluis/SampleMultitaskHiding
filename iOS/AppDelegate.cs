using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

namespace SampleMultitaskHiding.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		// Obscure UI in multitasking view
		public override void OnResignActivation(UIApplication uiApplication)
		{
			base.OnResignActivation(uiApplication);

			// Prevent taking snapshot
			uiApplication.IgnoreSnapshotOnNextApplicationLaunch();

			// Create our hiding view which is transparent initially. Also note the tag I'm giving the view. Why 42 you ask..?
			// For this example let's just use a Xamarin Blue background color
			var bgView = new UIView(uiApplication.KeyWindow.Frame) { Tag = 42, Alpha = 0, BackgroundColor = Color.FromHex("#449CD5").ToUIColor() };

			// Add the view to our current window
			uiApplication.KeyWindow.AddSubview(bgView);
			uiApplication.KeyWindow.BringSubviewToFront(bgView);

			// Animate it to the front and thus hide the contents of the app
			UIView.Animate(0.5, () =>
				{
					bgView.Alpha = 1;
				});
		}

		// Bring back main interface from obscuring
		public override void OnActivated(UIApplication uiApplication)
		{
			base.OnActivated(uiApplication);

			// Find our hiding view by the same tag we saw earlier
			var view = uiApplication.KeyWindow.ViewWithTag(42);

			// If we found it, hide it again!
			if (view != null)
			{
				// Animate it back to transparent
				UIView.Animate(0.5, () =>
					{
						view.Alpha = 0;
					}, () =>
					{
						// And after that completed, remove te view altogether
						view.RemoveFromSuperview();
					});
			}
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}