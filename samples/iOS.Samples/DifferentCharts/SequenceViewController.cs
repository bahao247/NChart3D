using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace DifferentCharts
{
	public class SequenceViewController : UIViewController, INChartSeriesDataSource
	{
		NChartView m_view;
		Random m_rand;
		// Brushes.
		NChartSolidColorBrush[] brushes { get; set; }

		public SequenceViewController() : base()
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

			// Margin to ensure some free space for the iOS status bar.
			m_view.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
			m_view.Chart.PolarSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// Create series that will be displayed on the chart.
			for (int i = 0; i < 3; ++i)
			{
				NChartSequenceSeries series = new NChartSequenceSeries();
				series.DataSource = this;
				series.Tag = i;
				series.Brush = brushes[i];
				m_view.Chart.AddSeries(series);
			}

			m_view.Chart.CartesianSystem.XAxis.HasOffset = false;
			m_view.Chart.CartesianSystem.YAxis.HasOffset = true;
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

			for (int i = 0; i < 30; ++i)
			{
				int y = m_rand.Next() % 4;
				double open = m_rand.Next() % 30;
				double close = open + 1.0;
				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToYWithYOpenClose(
					y, open, close
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

