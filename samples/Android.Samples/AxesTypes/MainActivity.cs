﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;
using Java.Util;
using Random = System.Random;

using NChart3D_Android;

namespace AxesTypes
{
	[Activity(Label = "AxesTypes", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, INChartSeriesDataSource, INChartValueAxisDataSource
	{
		NChartView mNChartView;

		Random random = new Random();

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

			// Array of colors for the series.
			int[,] colors = new int[,] {
				{ 114, 201, 224 },
				{ 220, 225, 68 },
				{ 230, 85, 130 }
			};

			// Create column series with colors from the array and add them to the chart.
			for (int i = 0; i < 3; ++i)
			{
				NChartColumnSeries series = new NChartColumnSeries();
				series.Brush = new NChartSolidColorBrush(Color.Argb(255, colors[i, 0], colors[i, 1], colors[i, 2]));
				series.DataSource = this;

				// Tag is used to get data for a particular series in the data source.
				series.Tag = i + 1;

				mNChartView.Chart.AddSeries(series);
			}

			// Set data source for the X-Axis to have custom values on them.
			mNChartView.Chart.CartesianSystem.XAxis.DataSource = this;

			// Set the type of value axes. Uncomment one of the following lines to see what happens.
			//			mNChartView.Chart.CartesianSystem.ValueAxesType = NChartValueAxesType.Absolute; // Default absolute type.
			//			mNChartView.Chart.CartesianSystem.ValueAxesType = NChartValueAxesType.Additive; // Additive type.
			mNChartView.Chart.CartesianSystem.ValueAxesType = NChartValueAxesType.Percent; // Percent type.

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

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			NChartPoint[] result = new NChartPoint[11];
			for (int i = 0; i <= 10; ++i)
				result[i] = new NChartPoint(NChartPointState.PointStateAlignedToXWithXY(i, random.Next(30) + 1), series);
			return result;
		}

		public string Name(NChartSeries series)
		{
			return string.Format("My series {0}", series.Tag);
		}

		// If you don't want to implement method, return null.
		public Bitmap Image(NChartSeries series) { return null; }
		public NChartPoint[] ExtraPoints(NChartSeries series) { return null; }

		#endregion

		#region INChartValueAxisDataSource

		public string[] Ticks(NChartValueAxis nChartValueAxis)
		{
			// Choose ticks by the kind of axis.
			if (nChartValueAxis.Kind.Ordinal() == NChartValueAxisKind.X.Ordinal())
				// Return 10 ticks for the X-Axis representing, let us say, 10 years.
				return new string[] { "2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009" };
			else
				// Other axes have no ticks.
				return null;
		}

		// If you don't want to implement method, return null.
		public string Name(NChartValueAxis nChartValueAxis) { return null; }
		public Number Min(NChartValueAxis nChartValueAxis) { return null; }
		public Number Max(NChartValueAxis nChartValueAxis) { return null; }
		public Number Step(NChartValueAxis nChartValueAxis) { return null; }
		public string[] ExtraTicks(NChartValueAxis nChartValueAxis) { return null; }
		public Number Length(NChartValueAxis nChartValueAxis) { return null; }
		public string DoubleToString(double v, NChartValueAxis nChartValueAxis) { return null; }
		public Date MinDate(NChartValueAxis nChartValueAxis) { return null; }
		public Date MaxDate(NChartValueAxis nChartValueAxis) { return null; }
		public Number DateStep(NChartValueAxis nChartValueAxis) { return null; }
		public string DateToString(Date nDate, double v, NChartValueAxis nChartValueAxis) { return null; }

		#endregion
	}
}


