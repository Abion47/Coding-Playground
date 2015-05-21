using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace org.general
{
    public static class GlobalSettings
    {
        public static PixelFormat DefaultPixelFormat = PixelFormat.Format32bppArgb;
        public static int DefaultPixelFormatBpp = 4;
        public static bool IsPixelOrderReversed;

        public static string TestFileInputDirectory = "input\\";
        public static string TestFileOutputDirectory = "output\\";

        static GlobalSettings()
        {
            IsPixelOrderReversed = BitConverter.IsLittleEndian;

            if (!Directory.Exists(TestFileInputDirectory))
                Directory.CreateDirectory(TestFileInputDirectory);

            if (!Directory.Exists(TestFileOutputDirectory))
                Directory.CreateDirectory(TestFileOutputDirectory);
        }
    }
}
