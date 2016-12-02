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
	public class BandChartController : Java.Lang.Object, INChartSeriesDataSource
	{
		NChartView mNChartView;
		Random random = new Random();

		public BandChartController(NChartView view)
		{
			mNChartView = view;
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
			NChartBandSeries series = new NChartBandSeries();
			series.DataSource = this;
			series.Tag = 0;
			series.PositiveColor = Color.Argb((int)(255 * 0.8), (int)(255 * 0.41), (int)(255 * 0.67), (int)(255 * 0.95));
			series.NegativeColor = Color.Argb((int)(255 * 0.8), (int)(255 * 0.77), (int)(255 * 0.94), (int)(255 * 0.36));
			series.HighBorderColor = Color.Argb((int)(255 * 0.8), (int)(255 * 0.51), (int)(255 * 0.78), (int)(255 * 1.0));
			series.LowBorderColor = Color.Argb((int)(255 * 0.8), (int)(255 * 0.89), (int)(255 * 1.0), (int)(255 * 0.44));

			series.BorderThickness = 5.0f;
			mNChartView.Chart.AddSeries(series);

			mNChartView.Chart.CartesianSystem.XAxis.HasOffset = true;
			mNChartView.Chart.CartesianSystem.YAxis.HasOffset = false;
			mNChartView.Chart.CartesianSystem.ZAxis.HasOffset = true;
		}

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();

			for (int i = 0; i < 10; ++i)
			{
				double low = random.Next(20);
				double high = random.Next(20);
				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXWithXLowHigh(i, low, high), series));
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


