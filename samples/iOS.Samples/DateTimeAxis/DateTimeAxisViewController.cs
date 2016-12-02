using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace DateTimeAxis
{
	public class DateTimeAxisViewController : UIViewController, INChartSeriesDataSource, INChartValueAxisDataSource
	{
		NChartView m_view;
		Random m_rand;

		public DateTimeAxisViewController() : base()
		{
			m_rand = new Random();
		}

		public override void LoadView()
		{
			// Create a chart view that will display the chart.
			m_view = new NChartView();

			// Paste your license key here.
			m_view.Chart.LicenseKey = "";

			// Set the data source for X-Axis to perform custom date to string conversion.
			m_view.Chart.CartesianSystem.XAxis.DataSource = this;

			// Turn on the date time mode of X-Axis.
			m_view.Chart.CartesianSystem.XAxis.HasDates = true;

			// Margin to ensure some free space for the iOS status bar.
			m_view.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// Create series that will be displayed on the chart.
			NChartColumnSeries series = new NChartColumnSeries();

			// Set brush that will fill that series with color.
			series.Brush = new NChartSolidColorBrush(UIColor.FromRGBA(97, 206, 231, 255));

			// Set data source for the series.
			series.DataSource = this;

			// Add series to the chart.
			m_view.Chart.AddSeries(series);

			// Update data in the chart.
			m_view.Chart.UpdateData();

			// Calculate optimal tick spacing to display dates avoiding overlapping.
			m_view.Chart.CartesianSystem.XAxis.CalcOptimalMinTickSpacing();

			// Set chart view to the controller.
			this.View = m_view;
		}

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();
			int numberOfDays = 8;
			int startDay = 2;
			NSCalendar cal = NSCalendar.CurrentCalendar;
			NSDateComponents comp = new NSDateComponents();
			comp.Year = 2015;
			comp.Month = 1;
			comp.Day = 2;
			NSDate startDate = cal.DateFromComponents(comp);

			for (int i = 0; i < numberOfDays; ++i)
			{
				NSDate date = cal.DateBySettingUnit(NSCalendarUnit.Day, startDay + i, startDate, NSCalendarOptions.None);
				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXWithXY(date, m_rand.Next() % 30 + 1), series));
			}

			return result.ToArray();
		}

		public string Name(NChartSeries series)
		{
			// Get name of the series.
			return "My series";
		}

		// If you don't want to implement method, return null.
		public UIImage Image(NChartSeries series) { return null; }
		public NChartPoint[] ExtraPoints(NChartSeries series) { return null; }

		#endregion

		#region INChartValueAxisDataSource

		public string DateToString(NSDate date, double value, NChartValueAxis axis)
		{
			// Perform date to string conversion. Let's perform it by the following rule:
			// - if tick interval is more than one day, show localized string "Day Month Year";
			// - otherwise show time string "Month.Day.Year Hours:Minutes".
			NSDateFormatter formatter = new NSDateFormatter();
			if (value >= 86400.0)
			{
				formatter.DateFormat = "d MMM YYYY";
			}
			else {
				formatter.DateFormat = "MM.dd.YYYY HH:mm";
			}
			return formatter.StringFor(date);
		}

		// If you don't want to implement method, return null.
		public string[] Ticks(NChartValueAxis axis) { return null; }
		public string[] ExtraTicks(NChartValueAxis axis) { return null; }
		public string DoubleToString(double value, NChartValueAxis axis) { return null; }
		public NSNumber Length(NChartValueAxis axis) { return null; }
		public NSNumber Max(NChartValueAxis axis) { return null; }
		public NSNumber Min(NChartValueAxis axis) { return null; }
		public string Name(NChartValueAxis axis) { return null; }
		public NSNumber Step(NChartValueAxis axis) { return null; }
		public NSDate MinDate(NChartValueAxis axis) { return null; }
		public NSDate MaxDate(NChartValueAxis axis) { return null; }
		public NSNumber DateStep(NChartValueAxis axis) { return null; }

		#endregion
	}
}

