using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace DataSmoothing
{
	public class DataSmoothingViewController : UIViewController, INChartSeriesDataSource
	{
		NChartView m_view;
		Random m_rand;

		public DataSmoothingViewController() : base()
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

			// Create series that will be smoothed.
			{
				NChartLineSeries series = new NChartLineSeries();
				series.Tag = 1;
				series.Brush = new NChartSolidColorBrush(new UIColor(0.38f, 0.8f, 0.91f, 1.0f));
				series.LineThickness = 2.0f;
				series.DataSource = this;

				// Set data smoother. You can experiment with different data smoothers to see how they work.
				//series.dataSmoother = new NChartDataSmootherSpline ();
				//series.dataSmoother = new NChartDataSmootherSBezier ();
				series.DataSmoother = new NChartDataSmootherTBezier();

				m_view.Chart.AddSeries(series);
			}

			// Create series with no smoothing.
			{
				NChartLineSeries series = new NChartLineSeries();
				series.Tag = 2;
				series.Brush = new NChartSolidColorBrush(UIColor.Red);
				series.LineThickness = 1.0f;
				series.DataSource = this;
				series.Marker = new NChartMarker();
				series.Marker.Shape = NChartMarkerShape.Circle;
				series.Marker.Size = 5;
				series.Marker.Brush = new NChartSolidColorBrush(UIColor.Red);
				m_view.Chart.AddSeries(series);
			}

			// Update data in the chart.
			m_view.Chart.UpdateData();

			// Set chart view to the controller.
			this.View = m_view;
		}

		#region INChartSeriesDataSource

		int[,] data = {
			{ 0, 0 },
			{ 20, 0 },
			{ 45, -47 },
			{ 53, 335 },
			{ 57, 26 },
			{ 62, 387 },
			{ 74, 104 },
			{ 89, 0 },
			{ 95, 100 },
			{ 100, 0 }
		};

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();
			for (int i = 0; i < 10; ++i)
				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXWithXY(data[i, 0], data[i, 1]), series));
			return result.ToArray();
		}

		public string Name(NChartSeries series)
		{
			// Get name of the series.
			return series.Tag == 1 ? "Smoothed" : "Linear";
		}

		// If you don't want to implement method, return null.
		public UIImage Image(NChartSeries series) { return null; }
		public NChartPoint[] ExtraPoints(NChartSeries series) { return null; }

		#endregion
	}
}

