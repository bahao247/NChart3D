using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;
using System.Collections.Generic;

using NChart3D_Android;

namespace DifferentCharts
{
	[Activity(Label = "DifferentCharts", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		NChartView mNChartView;

		enum SeriesType
		{
			// 2D types.
			Column2D,
			Bar2D,
			Area2D,
			Pie2D,
			Doughnut2D,
			Line2D,
			Step2D,
			Bubble2D,
			Candlestick2D,
			OHLC2D,
			Band,           // Only in 2D
			Sequence,       // Only in 2D
			Radar,          // Only in 2D
			Funnel2D,
			Heatmap,        // Only in 2D

			// 3D types.
			Column3D,
			Bar3D,
			Area3D,
			Pie3D,
			Doughnut3D,
			Line3D,
			Ribbon,         // Only in 3D
			Step3D,
			Bubble3D,
			Surface,        // Only in 3D
			Candlestick3D,
			OHLC3D,
			Funnel3D
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			mNChartView = FindViewById<NChartView>(Resource.Id.surface);

			LoadView();
		}

		private void LoadView()
		{
			// Paste your license key here.
			mNChartView.Chart.LicenseKey = "";

			// Choose chart type from SeriesType enum.
			SeriesType selectedType = SeriesType.Column2D;
			bool is3D = selectedType >= SeriesType.Column3D;
			switch (selectedType)
			{
				case SeriesType.Area2D:
				case SeriesType.Area3D:
					{
						AreaChartController controller = new AreaChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.UpdateData();
					}
					break;

				case SeriesType.Band:
					{
						BandChartController controller = new BandChartController(mNChartView);
						controller.UpdateData();
					}
					break;

				case SeriesType.Bar2D:
				case SeriesType.Bar3D:
					{
						BarChartController controller = new BarChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.UpdateData();
					}
					break;

				case SeriesType.Column2D:
				case SeriesType.Column3D:
					{
						ColumnChartController controller = new ColumnChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.UpdateData();
					}
					break;

				case SeriesType.Pie2D:
				case SeriesType.Pie3D:
					{
						PieChartController controller = new PieChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.HoleRatio = 0.0f;
						controller.UpdateData();
					}
					break;

				case SeriesType.Doughnut2D:
				case SeriesType.Doughnut3D:
					{
						PieChartController controller = new PieChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.HoleRatio = 0.0f;
						controller.UpdateData();
					}
					break;

				case SeriesType.Line2D:
				case SeriesType.Line3D:
					{
						LineChartController controller = new LineChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.StepMode = false;
						controller.UpdateData();
					}
					break;

				case SeriesType.Step2D:
				case SeriesType.Step3D:
					{
						LineChartController controller = new LineChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.StepMode = true;
						controller.UpdateData();
					}
					break;

				case SeriesType.Bubble2D:
				case SeriesType.Bubble3D:
					{
						BubbleChartController controller = new BubbleChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.UpdateData();
					}
					break;

				case SeriesType.Candlestick2D:
				case SeriesType.Candlestick3D:
					{
						CandlestickChartController controller = new CandlestickChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.UpdateData();
					}
					break;

				case SeriesType.OHLC2D:
				case SeriesType.OHLC3D:
					{
						OHLCChartController controller = new OHLCChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.UpdateData();
					}
					break;

				case SeriesType.Funnel2D:
				case SeriesType.Funnel3D:
					{
						FunnelChartController controller = new FunnelChartController(mNChartView);
						controller.DrawIn3D = is3D;
						controller.UpdateData();
					}
					break;

				case SeriesType.Sequence:
					{
						SequenceChartController controller = new SequenceChartController(mNChartView);
						controller.UpdateData();
					}
					break;

				case SeriesType.Radar:
					{
						RadarChartController controller = new RadarChartController(mNChartView);
						controller.UpdateData();
					}
					break;

				case SeriesType.Heatmap:
					{
						HeatmapChartController controller = new HeatmapChartController(mNChartView);
						controller.UpdateData();
					}
					break;

				case SeriesType.Ribbon:
					{
						RibbonChartController controller = new RibbonChartController(mNChartView);
						controller.UpdateData();
					}
					break;

				case SeriesType.Surface:
					{
						SurfaceChartController controller = new SurfaceChartController(mNChartView);
						controller.UpdateData();
					}
					break;
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			mNChartView.OnResume();
		}

		protected override void OnPause()
		{
			base.OnPause();
			mNChartView.OnPause();
		}
	}
}


