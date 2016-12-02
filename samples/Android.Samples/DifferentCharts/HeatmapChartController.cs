using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;
using System.Collections.Generic;

using NChart3D_Android;

namespace DifferentCharts
{
	public class HeatmapChartController : Java.Lang.Object, INChartSeriesDataSource
	{
		NChartView mNChartView;
		NChartBrush[] brushes;

		public HeatmapChartController(NChartView view)
		{
			mNChartView = view;

			// Create brushes.
			brushes = new NChartBrush[3];
			brushes[0] = new NChartSolidColorBrush(Color.Argb(255, (int)(0.38 * 255), (int)(0.8 * 255), (int)(0.91 * 255)));
			brushes[1] = new NChartSolidColorBrush(Color.Argb(255, (int)(0.8 * 255), (int)(0.86 * 255), (int)(0.22 * 255)));
			brushes[2] = new NChartSolidColorBrush(Color.Argb(255, (int)(0.9 * 255), (int)(0.29 * 255), (int)(0.51 * 255)));
		}

		public void UpdateData()
		{
			// Switch on antialiasing.
			mNChartView.Chart.ShouldAntialias = true;

			mNChartView.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
			mNChartView.Chart.PolarSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// Create series that will be displayed on the chart.
			CreateSeries();

			// Update data in the chart.
			mNChartView.Chart.UpdateData();
		}

		void CreateSeries()
		{
			NChartHeatmapSeries series = new NChartHeatmapSeries();
			series.DataSource = this;
			series.Tag = 0;

			// Create brush scale.
			// See NChartBrushScale for details.
			series.Scale = new NChartBrushScale(brushes, new Number[] { (Number)(-0.3), (Number)(0.3) });

			mNChartView.Chart.AddSeries(series);
			mNChartView.Chart.CartesianSystem.XAxis.ShouldBeautifyMinAndMax = false;
			mNChartView.Chart.CartesianSystem.YAxis.ShouldBeautifyMinAndMax = false;
			mNChartView.Chart.CartesianSystem.XAxis.HasOffset = false;
			mNChartView.Chart.CartesianSystem.YAxis.HasOffset = false;
		}

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();

			for (int i = 0, n = 75; i <= n; ++i)
			{
				for (int j = 0, m = 75; j <= m; ++j)
				{
					double x = 1.0 - 2.0 * (double)(i) / (double)(n);
					double y = 1.0 - 2.0 * (double)(j) / (double)(m);
					double value = (1.0 - System.Math.Abs(x * y)) * System.Math.Sin((1.0 - System.Math.Abs(x * y)) * System.Math.PI * 4.0);
					result.Add(new NChartPoint(NChartPointState.PointStateWithXYValue(x, y, value), series));
				}
			}

			return result.Count > 0 ? result.ToArray() : null;
		}

		public NChartPoint[] ExtraPoints(NChartSeries series)
		{
			return null;
		}

		public string Name(NChartSeries series)
		{
			return string.Format("My series {0}", series.Tag + 1);
		}

		public Bitmap Image(NChartSeries series)
		{
			return null;
		}
	}
}


