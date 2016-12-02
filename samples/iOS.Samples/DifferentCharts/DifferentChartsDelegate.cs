using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace DifferentCharts
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register("DifferentChartsDelegate")]
	public partial class DifferentChartsDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow(UIScreen.MainScreen.Bounds);

			// Set chart type these:
			SeriesType selectedType = SeriesType.Column2D;
			window.RootViewController = chooseController(selectedType);

			// make the window visible
			window.MakeKeyAndVisible();

			return true;
		}

		UIViewController chooseController(SeriesType type)
		{
			bool is3D = type >= SeriesType.Column3D;
			switch (type)
			{
				case SeriesType.Area2D:
				case SeriesType.Area3D:
					{
						AreaViewController controller = new AreaViewController();
						controller.drawIn3D = is3D;
						return controller;
					}

				case SeriesType.Band:
					{
						BandViewController controller = new BandViewController();
						return controller;
					}

				case SeriesType.Bar2D:
				case SeriesType.Bar3D:
					{
						BarViewController controller = new BarViewController();
						controller.drawIn3D = is3D;
						return controller;
					}

				case SeriesType.Column2D:
				case SeriesType.Column3D:
					{
						ColumnViewController controller = new ColumnViewController();
						controller.drawIn3D = is3D;
						return controller;
					}

				case SeriesType.Pie2D:
				case SeriesType.Pie3D:
					{
						PieViewController controller = new PieViewController();
						controller.drawIn3D = is3D;
						controller.holeRatio = 0.0f;
						return controller;
					}

				case SeriesType.Doughnut2D:
				case SeriesType.Doughnut3D:
					{
						PieViewController controller = new PieViewController();
						controller.drawIn3D = is3D;
						controller.holeRatio = 0.0f;
						return controller;
					}

				case SeriesType.Line2D:
				case SeriesType.Line3D:
					{
						LineViewController controller = new LineViewController();
						controller.drawIn3D = is3D;
						controller.stepMode = false;
						return controller;
					}

				case SeriesType.Step2D:
				case SeriesType.Step3D:
					{
						LineViewController controller = new LineViewController();
						controller.drawIn3D = is3D;
						controller.stepMode = true;
						return controller;
					}

				case SeriesType.Bubble2D:
				case SeriesType.Bubble3D:
					{
						BubbleViewController controller = new BubbleViewController();
						controller.drawIn3D = is3D;
						return controller;
					}

				case SeriesType.Candlestick2D:
				case SeriesType.Candlestick3D:
					{
						CandlestickViewController controller = new CandlestickViewController();
						controller.drawIn3D = is3D;
						return controller;
					}

				case SeriesType.OHLC2D:
				case SeriesType.OHLC3D:
					{
						OHLCViewController controller = new OHLCViewController();
						controller.drawIn3D = is3D;
						return controller;
					}

				case SeriesType.Funnel2D:
				case SeriesType.Funnel3D:
					{
						FunnelViewController controller = new FunnelViewController();
						controller.drawIn3D = is3D;
						return controller;
					}

				case SeriesType.Sequence:
					{
						SequenceViewController controller = new SequenceViewController();
						return controller;
					}

				case SeriesType.Radar:
					{
						RadarViewController controller = new RadarViewController();
						return controller;
					}

				case SeriesType.Heatmap:
					{
						HeatmapViewController controller = new HeatmapViewController();
						return controller;
					}

				case SeriesType.Ribbon:
					{
						RibbonViewController controller = new RibbonViewController();
						return controller;
					}

				case SeriesType.Surface:
					{
						SurfaceViewController controller = new SurfaceViewController();
						return controller;
					}
				default:
					return null;
			}
		}
	}
}

