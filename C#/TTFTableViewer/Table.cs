using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTFTableViewer
{
    public class Table
    {
        public static void ProcessAcnt(byte[] table, int startPoint,
            out float versionNum,
            out ushort firstAccentGlyphIndex,
            out ushort lastAccentGlyphIndex,
            out uint descriptionOffset,
            out uint extensionOffset,
            out uint secondaryOffset,
            out byte[] glyphs,
            out byte[] ext,
            out byte[] accents)
        {
            versionNum = Conversions.ByteArrayToFixed(table, startPoint);
            firstAccentGlyphIndex = Conversions.ByteArrayToUnsignedShort(table, startPoint + 4);
            lastAccentGlyphIndex = Conversions.ByteArrayToUnsignedShort(table, startPoint + 6);
            descriptionOffset = Conversions.ByteArrayToUnsignedInt(table, startPoint + 8);
            extensionOffset = Conversions.ByteArrayToUnsignedInt(table, startPoint + 12);
            secondaryOffset = Conversions.ByteArrayToUnsignedInt(table, startPoint + 16);

            glyphs = null;
            ext = null;
            accents = null;
        }

        public static void ProcessAvar(byte[] table, int startPoint)
        {

        }
    }
}
