using System;

namespace TTFTableViewer.Tables
{
	#region Required
	public struct CMAP {

	}

	public struct GLYF {
		public short NumberOfContours;
		public FWord xMin;
		public FWord yMin;
		public FWord xMax;
		public FWord yMax;
	}

	public struct HEAD {

	}

	public struct HHEA {

	}

	public struct HMTX {

	}

	public struct LOCA {

	}

	public struct MAXP {

	}

	public struct NAME {

	}

	public struct POST {

	}

	public struct OS2 {

	}
	#endregion

	#region Other 
	public struct CVT {
		public List<FWord> Values;
	}

	public struct EBDT {
		public Fixed Version;
		public byte[] TableData;
	}

	public struct EBLC {
		public Fixed Version;
		public byte[] TableData;
	}

	public struct EBSC {
		public Fixed Version;
		public byte[] TableData;
	}

	public struct FPGM {
		public List<byte> Instructions;
	}

	public struct GASP {
		public ushort Version;
		public ushort NumRanges;
		public TableSubtypes.GASPRANGE[] GaspRanges;
	}

	public struct HDMX {

	}

	public struct KERN {

	}

	public struct LTSH {

	}

	public struct PREP {

	}

	public struct PCLT {

	}

	public struct VDMX {

	}

	public struct VHEA {

	}

	public struct VMTX {

	}

	public class TableSubtypes {
		public struct GASPRANGE {
			public ushort RangeMaxPPEM;
			public ushort RangeGaspBehavior;
		}
	}

	public class TableFlags {
		public enum GASP_Flag_Behavior {
			NONE = 0x0000,
			GASP_GRIDFIT = 0x0001,
			GASP_DOGRAY = 0x0002
		}
	}

	public static class TableUtil {
		public static unsafe ulong CalcChecksum(ulong* table, ulong length) {
			ulong sum = 0L;
			ulong* endptr = table + ((length + 3) & ~3UL) / sizeof(ulong);

			while (table < endptr)
				sum += *table++;

			return sum;
		}
	}
	#endregion
}

