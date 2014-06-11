using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTFTableViewer
{
    public class Conversions
    {
        public static float ByteArrayToShortFraction(byte[] arr, int startIndex = 0)
        {
            float sign = (arr[0] >> 7) == 0 ? 1f : -1f;
            int val = arr[startIndex] & 0x7F;
            val = val << 8;
            val += arr[startIndex + 1];

			//float 

			return 0;
        }

        public static float ByteArrayToFixed(byte[] arr, int startIndex = 0)
        {
            short mantissa = ByteArrayToShort(arr, startIndex);
            short fraction = ByteArrayToShort(arr, startIndex + 2);

            return (float)mantissa + ((float)fraction / 16384);
        }

        public static float ByteArrayToF2Dot14(byte[] arr, int startIndex = 0)
        {
            int mantissa = arr[startIndex] >> 6;

            int fraction = arr[startIndex] & 0x3F;
            fraction = fraction << 8;
            fraction += arr[startIndex + 1];

            return (float)mantissa + ((float)fraction / 16384);
        }

        public static short ByteArrayToShort(byte[] arr, int startIndex = 0)
        {
            short sign = (arr[0] >> 7) == 0 ? (short)1 : (short)-1;
            short val = (short)(arr[startIndex] & 0x7F);
            val = (short)(val << 8);
            val += (short)arr[startIndex + 1];
            return (short)(val * sign);
        }

        public static ushort ByteArrayToUnsignedShort(byte[] arr, int startIndex = 0)
        {
            ushort val = (ushort)arr[startIndex];
            val = (ushort)(val << 8);
            val += (ushort)arr[startIndex + 1];
            return val;
        }

        public static int ByteArrayToInt(byte[] arr, int startIndex = 0)
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

        public static uint ByteArrayToUnsignedInt(byte[] arr, int startIndex = 0)
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

        public static string ByteToBinaryString(byte b)
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

        public static string IntToBinaryString(int b)
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
