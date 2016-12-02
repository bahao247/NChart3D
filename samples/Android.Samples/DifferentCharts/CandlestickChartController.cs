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
	public class CandlestickChartController : Java.Lang.Object, INChartSeriesDataSource
	{
		NChartView mNChartView;
		Random random = new Random();

		public bool DrawIn3D { get; set; }

		public CandlestickChartController(NChartView view)
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
			NChartCandlestickSeries series = new NChartCandlestickSeries();
			series.DataSource = this;
			series.Tag = 0;
			series.PositiveColor = Color.Argb(255, (int)(255 * 0.28), (int)(255 * 0.88), (int)(255 * 0.55));
			series.PositiveBorderColor = Color.Argb(255, (int)(255 * 0.25), (int)(255 * 0.8), (int)(255 * 0.15));
			series.NegativeColor = Color.Argb(255, (int)(255 * 0.87), (int)(255 * 0.28), (int)(255 * 0.28));
			series.NegativeBorderColor = Color.Argb(255, (int)(255 * 0.78), (int)(255 * 0.1), (int)(255 * 0.2));
			series.BorderThickness = 3.0f;
			mNChartView.Chart.AddSeries(series);

			NChartCandlestickSeriesSettings settings = new NChartCandlestickSeriesSettings();
			settings.CylindersResolution = 20;
			mNChartView.Chart.AddSeriesSettings(settings);
			mNChartView.Chart.CartesianSystem.XAxis.HasOffset = true;
			mNChartView.Chart.CartesianSystem.YAxis.HasOffset = false;
			mNChartView.Chart.CartesianSystem.ZAxis.HasOffset = true;
		}

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();

			for (int i = 0; i < 30; ++i)
			{
				double open = 5.0f * System.Math.Sin((float)i * System.Math.PI / 10.0);
				double close = 5.0f * System.Math.Cos((float)i * System.Math.PI / 10.0);
				double low = System.Math.Min(open, close) - random.Next(3);
				double high = System.Math.Max(open, close) + random.Next(3);

				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXZWithXZLowOpenCloseHigh(i, series.Tag,
					low, open, close, high), series));
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


