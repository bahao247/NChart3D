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
	public class ColumnChartController : Java.Lang.Object, INChartSeriesDataSource
	{
		NChartView mNChartView;
		Random random = new Random();

		public bool DrawIn3D { get; set; }

		NChartBrush[] brushes;

		public ColumnChartController(NChartView view)
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
			for (int i = 0; i < 3; ++i)
			{
				NChartColumnSeries series = new NChartColumnSeries();
				series.DataSource = this;
				series.Tag = i;
				series.Brush = brushes[i];
				mNChartView.Chart.AddSeries(series);
			}
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
				for (int i = 0; i <= 4; ++i)
					for (int j = 0; j <= 4; ++j)
						result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXZWithXYZ(i, random.Next(30) + 1, j), series));
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


