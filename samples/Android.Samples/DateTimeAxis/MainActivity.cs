using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;
using Java.Util;
using Java.Text;
using Random = System.Random;

using NChart3D_Android;

namespace DateTimeAxis
{
	[Activity(Label = "DateTimeAxis", MainLauncher = true, Icon = "@drawable/icon")]
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

			// Set the data source for X-Axis to perform custom date to string conversion.
			mNChartView.Chart.CartesianSystem.XAxis.DataSource = this;

			// Turn on the date time mode of X-Axis.
			mNChartView.Chart.CartesianSystem.XAxis.HasDates = true;

			// Margin to ensure some free space for the iOS status bar.
			mNChartView.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// Create column series with colors from the array and add them to the chart.
			NChartColumnSeries series = new NChartColumnSeries();
			series.Brush = new NChartSolidColorBrush(Color.Argb(255, 97, 205, 232));
			series.DataSource = this;
			mNChartView.Chart.AddSeries(series);

			// Update data in the chart.
			mNChartView.Chart.UpdateData();

			// Calculate optimal tick spacing to display dates avoiding overlapping.
			mNChartView.Chart.CartesianSystem.XAxis.CalcOptimalMinTickSpacing();
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
			// Create chart points with dates by X-Axis.
			const int numberOfDays = 8;
			const int startDay = 2;
			NChartPoint[] result = new NChartPoint[numberOfDays];
			Calendar cal = Calendar.Instance;
			cal.Set(CalendarField.Year, 2015);
			cal.Set(CalendarField.Month, 1);
			cal.Set(CalendarField.DayOfMonth, startDay);
			cal.Set(CalendarField.HourOfDay, 12);
			Date startDate = cal.Time;
			for (int i = 0; i < numberOfDays; ++i)
			{
				cal.Set(CalendarField.DayOfMonth, startDay + i);
				Date date = cal.Time;
				result[i] = new NChartPoint(NChartPointState.PointStateAlignedToXWithXY(date, random.Next(30) + 1), series);
			}
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

		#region INChartValueAxisDataSource

		public string DateToString(Date nDate, double v, NChartValueAxis nChartValueAxis)
		{
			// Perform date to string conversion. Let's perform it by the following rule:
			// - if tick interval is more than one day, show localized string "Day Month Year";
			// - otherwise show time string "Month.Day.Year Hours:Minutes".
			if (v >= 86400000.0)
			{
				return new SimpleDateFormat("d MMM yyyy").Format(nDate);
			}
			else {
				return new SimpleDateFormat("MM.dd.yyyy HH:mm").Format(nDate);
			}
		}

		// If you don't want to implement method, return null.
		public string Name(NChartValueAxis nChartValueAxis) { return null; }
		public Number Min(NChartValueAxis nChartValueAxis) { return null; }
		public Number Max(NChartValueAxis nChartValueAxis) { return null; }
		public Number Step(NChartValueAxis nChartValueAxis) { return null; }
		public string[] Ticks(NChartValueAxis nChartValueAxis) { return null; }
		public string[] ExtraTicks(NChartValueAxis nChartValueAxis) { return null; }
		public Number Length(NChartValueAxis nChartValueAxis) { return null; }
		public string DoubleToString(double v, NChartValueAxis nChartValueAxis) { return null; }
		public Date MinDate(NChartValueAxis nChartValueAxis) { return null; }
		public Date MaxDate(NChartValueAxis nChartValueAxis) { return null; }
		public Number DateStep(NChartValueAxis nChartValueAxis) { return null; }

		#endregion
	}
}


