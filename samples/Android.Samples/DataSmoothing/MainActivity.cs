using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;

using NChart3D_Android;

namespace DataSmoothing
{
	[Activity(Label = "DataSmoothing", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, INChartSeriesDataSource
	{
		NChartView mNChartView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			mNChartView = FindViewById<NChartView>(Resource.Id.surface);

			LoadView();
		}

		private void LoadView()
		{
			// Paste your license key here.
			mNChartView.Chart.LicenseKey = "";

			// Margin to ensure some free space for the iOS status bar.
			mNChartView.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// Switch on antialiasing.
			mNChartView.Chart.ShouldAntialias = true;

			// Create series that will be smoothed.
			{
				NChartLineSeries series = new NChartLineSeries();
				series.Tag = 1;
				series.Brush = new NChartSolidColorBrush(Color.Argb(255, 100, 200, 220));
				series.LineThickness = 2.0f;
				series.DataSource = this;

				// Set data smoother. You can experiment with different data smoothers to see how they work.
				//series.dataSmoother = new NChartDataSmootherSpline ();
				//series.dataSmoother = new NChartDataSmootherSBezier ();
				series.DataSmoother = new NChartDataSmootherTBezier();

				mNChartView.Chart.AddSeries(series);
			}

			// Create series with no smoothing.
			{
				NChartLineSeries series = new NChartLineSeries();
				series.Tag = 2;
				series.Brush = new NChartSolidColorBrush(Color.Red);
				series.LineThickness = 1.0f;
				series.DataSource = this;
				series.Marker = new NChartMarker();
				series.Marker.Shape = NChartMarkerShape.Circle;
				series.Marker.Size = 5;
				series.Marker.Brush = new NChartSolidColorBrush(Color.Red);
				mNChartView.Chart.AddSeries(series);
			}

			// Update data in the chart.
			mNChartView.Chart.UpdateData();
		}

		protected override void OnResume()
		{
			base.OnResume();
			mNChartView.OnResume();
		}

		protected override void OnPause()
		{
			base.OnPause();
			mNChartView.OnPause();
		}

		int[,] data = new int[,] {
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

		#region INChartSeriesDataSource

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
		public Bitmap Image(NChartSeries series) { return null; }
		public NChartPoint[] ExtraPoints(NChartSeries series) { return null; }

		#endregion
	}
}


