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

namespace Crosshair
{
	[Activity(Label = "Crosshair", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, INChartSeriesDataSource, INChartCrosshairDelegate
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

			// Create area series with colors and add them to the chart.
			NChartAreaSeries series = new NChartAreaSeries();
			series.Brush = new NChartSolidColorBrush(Color.Argb(255, 97, 205, 232));
			series.DataSource = this;
			mNChartView.Chart.AddSeries(series);

			// Create crosshair.
			NChartCrosshair cs = new NChartCrosshair();

			// Set color for crosshair's haris.
			cs.XHair.SetColor(Color.Argb(255, 255, 0, 0));
			cs.YHair.SetColor(Color.Argb(255, 0, 0, 255));

			// Set thickness for crosshair's hairs.
			cs.Thickness = 2.0f;

			// Set value for crosshair. Alternatively it's possible to create crosshair connected to chart point using the method
			// new NChartCrosshair (Color color, float thickness, NChartPoint targetPoint);
			cs.XHair.Value = 5.25;
			cs.YHair.Value = 13.0;

			// Set crosshair delegate to handle moving.
			cs.Delegate = this;

			// Set crosshair snapping to X-Axis ticks.
			cs.XHair.SnapToMinorTicks = true;
			cs.XHair.SnapToMajorTicks = true;

			// Add tooltip to the hair parallel to Y-Axis.
			cs.XHair.Tooltip = CreateTooltip();
			UpdateTooltipText(cs.XHair.Tooltip, cs.XHair.Value);

			// Add crosshair to the chart's coordinate system.
			mNChartView.Chart.CartesianSystem.AddCrosshair(cs);

			// Update data in the chart.
			mNChartView.Chart.UpdateData();
		}

		NChartTooltip CreateTooltip()
		{
			NChartTooltip result = new NChartTooltip();

			result.Background = new NChartSolidColorBrush(Color.Argb(255, 255, 255, 255));
			result.Background.Opacity = 0.9f;
			result.Padding = new NChartMargin(10.0f, 10.0f, 10.0f, 10.0f);
			result.BorderColor = Color.Argb(255, 128, 128, 128);
			result.BorderThickness = 1.0f;
			result.Font = new NChartFont(16.0f);

			return result;
		}

		void UpdateTooltipText(NChartTooltip tooltip, double value)
		{
			tooltip.Text = string.Format("{0:0.##}", value);
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
			return "My series";
		}

		// If you don't want to implement method, return null.
		public Bitmap Image(NChartSeries series) { return null; }
		public NChartPoint[] ExtraPoints(NChartSeries series) { return null; }

		#endregion

		#region INChartCrosshairDelegate

		public void DidBeginMoving(NChartCrosshair crosshair) { }

		public void DidMove(NChartCrosshair crosshair)
		{
			UpdateTooltipText(crosshair.XHair.Tooltip, crosshair.XHair.Value);
		}

		public void DidEndMoving(NChartCrosshair crosshair)
		{
			UpdateTooltipText(crosshair.XHair.Tooltip, crosshair.XHair.Value);
		}

		#endregion
	}
}


