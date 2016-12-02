using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace DifferentCharts
{
	public class FunnelViewController : UIViewController, INChartSeriesDataSource
	{
		NChartView m_view;
		Random m_rand;
		// Brushes.
		NChartSolidColorBrush[] brushes { get; set; }
		public bool drawIn3D { get; set; }

		public FunnelViewController() : base()
		{
			m_rand = new Random();

			// Create brushes.
			brushes = new NChartSolidColorBrush[] {
				new NChartSolidColorBrush (UIColor.FromRGB (97, 206, 231)),
				new NChartSolidColorBrush (UIColor.FromRGB (203, 220, 56)),
				new NChartSolidColorBrush (UIColor.FromRGB (229, 74, 131)),
			};
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
			for (int i = 0; i < 3; ++i)
			{
				NChartFunnelSeries series = new NChartFunnelSeries();
				series.DataSource = this;
				series.Tag = i;
				series.BottomRadius = (float)(i + 1) / 5.0f;
				series.TopRadius = (float)(i + 2) / 5.0f;
				NChartBrush brush = brushes[i];
				brush.Opacity = 0.8f;
				series.Brush = brush;
				m_view.Chart.AddSeries(series);
			}
			m_view.Chart.CartesianSystem.Visible = false;

			// Update data in the chart.
			m_view.Chart.UpdateData();

			// Set chart view to the controller.
			this.View = m_view;
		}

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			return new NChartPoint[] { new NChartPoint(NChartPointState.PointStateWithXYZ(0, m_rand.Next() % 30, 0), series) };
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

