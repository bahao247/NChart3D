using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;

using NChart3D_Android;

namespace Callout
{
	[Activity(Label = "Callout", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, INChartSeriesDataSource
	{
		NChartView mNChartView;
		Random random = new Random();
		NChartBrush[] brushes;

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

			// Margin
			mNChartView.Chart.PolarSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 40.0f);

			// Create brushes.
			brushes = new NChartBrush[3];
			brushes[0] = new NChartSolidColorBrush(Color.Argb(255, (int)(255 * 0.38), (int)(255 * 0.8), (int)(255 * 0.92)));
			brushes[1] = new NChartSolidColorBrush(Color.Argb(255, (int)(255 * 0.8), (int)(255 * 0.86), (int)(255 * 0.22)));
			brushes[2] = new NChartSolidColorBrush(Color.Argb(255, (int)(255 * 0.9), (int)(255 * 0.29), (int)(255 * 0.51)));

			for (int i = 0; i < 3; ++i)
			{
				// Create series that will be displayed on the chart.
				NChartPieSeries series = new NChartPieSeries();

				// Set data source for the series.
				series.DataSource = this;

				// Set tag of the series.
				series.Tag = i;

				// Set brush that will fill that series with color.
				series.Brush = brushes[i % brushes.Length];

				// Add series to the chart.
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

		NChartCallout CreateCallout(string text, int dataIndex)
		{
			NChartCallout result = new NChartCallout();

			result.Background = new NChartSolidColorBrush(Color.Argb(255, 255, 255, 255));
			result.Background.Opacity = 0.9;
			result.Padding = new NChartMargin(10.0f, 10.0f, 10.0f, 10.0f);
			result.BorderColor = Color.Argb(255, 128, 128, 128);
			result.BorderThickness = 1;
			result.Font = new NChartFont(16);
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
			result[0] = new NChartPoint(NChartPointState.PointStateWithCircleValue(0, random.Next() % 30 + 1), series);
			result[0].Tooltip = CreateCallout(string.Format("Callout for series {0}", series.Tag + 1), series.Tag);
			return result;
		}

		public string Name(NChartSeries series)
		{
			return string.Format("Series {0}", series.Tag + 1);
		}

		// If you don't want to implement method, return null.
		public Bitmap Image(NChartSeries series) { return null; }
		public NChartPoint[] ExtraPoints(NChartSeries series) { return null; }

		#endregion
	}
}


