using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Threading;
using NChart3D_iOS;

namespace AutoScroll
{
	public class AutoScrollViewController : UIViewController, INChartSeriesDataSource, INChartValueAxisDataSource
	{
		NChartView m_view;
		Timer m_timer;
		Object m_guard;
		Random m_rand;
		nint m_count;

		public AutoScrollViewController() : base()
		{
			m_guard = new Object();
			m_rand = new Random();
		}

		public override void LoadView()
		{
			// Create a chart view that will display the chart.
			m_view = new NChartView();

			// Paste your license key here.
			m_view.Chart.LicenseKey = "";

			// Margin to ensure some free space for the iOS status bar.
			m_view.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
			m_view.Chart.CartesianSystem.XAxis.ShouldBeautifyMinAndMax = true;
			m_view.Chart.CartesianSystem.XAxis.MinTickSpacing = 50;
			m_view.Chart.CartesianSystem.YAxis.ShouldBeautifyMinAndMax = true;
			m_view.Chart.CartesianSystem.YAxis.MinTickSpacing = 10;

			// !!! This is mandatory to provide ticks for X-Axis.
			m_view.Chart.CartesianSystem.XAxis.DataSource = this;

			// !!! Increase the minimal tick spacing for X-Axis to avoid line breaks in ticks.
			m_view.Chart.CartesianSystem.XAxis.MinTickSpacing = 80.0f;

			// Create series that will be displayed on the chart.
			NChartCandlestickSeries series = new NChartCandlestickSeries();
			series.PositiveColor = UIColor.Green;
			series.PositiveBorderColor = series.PositiveColor;
			series.NegativeColor = UIColor.Red;
			series.NegativeBorderColor = series.NegativeColor;
			series.BorderThickness = 1.0f;
			series.DataSource = this;
			m_view.Chart.AddSeries(series);

			// Activate auto scroll and auto toggle of scroll by pan.
			m_view.Chart.ShouldAutoScroll = true;
			m_view.Chart.ShouldToggleAutoScrollByPan = true;

			// Create label that is shown when auto scroll toggles.
			NChartAutoScrollLabel lbl = new NChartAutoScrollLabel();
			lbl.Background = new NChartSolidColorBrush(new UIColor(0.0f, 0.0f, 0.0f, 0.8f));
			lbl.Font = UIFont.BoldSystemFontOfSize(16.0f);
			lbl.TextColor = UIColor.White;
			lbl.Padding = new NChartMargin(20.0f, 20.0f, 5.0f, 10.0f);
			lbl.OnText = @"Autoscroll ON";
			lbl.OffText = @"Autoscroll OFF";
			m_view.Chart.AutoScrollLabel = lbl;

			// Enable auto-zoom of the Y-Axis.
			m_view.Chart.CartesianSystem.ShouldAutoZoom = true;
			m_view.Chart.CartesianSystem.AutoZoomAxes = NChartAutoZoomAxes.NormalAxis; // This can also be NChartAutoZoomAxes.SecondaryAxis, in case series are hosted on the secondary axis.
																					   // Disable unwanted interactive vertical panning and zooming not to conflict with automatic ones.
			m_view.Chart.UserInteractionMode = (m_view.Chart.UserInteractionMode) ^ ((uint)NChartUserInteraction.ProportionalZoom | (uint)NChartUserInteraction.VerticalZoom | (uint)NChartUserInteraction.VerticalMove);
			m_view.Chart.ZoomMode = NChartZoomMode.Directional;

			// Update data in the chart.
			m_view.Chart.UpdateData();

			// Set chart view to the controller.
			this.View = m_view;

			TimerCallback timerCallback = new TimerCallback(stream);
			m_timer = new Timer(timerCallback, m_view.Chart.Series()[m_view.Chart.Series().Length - 1], 100, 100);
		}

		void stream(object series)
		{
			lock (m_guard)
			{

				// Begin the data changing session from-within separated thread.
				// Ensure thread-safe changes in the chart by wrapping the updating routine with beginTransaction and
				// endTransaction calls.
				m_view.Chart.BeginTransaction();

				// Force chart to extend data.
				m_view.Chart.ExtendData();

				// End the data changing session from-within separate thread.
				m_view.Chart.EndTransaction();
			}
		}

		double myrand(int max)
		{
			return (double)m_rand.Next(max);
		}

		NChartPointState randomState(nint index)
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
		public UIImage Image(NChartSeries series) { return null; }

		#endregion

		#region INChartValueAxisDataSource

		public string[] Ticks(NChartValueAxis axis)
		{
			// Choose ticks by the kind of axis.
			switch (axis.Kind)
			{
				case NChartValueAxisKind.X:
					// !!! Create initial ticks for X-Axis.
					List<string> result = new List<string>();
					for (int i = 0; i < m_count; ++i)
						result.Add(string.Format("{0}", i));
					return result.ToArray();

				default:
					// We do not have other axes.
					return null;
			}
		}

		public string[] ExtraTicks(NChartValueAxis axis)
		{
			// Choose ticks by the kind of axis.
			switch (axis.Kind)
			{
				case NChartValueAxisKind.X:
					// !!! Create extra ticks for X-Axis.
					return new string[] { string.Format("{0}", m_count) };

				default:
					// We do not have other axes.
					return null;
			}
		}

		// If you don't want to implement method, return null.
		public string DoubleToString(double value, NChartValueAxis axis) { return null; }
		public NSNumber Length(NChartValueAxis axis) { return null; }
		public NSNumber Max(NChartValueAxis axis) { return null; }
		public NSNumber Min(NChartValueAxis axis) { return null; }
		public string Name(NChartValueAxis axis) { return null; }
		public NSNumber Step(NChartValueAxis axis) { return null; }
		public NSDate MinDate(NChartValueAxis axis) { return null; }
		public NSDate MaxDate(NChartValueAxis axis) { return null; }
		public NSNumber DateStep(NChartValueAxis axis) { return null; }
		public string DateToString(NSDate date, double value, NChartValueAxis axis) { return null; }

		#endregion
	}
}

