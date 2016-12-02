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
	public class AreaChartController : Java.Lang.Object, INChartSeriesDataSource
	{
		NChartView mNChartView;
		Random random = new Random();

		public bool DrawIn3D { get; set; }

		public AreaChartController(NChartView view)
		{
			mNChartView = view;
		}

		public void UpdateData()
		{
			// Switch on antialiasing.
			mNChartView.Chart.ShouldAntialias = true;

			if (DrawIn3D)
			{
				// Switch 3D on.
				mNChartView.Chart.DrawIn3D = true;
				mNChartView.Chart.CartesianSystem.Margin = new NChartMargin(50.0f, 50.0f, 10.0f, 20.0f);
				mNChartView.Chart.PolarSystem.Margin = new NChartMargin(50.0f, 50.0f, 10.0f, 20.0f);
			}
			else {
				mNChartView.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
				mNChartView.Chart.PolarSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);
			}

			// Create series that will be displayed on the chart.
			CreateSeries();

			// Update data in the chart.
			mNChartView.Chart.UpdateData();
		}

		void CreateSeries()
		{
			NChartAreaSeries series = new NChartAreaSeries();
			series.DataSource = this;
			series.Tag = 0;
			series.Brush = new NChartSolidColorBrush(Color.Argb(255, (int)(0.38 * 255), (int)(0.8 * 255), (int)(0.91 * 255)));
			mNChartView.Chart.AddSeries(series);

			mNChartView.Chart.CartesianSystem.XAxis.HasOffset = true;
			mNChartView.Chart.CartesianSystem.YAxis.HasOffset = false;
			mNChartView.Chart.CartesianSystem.ZAxis.HasOffset = true;
		}

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();


			if (DrawIn3D)
			{
				for (int i = 0; i <= 10; ++i)
					result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXZWithXYZ(i, random.Next(30) + 1, series.Tag), series));
			}
			else {
				for (int i = 0; i <= 10; ++i)
					result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXWithXY(i, random.Next(30) + 1), series));
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


