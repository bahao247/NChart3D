using System;
using System.Collections.Generic;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;
using Java.Util;
using Random = System.Random;
using Timer = System.Threading.Timer;

using NChart3D_Android;

namespace AutoScroll
{
	[Activity(Label = "AutoScroll", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, INChartSeriesDataSource, INChartValueAxisDataSource
	{
		NChartView mNChartView;

		Random random = new Random();

		Timer timer;
		object guard = new object();
		int m_count;

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

			// Margin to ensure some free space for the iOS status bar.
			mNChartView.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
			mNChartView.Chart.CartesianSystem.XAxis.ShouldBeautifyMinAndMax = true;
			mNChartView.Chart.CartesianSystem.XAxis.MinTickSpacing = 50;
			mNChartView.Chart.CartesianSystem.YAxis.ShouldBeautifyMinAndMax = true;
			mNChartView.Chart.CartesianSystem.YAxis.MinTickSpacing = 10;

			// !!! This is mandatory to provide ticks for X-Axis.
			mNChartView.Chart.CartesianSystem.XAxis.DataSource = this;

			// !!! Increase the minimal tick spacing for X-Axis to avoid line breaks in ticks.
			mNChartView.Chart.CartesianSystem.XAxis.MinTickSpacing = 80.0f;

			// Create series that will be displayed on the chart.
			NChartCandlestickSeries series = new NChartCandlestickSeries();
			series.PositiveColor = Color.Green;
			series.PositiveBorderColor = series.PositiveColor;
			series.NegativeColor = Color.Red;
			series.NegativeBorderColor = series.NegativeColor;
			series.BorderThickness = 1.0f;
			series.DataSource = this;
			mNChartView.Chart.AddSeries(series);

			// Activate auto scroll and auto toggle of scroll by pan.
			mNChartView.Chart.ShouldAutoScroll = true;
			mNChartView.Chart.ShouldToggleAutoScrollByPan = true;

			// Create label that is shown when auto scroll toggles.
			NChartAutoScrollLabel lbl = new NChartAutoScrollLabel();
			lbl.Background = new NChartSolidColorBrush(Color.Argb(230, 0, 0, 0));
			lbl.Font = new NChartFont(NChartFont.FontDefaultBold, NChartFont.StyleBold, 16.0f);
			lbl.TextColor = Color.White;
			lbl.Padding = new NChartMargin(20.0f, 20.0f, 5.0f, 10.0f);
			lbl.OnText = @"Autoscroll ON";
			lbl.OffText = @"Autoscroll OFF";
			mNChartView.Chart.AutoScrollLabel = lbl;

			// Enable auto-zoom of the Y-Axis.
			mNChartView.Chart.CartesianSystem.ShouldAutoZoom = true;
			mNChartView.Chart.CartesianSystem.AutoZoomAxes = NChartAutoZoomAxes.NormalAxis; // This can also be NChartAutoZoomAxes.SecondaryAxis, in case series are hosted on the secondary axis.
																							// Disable unwanted interactive vertical panning and zooming not to conflict with automatic ones.
			mNChartView.Chart.UserInteractionMode = (mNChartView.Chart.UserInteractionMode) ^ (NChartUserInteraction.ProportionalZoom | NChartUserInteraction.VerticalZoom | NChartUserInteraction.VerticalMove);
			mNChartView.Chart.ZoomMode = NChartZoomMode.Directional;

			// Update data in the chart.
			mNChartView.Chart.UpdateData();

			TimerCallback timerCallback = new TimerCallback(Stream);
			timer = new Timer(timerCallback, null, 100, 100);
		}

		void Stream(object any)
		{
			lock (guard)
			{

				// Begin the data changing session from-within separated thread.
				// Ensure thread-safe changes in the chart by wrapping the updating routine with beginTransaction and
				// endTransaction calls.
				mNChartView.Chart.BeginTransaction();

				// Force chart to extend data.
				mNChartView.Chart.ExtendData();

				// End the data changing session from-within separate thread.
				mNChartView.Chart.EndTransaction();
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

		double myrand(int max)
		{
			return (double)random.Next(max);
		}

		NChartPointState randomState(int index)
		{
			double low = 20.0 - myrand(5);
			double high = 22.0 + myrand(5);
			double open = low + myrand((int)(high - low));
			double close = low + myrand((int)(high - low));
			return NChartPointState.PointStateAlignedToXWithXLowOpenCloseHigh(index, low, open, close, high);
		}

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();
			for (m_count = 0; m_count <= 100; ++m_count)
				result.Add(new NChartPoint(randomState(m_count), series));
			return result.ToArray();
		}

		public NChartPoint[] ExtraPoints(NChartSeries series)
		{
			NChartPointState result = randomState(m_count);
			++m_count;
			return new NChartPoint[] { new NChartPoint(result, series) };
		}

		public string Name(NChartSeries series)
		{
			// Get name of the series.
			return "My series";
		}

		// If you don't want to implement method, return null.
		public Bitmap Image(NChartSeries series) { return null; }

		#endregion

		#region INChartValueAxisDataSource

		public string[] Ticks(NChartValueAxis axis)
		{
			if (axis.Kind.Ordinal() != NChartValueAxisKind.X.Ordinal())
				return null;
			// !!! Create initial ticks for X-Axis.
			List<string> result = new List<string>();
			for (int i = 0; i < m_count; ++i)
				result.Add(string.Format("{0}", i));
			return result.ToArray();
		}

		public string[] ExtraTicks(NChartValueAxis axis)
		{
			if (axis.Kind.Ordinal() != NChartValueAxisKind.X.Ordinal())
				return null;
			return new string[] { string.Format("{0}", m_count) };
		}

		// If you don't want to implement method, return null.
		public string DoubleToString(double value, NChartValueAxis axis) { return null; }
		public Number Length(NChartValueAxis axis) { return null; }
		public Number Max(NChartValueAxis axis) { return null; }
		public Number Min(NChartValueAxis axis) { return null; }
		public string Name(NChartValueAxis axis) { return null; }
		public Number Step(NChartValueAxis axis) { return null; }
		public Date MinDate(NChartValueAxis nChartValueAxis) { return null; }
		public Date MaxDate(NChartValueAxis nChartValueAxis) { return null; }
		public Number DateStep(NChartValueAxis nChartValueAxis) { return null; }
		public string DateToString(Date nDate, double v, NChartValueAxis nChartValueAxis) { return null; }

		#endregion
	}
}


