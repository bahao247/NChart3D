using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using NChart3D_iOS;

namespace Crosshair
{
	public class CrosshairViewController : UIViewController, INChartSeriesDataSource, INChartCrosshairDelegate
	{
		NChartView m_view;
		Random m_rand;

		public CrosshairViewController() : base()
		{
			m_rand = new Random();
		}

		public override void LoadView()
		{
			// Create a chart view that will display the chart.
			m_view = new NChartView();

			// Paste your license key here.
			m_view.Chart.LicenseKey = "";

			// Margin to ensure some free space for the iOS status bar.
			m_view.Chart.CartesianSystem.Margin = new NChartMargin(10.0f, 10.0f, 10.0f, 20.0f);

			// Create series that will be displayed on the chart.
			NChartAreaSeries series = new NChartAreaSeries();

			// Set brush that will fill that series with color.
			series.Brush = new NChartSolidColorBrush(UIColor.FromRGB(97, 206, 231));

			// Set data source for the series.
			series.DataSource = this;

			// Add series to the chart.
			m_view.Chart.AddSeries(series);

			// Create crosshair.
			NChartCrosshair cs = new NChartCrosshair();

			// Set color for crosshair's haris.
			cs.XHair.SetColor(UIColor.FromRGB(255, 0, 0));
			cs.YHair.SetColor(UIColor.FromRGB(0, 0, 255));

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
			m_view.Chart.CartesianSystem.AddCrosshair(cs);

			// Update data in the chart.
			m_view.Chart.UpdateData();

			// Set chart view to the controller.
			this.View = m_view;
		}

		NChartTooltip CreateTooltip()
		{
			NChartTooltip result = new NChartTooltip();

			result.Background = new NChartSolidColorBrush(UIColor.FromRGB(255, 255, 255));
			result.Background.Opacity = 0.9f;
			result.Padding = new NChartMargin(10.0f, 10.0f, 10.0f, 10.0f);
			result.BorderColor = UIColor.FromRGB(128, 128, 128);
			result.BorderThickness = 1.0f;
			result.Font = UIFont.SystemFontOfSize(16.0f);

			return result;
		}

		void UpdateTooltipText(NChartTooltip tooltip, double value)
		{
			tooltip.Text = string.Format("{0:0.##}", value);
		}

		#region INChartSeriesDataSource

		public NChartPoint[] Points(NChartSeries series)
		{
			// Create points with some data for the series.
			List<NChartPoint> result = new List<NChartPoint>();
			for (int i = 0; i <= 10; ++i)
				result.Add(new NChartPoint(NChartPointState.PointStateAlignedToXWithXY(i, m_rand.Next() % 30 + 1), series));
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

		#region INChartCrosshairDelegate

		public void DidBeginMoving(NChartCrosshair crosshair) { }
		public void DidMove(NChartCrosshair crosshair) { UpdateTooltipText(crosshair.XHair.Tooltip, crosshair.XHair.Value); }
		public void DidEndMoving(NChartCrosshair crosshair) { UpdateTooltipText(crosshair.XHair.Tooltip, crosshair.XHair.Value); }

		#endregion
	}
}

