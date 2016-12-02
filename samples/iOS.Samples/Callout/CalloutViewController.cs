using System;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace Callout
{
	public class CalloutViewController : UIViewController, INChartSeriesDataSource
	{
		NChartView m_view;
		Random m_rand;

		public NChartSolidColorBrush[] brushes { get; set; }

		public CalloutViewController() : base()
		{
			m_rand = new Random();

			// Create brushes.
			brushes = new NChartSolidColorBrush[3];
			brushes[0] = new NChartSolidColorBrush(UIColor.FromRGB(97, 206, 231));
			brushes[1] = new NChartSolidColorBrush(UIColor.FromRGB(203, 220, 56));
			brushes[2] = new NChartSolidColorBrush(UIColor.FromRGB(229, 74, 131));
		}

		public override void LoadView()
		{
			// Create a chart view that will display the chart.
			m_view = new NChartView();

			// Paste your license key here.
			m_view.Chart.LicenseKey = "";

			m_view.Chart.PolarSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// Create series that will be displayed on the chart.
			for (int i = 0; i < 3; ++i)
			{
				NChartPieSeries series = new NChartPieSeries();
				series.DataSource = this;
				series.Tag = i;
				series.Brush = brushes[i];
				m_view.Chart.AddSeries(series);
			}

			// Update data in the chart.
			m_view.Chart.UpdateData();

			// Set chart view to the controller.
			this.View = m_view;
		}

		NChartCallout CreateCallout(string text, nint dataIndex)
		{
			NChartCallout result = new NChartCallout();

			result.Background = new NChartSolidColorBrush(UIColor.White);
			result.Background.Opacity = 0.9f;
			result.Padding = new NChartMargin(10.0f, 10.0f, 10.0f, 10.0f);
			result.BorderColor = UIColor.FromRGB(128, 128, 128);
			result.BorderThickness = 1.0f;
			result.Font = UIFont.SystemFontOfSize(16.0f);
			result.Visible = true;
			result.Text = text;

			// Force the margin of callout to correspond the median of the corresponding pie sector.
			result.Distance = 100.0f;

			return result;
		}

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			NChartPoint[] result = new NChartPoint[1];

			result[0] = new NChartPoint(NChartPointState.PointStateWithCircleValue(0, (m_rand.Next() % 30) + 1), series);
			result[0].Tooltip = CreateCallout(string.Format("Callout for series {0}", series.Tag + 1), series.Tag);

			return result;
		}

		public string Name(NChartSeries series)
		{
			// Get name of the series.
			return string.Format("Series {0}", series.Tag + 1);
		}

		// If you don't want to implement method, return null.
		public UIImage Image(NChartSeries series) { return null; }
		public NChartPoint[] ExtraPoints(NChartSeries series) { return null; }

		#endregion
	}
}

