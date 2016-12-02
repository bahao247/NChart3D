using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace BrushScale
{
	public class BrushScaleViewController : UIViewController, INChartSeriesDataSource, INChartScaleLegendDelegate
	{
		NChartView m_view;

		public BrushScaleViewController() : base()
		{
		}

		public override void LoadView()
		{
			// Create a chart view that will display the chart.
			m_view = new NChartView();

			// Paste your license key here.
			m_view.Chart.LicenseKey = "";

			// Margin to ensure some free space for the iOS status bar.
			m_view.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// We'll use 3D chart.
			m_view.Chart.DrawIn3D = true;

			// Create series that will be displayed on the chart.
			NChartSurfaceSeries series = new NChartSurfaceSeries();
			// Set data source for the series.
			series.DataSource = this;
			// Add series to the chart.
			m_view.Chart.AddSeries(series);

			// Create brush scale for series.
			// First create an array of solid color brushes with the scale colors.
			NChartBrush[] brushes = {
				new NChartSolidColorBrush(UIColor.Red),
				new NChartSolidColorBrush(UIColor.Yellow),
				new NChartSolidColorBrush(UIColor.Green),
				new NChartSolidColorBrush(UIColor.Cyan)
			};
			// Then create array of values that will indicate intervals to choose colors. Note, that the rule of getting brush
			// from the scale for given value x is:
			// if x <= values[0] then choose brush[0]
			// else if x > values[i] and x <= values[i + 1] then choose brush[i + 1], i = 0 .. n - 1, n is number of values
			// else if x > values[n - 1] then choose brush[n]
			// This is why array of brushes should have one more element than array of values.
			NSNumber[] values = { new NSNumber(-0.5), new NSNumber(0.0), new NSNumber(0.5) };
			// Now create the brush scale itself.
			NChartBrushScale scale = new NChartBrushScale(brushes, values);
			// If you want smooth color transitions, let this line commented.
			// scale.IsGradient = false;
			// Assign the scale to the series.
			series.Scale = scale;

			// You probably want to have a legend on the chart displaying the scale. So create one.
			NChartScaleLegend scaleLegend = new NChartScaleLegend(scale);
			// Let it be on the left. Feel free to experiment with different locations.
			scaleLegend.BlockAlignment = NChartLegendBlockAlignment.Left;
			// Let it have a single column.
			scaleLegend.ColumnCount = 1;
			// Set the delegate for the legend to customize the strings in the entries. Do not assign the delegate if the
			// default behavior is ok for you.
			scaleLegend.ScaleDelegate = this;
			// Add the legend to the chart.
			m_view.Chart.AddScaleLegend(scaleLegend);

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
				for (int j = 0, m = 20; j < m; ++j)
				{
					double x = (double)(i) * 2.0 * Math.PI / (double)n;
					double z = (double)(j) * 2.0 * Math.PI / (double)m;
					double y = Math.Sin(x) * Math.Cos(z);
					NChartPointState state = NChartPointState.PointStateWithXYZ(x, y, z);
					result.Add(new NChartPoint(state, series));
				}
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

		#region INChartScaleLegendDelegate

		public string StringRepresentationOfRange(NSNumber from, NSNumber to, NChartScaleLegend scaleLegend)
		{
			// Perform a conversion of the values in the legend to string.
			// Return null from this method to see the default behavior.
			if (from != null && to == null)
				return string.Format("more than {0:0.#}", from.DoubleValue);
			else if (from == null && to != null)
				return string.Format("less than {0:0.#}", to.DoubleValue);
			else
				return string.Format("from {0:0.#} to {1:0.#}", from.DoubleValue, to.DoubleValue);
		}

		#endregion
	}
}

