using System;
using System.Drawing;
using UIKit;
using Foundation;
using NChart3D_iOS;
using CoreGraphics;

namespace MultipleViews
{
	public class MultipleViewsView : UIView
	{
		public NChartView view1;
		public NChartView view2;

		public MultipleViewsView() : base()
		{
			this.view1 = new NChartView();
			this.AddSubview(view1);
			this.view2 = new NChartView();
			this.AddSubview(view2);

			this.BackgroundColor = UIColor.White;
		}

		public override void LayoutSubviews()
		{
			float mrg = 20.0f; // Magin for iOS' status bar

			// Place views side by side in landscape interface orientation and one over another in portrait interface orientation
			if (this.Bounds.Size.Width > this.Bounds.Size.Height)
			{
				CGSize size = new CGSize((float)Math.Round(this.Bounds.Size.Width / 2.0f), (float)Math.Round(this.Bounds.Size.Height - mrg));
				this.view1.Frame = new CGRect(0, mrg, size.Width, size.Height);
				this.view2.Frame = new CGRect((int)size.Width, (int)mrg, (int)size.Width, (int)size.Height);
			}
			else {
				CGSize size = new CGSize(this.Bounds.Size.Width, (float)Math.Round((this.Bounds.Size.Height - mrg) / 2.0f));
				this.view1.Frame = new CGRect(0, mrg, size.Width, size.Height);
				this.view2.Frame = new CGRect(0, (size.Height + mrg), size.Width, size.Height);
			}
		}
	}
}
