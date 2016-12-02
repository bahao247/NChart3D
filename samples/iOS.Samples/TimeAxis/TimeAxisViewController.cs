using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace TimeAxis
{
	public class TimeAxisViewController : UIViewController, INChartSeriesDataSource, INChartTimeAxisDataSource
	{
		NChartView m_view;
		Random m_rand;

		public TimeAxisViewController() : base()
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

			// Set up the time axis.
			m_view.Chart.TimeAxis.TickShape = NChartTimeAxisTickShape.Line;
			m_view.Chart.TimeAxis.TickTitlesFont = UIFont.BoldSystemFontOfSize(11.0f);
			m_view.Chart.TimeAxis.TickTitlesLayout = NChartTimeAxisLabelsLayout.ShowFirstLastLabelsOnly;
			m_view.Chart.TimeAxis.TickTitlesPosition = NChartTimeAxisLabelsPosition.Beneath;
			m_view.Chart.TimeAxis.TickTitlesColor = UIColor.FromRGB(140, 140, 140);
			m_view.Chart.TimeAxis.TickColor = UIColor.FromRGB(110, 110, 110);
			m_view.Chart.TimeAxis.Margin = new NChartMargin(20.0f, 20.0f, 10.0f, 0.0f);
			m_view.Chart.TimeAxis.AutohideTooltip = false;

			// Create the time axis tooltip.
			m_view.Chart.TimeAxis.Tooltip = new NChartTimeAxisTooltip();
			m_view.Chart.TimeAxis.Tooltip.TextColor = UIColor.FromRGB(140, 140, 140);
			m_view.Chart.TimeAxis.Tooltip.Font = UIFont.SystemFontOfSize(11.0f);

			// Set images for the time axis.
			m_view.Chart.TimeAxis.SetImages(null, null, null, null,
				UIImage.FromFile("play-light.png"),
				UIImage.FromFile("play-pushed-light.png"),
				UIImage.FromFile("pause-light.png"),
				UIImage.FromFile("pause-pushed-light.png"),
				UIImage.FromFile("slider-light.png"),
				UIImage.FromFile("handler-light.png"));

			// Visible time axis.
			m_view.Chart.TimeAxis.Visible = true;

			// Set animation time in seconds.
			m_view.Chart.TimeAxis.AnimationTime = 3.0f;

			// Create series.
			NChartColumnSeries series = new NChartColumnSeries();
			series.DataSource = this;
			series.Brush = new NChartSolidColorBrush(UIColor.FromRGB(97, 206, 231));

			// Add series to the chart.
			m_view.Chart.AddSeries(series);

			// Set data source for the time axis to provide ticks.
			m_view.Chart.TimeAxis.DataSource = this;

			// Reset animation.
			m_view.Chart.TimeAxis.Stop();
			m_view.Chart.TimeAxis.GoToFirstTick();

			// Update data in the chart.
			m_view.Chart.UpdateData();

			// Set chart view to the controller.
			this.View = m_view;
		}

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			NChartPoint[] result = new NChartPoint[10];
			for (int i = 0; i < 10; ++i)
			{
				NChartPointState[] states = new NChartPointState[3];
				for (int j = 0; j < 3; ++j)
				{
					NChartPointState state = NChartPointState.PointStateWithXYZ(i, (m_rand.Next() % 30) + 1, 0);
					states[j] = state;
				}
				result[i] = new NChartPoint(states, series);
			}
			return result;
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

		#region INChartTimeAxisDataSource

		public string[] Timestamps(NChartTimeAxis timeAxis)
		{
			return new string[] { "1", "2", "3" };
		}

		#endregion
	}
}

