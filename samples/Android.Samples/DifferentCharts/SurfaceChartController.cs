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
	public class SurfaceChartController : Java.Lang.Object, INChartSeriesDataSource
	{
		NChartView mNChartView;

		public SurfaceChartController(NChartView view)
		{
			mNChartView = view;
		}

		public void UpdateData()
		{
			// Switch on antialiasing.
			mNChartView.Chart.ShouldAntialias = true;

			// Switch 3D on.
			mNChartView.Chart.DrawIn3D = true;
			mNChartView.Chart.CartesianSystem.Margin = new NChartMargin(50.0f, 50.0f, 10.0f, 20.0f);
			mNChartView.Chart.PolarSystem.Margin = new NChartMargin(50.0f, 50.0f, 10.0f, 20.0f);

			// Create series that will be displayed on the chart.
			CreateSeries();

			// Update data in the chart.
			mNChartView.Chart.UpdateData();
		}

		void CreateSeries()
		{
			NChartSurfaceSeries series = new NChartSurfaceSeries();
			series.DataSource = this;
			series.Tag = 0;
			series.Brush = new NChartSolidColorBrush(Color.Argb(255, (int)(0.38 * 255), (int)(0.8 * 255), (int)(0.91 * 255)));
			mNChartView.Chart.AddSeries(series);

			mNChartView.Chart.CartesianSystem.XAxis.HasOffset = false;
			mNChartView.Chart.CartesianSystem.YAxis.HasOffset = false;
			mNChartView.Chart.CartesianSystem.ZAxis.HasOffset = false;
		}

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();

			for (int i = 0, n = 20; i < n; ++i)
			{
				for (int j = 0, m = 20; j < m; ++j)
				{
					double x = (double)(i) * 2.0 * System.Math.PI / (double)n;
					double z = (double)(j) * 2.0 * System.Math.PI / (double)m;
					double y = System.Math.Sin(x) * System.Math.Cos(z);

					NChartPointState state = NChartPointState.PointStateWithXYZ(i, y, j);
					result.Add(new NChartPoint(state, series));
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


