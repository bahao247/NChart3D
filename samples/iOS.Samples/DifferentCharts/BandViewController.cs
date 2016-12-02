using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace DifferentCharts
{
	public class BandViewController : UIViewController, INChartSeriesDataSource
	{
		NChartView m_view;
		Random m_rand;

		public BandViewController() : base()
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

			// Margin to ensure some free space for the iOS status bar.
			m_view.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
			m_view.Chart.PolarSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// Create series that will be displayed on the chart.
			NChartBandSeries series = new NChartBandSeries();
			series.DataSource = this;
			series.Tag = 0;
			series.PositiveColor = UIColor.FromRGBA(112, 170, 242, 204);
			series.NegativeColor = UIColor.FromRGBA(196, 240, 91, 204);
			series.HighBorderColor = UIColor.FromRGB(130, 196, 255);
			series.LowBorderColor = UIColor.FromRGB(226, 255, 112);
			series.BorderThickness = 5.0f;
			m_view.Chart.AddSeries(series);

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

			for (int i = 0; i < 10; ++i)
			{
				double low = m_rand.Next() % 20;
				double high = m_rand.Next() % 20;
				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXWithXLowHigh(
					i, low, high
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

