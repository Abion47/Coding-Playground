using System;
using System.Collections.Generic;
using Cairo;

namespace VectorGraphicLib
{
	public class Polygon : Base.Shape<Polygon>
	{
		public Polygon (params double[] coords)
			: base()
		{
			if (coords.Length % 2 != 0)
				throw new ArgumentException ("The amount of given coordinates must be an even number.");

			for (int i = 0; i < coords.Length; i += 2)
				_pts.Add (new PointD (coords [i], coords [i + 1]));
		}
		public Polygon (params PointD[] pts)
			: base()
		{
			_pts.AddRange(pts);
			_tag = "path";
		}

		public override Polygon Parse(System.Xml.Linq.XElement str) 
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

