using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Threading;
using NChart3D_iOS;

namespace Streaming
{
	public class StreamingViewController : UIViewController, INChartSeriesDataSource
	{
		NChartView m_view;
		Timer m_timer;
		object m_guard;
		Random m_rand;

		public StreamingViewController() : base()
		{
			m_guard = new object();
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

			// Create series that will be displayed on the chart.
			NChartColumnSeries series = new NChartColumnSeries();

			// Set brush that will fill that series with color.
			series.Brush = new NChartSolidColorBrush(UIColor.FromRGBA(97, 206, 231, 255));

			// Set data source for the series.
			series.DataSource = this;

			// Add series to the chart.
			m_view.Chart.AddSeries(series);

			// Activate streaming mode.
			m_view.Chart.StreamingMode = true;

			// Prevent minimum and maximum on the axes from "jumping" by activating incremental mode. So the minimum will remain
			// the minimal value ever appeared in the data, and maximum will remain the maximal one.
			m_view.Chart.IncrementalMinMaxMode = true;

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

				// Update data in the points.
				NChartPoint[] points = (series as NChartSeries).Points();
				foreach (NChartPoint point in points)
				{
					double value = m_rand.NextDouble();
					point.CurrentState.DoubleY = 30.0 * value;
					point.CurrentState.Brush = new NChartSolidColorBrush(UIColor.FromRGB((float)value, 0.8f, 1.0f - (float)value));
				}

				// Update data in the chart.
				m_view.Chart.StreamData();

				// End the data changing session from-within separate thread.
				m_view.Chart.EndTransaction();
			}
		}

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();
			for (int i = 0; i <= 10; ++i)
				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXWithXY(i, m_rand.Next() % 30 + 1), series));
			return result.ToArray();
		}

		public string Name(NChartSeries series)
		{
			// Get name of the series.
			return "My series";
		}

		// If you don't want to implement method, return null.
		public UIImage Image(NChartSeries series) { return null; }
		public NChartPoint[] ExtraPoints(NChartSeries series) { return null; }

		#endregion
	}
}

