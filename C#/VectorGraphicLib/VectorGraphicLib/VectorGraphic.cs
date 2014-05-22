using System;
using System.IO;
using System.Xml.Linq;

namespace VectorGraphicLib
{
	public class VectorGraphic
	{


		private VectorGraphic ()
		{

		}

		/// <summary>
		/// Parse SVG Data from the given file.
		/// </summary>
		/// <returns>
		/// A VectorGraphic object
		/// </returns>
		/// <param name="path">The path for the target file to parse.</param>
		public static VectorGraphic Load(string path) {
			FileStream stream = File.OpenRead (path);
			return Load (stream);
		}
		/// <summary>
		/// Parse SVG data from a stream.
		/// </summary>
		/// <returns>
		/// A VectorGraphic object
		/// </returns>
		/// <param name="stream">The stream object to parse.</param>
		public static VectorGraphic Load(Stream stream) {
			XElement elem;

			try {
				elem = XElement.Load (stream);
			} catch (Exception ex) {
				throw ex;
			}

			VectorGraphic vg = new VectorGraphic ();




			return vg;
		}
	}
}

