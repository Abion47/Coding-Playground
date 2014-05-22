using System;
using System.Collections.Generic;
using Cairo;

namespace VectorGraphicLib
{
	public class Line : Base.Shape<Line>
	{
		public Line(double x1, double y1, double x2, double y2)
			: this(new PointD(x1, y1), new PointD(x2, y2)) {}
		public Line (PointD a, PointD b)
			: base()
		{
			_pts.AddRange (new PointD[] { a, b });
			_tag = "l";
		}

		public override void AddPoint(PointD pt) {}
		public override void AddPoint(Point pt) {}
		public override void AddPoint(double x, double y) {}

		public override void InsertPoint(int index, Point pt) {}
		public override void InsertPoint(int index, PointD pt) {}
		public override void InsertPoint(int index, double x, double y) {}

		public override void RemovePoint(PointD pt) {}
		public override void RemovePointAt(int index) {}

		public override Line Parse(System.Xml.Linq.XElement str) 
		{


			return null;
		}

		public override bool Draw (Context g)
		{
			g.Save ();

			g.SetSourceRGBA (_strokeColor.R, _strokeColor.G, _strokeColor.B, _strokeColor.A);
			g.LineWidth = _strokeWidth;
			g.LineCap = _strokeCap;
			g.LineJoin = _strokeJoin;
			g.MiterLimit = _miterLimit;

			g.MoveTo (_pts [0]);
			g.LineTo (_pts [1]);
			g.Stroke ();

			g.Restore ();
			return true;
		}
	}
}

