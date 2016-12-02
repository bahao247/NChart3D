using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace DifferentCharts
{
	public class SurfaceViewController : UIViewController, INChartSeriesDataSource
	{
		NChartView m_view;
		Random m_rand;

		public SurfaceViewController() : base()
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

			// Switch 3D on.
			m_view.Chart.DrawIn3D = true;
			// Margin to ensure some free space for the iOS status bar and Y-Axis tick titles.
			m_view.Chart.CartesianSystem.Margin = new NChartMargin(50.0f, 50.0f, 10.0f, 20.0f);
			m_view.Chart.PolarSystem.Margin = new NChartMargin(50.0f, 50.0f, 10.0f, 20.0f);

			// Create series that will be displayed on the chart.
			NChartSurfaceSeries series = new NChartSurfaceSeries();
			series.DataSource = this;
			series.Tag = 0;
			series.Brush = new NChartSolidColorBrush(UIColor.FromRGB(97, 206, 231));
			m_view.Chart.AddSeries(series);

			m_view.Chart.CartesianSystem.XAxis.HasOffset = false;
			m_view.Chart.CartesianSystem.YAxis.HasOffset = false;
			m_view.Chart.CartesianSystem.ZAxis.HasOffset = false;

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

			for (int i = 0, n = 20; i < n; ++i)
			{
				for (int j = 0, m = 20; j < m; ++j)
				{
					double x = (double)(i) * 2.0 * Math.PI / (double)n;
					double z = (double)(j) * 2.0 * Math.PI / (double)m;
					double y = Math.Sin(x) * Math.Cos(z);

					NChartPointState state = NChartPointState.PointStateWithXYZ(i, y, j);
					result.Add(new NChartPoint(state, series));
				}
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

