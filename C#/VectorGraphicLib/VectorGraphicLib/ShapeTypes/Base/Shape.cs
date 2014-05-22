using System;
using System.Collections.Generic;
using Cairo;

namespace VectorGraphicLib.Base
{
	public abstract class Shape<T>
	{
		// Structure Information
		protected int _zOrder;

		// Generic Information
		protected string _tag;
		protected List<PointD> _pts;

		// Stroke Information
		protected Color _strokeColor;
		protected double _strokeWidth;
		protected LineCap _strokeCap;
		protected LineJoin _strokeJoin;
		protected double _miterLimit;

		// Fill Information
		protected Color _fillColor;
		protected FillRule _fillRule;

		// Getter/Setter Properties
		public int ZOrder {
			get { return this._zOrder; }
			set { this._zOrder = value; }
		}
		public string Tag {
			get { return this._tag; }
		}
		public Color StrokeColor {
			get { return this._strokeColor; }
			set { this._strokeColor = value; }
		}
		public double StrokeWidth {
			get { return this._strokeWidth; }
			set { this._strokeWidth = value; }
		}
		public LineCap StrokeCap {
			get { return this._strokeCap; }
			set { this._strokeCap = value; }
		}
		public LineJoin StrokeJoin {
			get { return this._strokeJoin; }
			set { this._strokeJoin = value; }
		}
		public double MiterLimit {
			get { return this._miterLimit; }
			set { this._miterLimit = value; }
		}
		public Color FillColor {
			get { return this._fillColor; }
			set { this._fillColor = value; }
		}
		public FillRule FillRule {
			get { return this._fillRule; }
			set { this._fillRule = value; }
		}

		// Indexer
		public PointD this[int index] { 
			get { return this._pts [index]; }
			set { this._pts [index] = value; }
		}

		// Enumerator
		public IEnumerator<PointD> Points() {
			foreach (var pt in _pts) {
				yield return pt;
			}
		}

		// Base Constructor
		protected Shape() {
			_pts = new List<PointD> ();

			_strokeColor = new Color (0, 0, 0, 0);
			_strokeWidth = 1.0;
			_strokeCap = LineCap.Butt;
			_strokeJoin = LineJoin.Miter;
			_miterLimit = 10.0;

			_fillColor = new Color (0, 0, 0, 0);
			_fillRule = FillRule.Winding;
		}

		// Vertex Setter Methods
		public virtual void AddPoint(PointD pt) { _pts.Add (pt); }
		public virtual void AddPoint(Point pt) { _pts.Add (new PointD (pt.X, pt.Y));}
		public virtual void AddPoint(double x, double y) { _pts.Add (new PointD (x, y));}

		public virtual void InsertPoint(int index, Point pt) { _pts.Insert (index, new PointD(pt.X, pt.Y)); }
		public virtual void InsertPoint(int index, PointD pt) { _pts.Insert (index, pt); }
		public virtual void InsertPoint(int index, double x, double y) { _pts.Insert (index, new PointD(x, y)); }

		public virtual void RemovePoint(PointD pt) { _pts.Remove (pt); }
		public virtual void RemovePointAt(int index) { _pts.RemoveAt (index); }

		// Abstract Methods
		public abstract T Parse(System.Xml.Linq.XElement elem);
		public abstract bool Draw (Context g);
	}
}

