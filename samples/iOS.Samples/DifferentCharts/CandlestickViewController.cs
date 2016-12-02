using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace DifferentCharts
{
	public class CandlestickViewController : UIViewController, INChartSeriesDataSource
	{
		NChartView m_view;
		Random m_rand;
		public bool drawIn3D { get; set; }

		public CandlestickViewController() : base()
		{
			m_rand = new Random();
		}

		public override void LoadView()
		{
			// Create a chart view that will display the chart.
			m_view = new NChartView();

			// Paste your license key here.
			m_view.Chart.LicenseKey = "";

			// Switch on antialiasing.
			m_view.Chart.ShouldAntialias = true;

			if (drawIn3D)
			{
				// Switch 3D on.
				m_view.Chart.DrawIn3D = true;
				// Margin to ensure some free space for the iOS status bar and Y-Axis tick titles.
				m_view.Chart.CartesianSystem.Margin = new NChartMargin(50.0f, 50.0f, 10.0f, 20.0f);
				m_view.Chart.PolarSystem.Margin = new NChartMargin(50.0f, 50.0f, 10.0f, 20.0f);
			}
			else {
				// Margin to ensure some free space for the iOS status bar.
				m_view.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
				m_view.Chart.PolarSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
			}

			// Create series that will be displayed on the chart.
			NChartCandlestickSeries series = new NChartCandlestickSeries();
			series.DataSource = this;
			series.Tag = 0;
			series.PositiveColor = UIColor.FromRGB(73, 226, 141);
			series.PositiveBorderColor = UIColor.FromRGB(65, 204, 38);
			series.NegativeColor = UIColor.FromRGB(221, 73, 73);
			series.NegativeBorderColor = UIColor.FromRGB(199, 15, 50);
			series.BorderThickness = 3.0f;
			m_view.Chart.AddSeries(series);

			NChartCandlestickSeriesSettings settings = new NChartCandlestickSeriesSettings();
			settings.CylindersResolution = 20;
			m_view.Chart.AddSeriesSettings(settings);

			m_view.Chart.CartesianSystem.XAxis.HasOffset = true;
			m_view.Chart.CartesianSystem.YAxis.HasOffset = false;
			m_view.Chart.CartesianSystem.ZAxis.HasOffset = true;

			// Update data in the chart.
			m_view.Chart.UpdateData();

			// Set chart view to the controller.
			this.View = m_view;
		}

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();

			for (int i = 0; i < 30; ++i)
			{
				double open = 5.0f * Math.Sin((float)i * Math.PI / 10);
				double close = 5.0f * Math.Cos((float)i * Math.PI / 10);
				double low = Math.Min(open, close) - (m_rand.Next() % 3);
				double high = Math.Max(open, close) + (m_rand.Next() % 3);
				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXZWithXZLowOpenCloseHigh(
					i, series.Tag, low, open, close, high
				), series));
			}

			return result.ToArray();
		}

		public NChartPoint[] ExtraPoints(NChartSeries series)
		{
			return null;
		}

		public string Name(NChartSeries series)
		{
			// Get name of the series.
			return string.Format("My series {0}", series.Tag + 1);
		}

		// If you don't want to implement method, return null.
		public UIImage Image(NChartSeries series) { return null; }

		#endregion
	}
}

