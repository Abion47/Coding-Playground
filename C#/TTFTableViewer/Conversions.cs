using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTFTableViewer
{
    public class Conversions
    {
        static float ByteArrayToFixed(byte[] arr)
        {
            short mantissa = ByteArrayToShort(arr, 0);
            short fraction = ByteArrayToShort(arr, 2);

            return (float)mantissa + ((float)fraction / 16384);
        }

        static float ByteArrayToF2Dot14(byte[] arr)
        {
            int mantissa = arr[0] >> 6;

            int fraction = arr[0] & 0x3F;
            fraction = fraction << 8;
            fraction += arr[1];

            return (float)mantissa + ((float)fraction / 16384);
        }

        static short ByteArrayToShort(byte[] arr, int startIndex = 0)
        {
            short sign = (arr[0] >> 7) == 0 ? (short)1 : (short)-1;
            short val = (short)(arr[startIndex] & 0x7F);
            val = (short)(val << 8);
            val += (short)arr[startIndex + 1];
            return (short)(val * sign);
        }

        static ushort ByteArrayToUnsignedShort(byte[] arr, int startIndex = 0)
        {
            ushort val = (ushort)arr[startIndex];
            val = (ushort)(val << 8);
            val += (ushort)arr[startIndex + 1];
            return val;
        }

        static int ByteArrayToInt(byte[] arr, int startIndex = 0)
        {
            int sign = (arr[0] >> 7) == 0 ? 1 : -1;
            int val = arr[startIndex] & 0x7F;
            val = val << 8;
            val += arr[startIndex + 1];
            val = val << 8;
            val += arr[startIndex + 2];
            val = val << 8;
            val += arr[startIndex + 3];
            return val * sign;
        }

        static uint ByteArrayToUnsignedInt(byte[] arr, int startIndex = 0)
        {
            uint val = arr[startIndex];
            val = val << 8;
            val += arr[startIndex + 1];
            val = val << 8;
            val += arr[startIndex + 2];
            val = val << 8;
            val += arr[startIndex + 3];
            return val ;
        }

        // ==================== To String =========================

        static string ByteToBinaryString(byte b)
        {
            StringBuilder sb = new StringBuilder();
            byte i = b;

            while (i > 0)
            {
                sb.Insert(0, i & 1);
                i = (byte)(i >> 1);
            }

            while (sb.Length < 8)
                sb.Insert(0, 0);

            return sb.ToString();
        }

        static string IntToBinaryString(int b)
        {
            StringBuilder sb = new StringBuilder();
            int i = b;

            while (i > 0)
            {
                sb.Insert(0, i & 1);
                i = (byte)(i >> 1);
            }

            return sb.ToString();
        }
    }
}
