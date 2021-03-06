﻿using org.general;
using org.general.Units;
using org.general.Units.Graphics;
using org.general.Units.Numerical;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace org.execute
{
    public class OneShotPrograms
    {
        public static void GetJfifResolutionData()
        {
            string filename = "circle.jpg";

            byte[] bytes = File.ReadAllBytes(filename);

            switch (bytes[13])
            {
                case 0x00:
                    Console.WriteLine("Aspect Ratio: Ratio set as " + (bytes[14] + bytes[15]) + " by " + (bytes[16] + bytes[17]) + ".");
                    break;
                case 0x01:
                    Console.WriteLine("Specified unit is dots per inch. Horizontal resolution: " + (bytes[14] + bytes[15]) + "; Vertical resolution: " + (bytes[16] + bytes[17]) + ".");
                    break;
                case 0x02:
                    Console.WriteLine("Specified unit is dots per centimeter. Horizontal resolution: " + (bytes[14] + bytes[15]) + "; Vertical resolution: " + (bytes[16] + bytes[17]) + ".");
                    break;
                default:
                    Console.WriteLine("Unexpected unit code found: " + bytes[13]);
                    break;
            }

            bytes[15] = 200;
            bytes[17] = 200;

            File.WriteAllBytes(filename, bytes);
        }

        static unsafe void GenerateLinearFisheyeDistortionMap()
        {
            float effectX = 250f;

            Bitmap bmp = new Bitmap(1024, 1024, PixelFormat.Format32bppArgb);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, 1024, 1024), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int stride = data.Stride;
            byte* ptr = (byte*)data.Scan0;
            int bpp = 4;

            for (int x = 0; x < 1024; x++)
            {
                for (int y = 0; y < 1024; y++)
                {
                    int idx = (y * stride) + (x * bpp);

                    ptr[idx + 3] = 255;

                    if (Math.Abs(x - 512) < effectX)
                    {
                        ptr[idx] = 127;
                        ptr[idx + 1] = 127;
                        ptr[idx + 2] = 127;
                    }
                    else
                    {
                        byte val = (byte)(127 + ((float)Math.Abs(x - 512) - effectX) / (512f - effectX) * (x < 512 ? -1 : 1) * 127);
                        ptr[idx] = val;
                        ptr[idx + 1] = val;
                        ptr[idx + 2] = val;
                    }
                }
            }

            bmp.UnlockBits(data);
            bmp.Save("fisheye-linear.png", ImageFormat.Bmp);
        }

        static unsafe void GenerateRectangularFisheyeDistortionMap()
        {
            float effectX = 250f;
            float effectY = 250f;

            Bitmap bmp = new Bitmap(1024, 1024, PixelFormat.Format32bppArgb);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, 1024, 1024), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int stride = data.Stride;
            byte* ptr = (byte*)data.Scan0;
            int bpp = 4;

            for (int x = 0; x < 1024; x++)
            {
                for (int y = 0; y < 1024; y++)
                {
                    int idx = (y * stride) + (x * bpp);

                    ptr[idx] = 0;
                    ptr[idx + 3] = 255;

                    if (Math.Abs(x - 512) < effectX)
                    {
                        ptr[idx + 1] = 127;
                    }
                    else
                    {
                        byte val = (byte)(127 + ((float)Math.Abs(x - 512) - effectX) / (512f - effectX) * (x < 512 ? -1 : 1) * 127);
                        ptr[idx + 1] = val;
                    }

                    if (Math.Abs(y - 512) < effectY)
                    {
                        ptr[idx + 2] = 127;
                    }
                    else
                    {
                        byte val = (byte)(127 + ((float)Math.Abs(y - 512) - effectY) / (512f - effectY) * (y < 512 ? -1 : 1) * 127);
                        ptr[idx + 2] = val;
                    }
                }
            }

            bmp.UnlockBits(data);
            bmp.Save("fisheye-rect.png", ImageFormat.Bmp);
        }

        static unsafe void GenerateRadialFisheyeDistortionMap()
        {
            float effectDist = 250f;
            float maxDist = GeometryUtility.Distance(0f, 0f, 512f, 512f);
            float maxShortDist = GeometryUtility.Distance(0f, 0f, 512f - 250f, 512f - 250f);

            Bitmap bmp = new Bitmap(1024, 1024, PixelFormat.Format32bppArgb);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, 1024, 1024), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int stride = data.Stride;
            byte* ptr = (byte*)data.Scan0;
            int bpp = 4;

            for (int x = 0; x < 1024; x++)
            {
                for (int y = 0; y < 1024; y++)
                {
                    int idx = (y * stride) + (x * bpp);

                    ptr[idx] = 0;
                    ptr[idx + 3] = 255;

                    float dist = GeometryUtility.Distance((float)x, (float)y, 512f, 512f);

                    if (dist >= effectDist)
                    {
                        float shortDist = dist - effectDist;
                        float r = shortDist / maxShortDist;
                        int sx = (int)((x - 512) * (shortDist / dist));
                        int sy = (int)((y - 512) * (shortDist / dist));

                        ptr[idx + 1] = (byte)(127 + (sx * r * (x - effectDist < 0 ? -1 : 1)));
                    }

                    /*if (Math.Abs(x - 512) < effectX)
                    {
                        ptr[idx + 1] = 127;
                    }
                    else
                    {
                        byte val = (byte)(127 + ((float)Math.Abs(x - 512) - effectX) / (512f - effectX) * (x < 512 ? -1 : 1) * 127);
                        ptr[idx + 1] = val;
                    }

                    if (Math.Abs(y - 512) < effectY)
                    {
                        ptr[idx + 2] = 127;
                    }
                    else
                    {
                        byte val = (byte)(127 + ((float)Math.Abs(y - 512) - effectY) / (512f - effectY) * (y < 512 ? -1 : 1) * 127);
                        ptr[idx + 2] = val;
                    }*/
                }
            }

            bmp.UnlockBits(data);
            bmp.Save("fisheye-rad.png", ImageFormat.Bmp);
        }

        static void GetCardImages()
        {
            string targetSetCode = "c14";

            string urlFile1 = @"C:\Users\Abion47\Desktop\LackeyCCG\plugins\magic\CardImageURLs1.txt";
            string urlFile2 = @"C:\Users\Abion47\Desktop\LackeyCCG\plugins\magic\CardImageURLs2.txt";
            string urlFile3 = @"C:\Users\Abion47\Desktop\LackeyCCG\plugins\magic\CardImageURLs3.txt";
            string urlFile4 = @"C:\Users\Abion47\Desktop\LackeyCCG\plugins\magic\CardImageURLs4.txt";
            string urlFile5 = @"C:\Users\Abion47\Desktop\LackeyCCG\plugins\magic\CardImageURLs5.txt";
            string urlFile6 = @"C:\Users\Abion47\Desktop\LackeyCCG\plugins\magic\CardImageURLs6.txt";

            string outputDir = @"C:\Users\Abion47\Desktop\LackeyCCG\plugins\magic\sets\setimages";

            if (!Directory.Exists(outputDir + "\\" + targetSetCode))
            {
                Directory.CreateDirectory(outputDir + "\\" + targetSetCode);
            }

            List<String> urlLines = new List<string>();

            urlLines.AddRange(File.ReadAllLines(urlFile1));
            urlLines.AddRange(File.ReadAllLines(urlFile2));
            urlLines.AddRange(File.ReadAllLines(urlFile3));
            urlLines.AddRange(File.ReadAllLines(urlFile4));
            urlLines.AddRange(File.ReadAllLines(urlFile5));
            urlLines.AddRange(File.ReadAllLines(urlFile6));

            using (WebClient client = new WebClient())
            {
                foreach (string line in urlLines)
                {
                    if (line.StartsWith(targetSetCode))
                    {
                        string[] split = line.Split('\t');
                        string outputFile = outputDir + "\\" + split[0];

                        Console.Write("Downloading " + split[0] + " ...");

                        if (File.Exists(outputFile))
                        {
                            Console.Write(" image exists.\n");
                            continue;
                        }

                        client.DownloadFile(split[1], outputFile);

                        Console.Write(" Done.\n");
                    }
                }
            }
        }

        class SplitListElement : IComparable<SplitListElement>
        {
            public int face;
            public int value;

            public int CompareTo(SplitListElement other)
            {
                return this.face.CompareTo(other.face);
            }

            public override string ToString()
            {
                return "{ " + face + ", " + value + " }";
            }
        }

        static List<List<SplitListElement>> SplitListLinq(List<SplitListElement> list)
        {
            return list
                .Select(x => new { Index = x.face, Value = x })
                .GroupBy(x => x.Index)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        static List<List<SplitListElement>> SplitListNative(List<SplitListElement> list)
        {
            list.Sort();

            List<List<SplitListElement>> retval = new List<List<SplitListElement>>();
            int currentFace = -1;
            int currentIndex = -1;

            foreach (var elem in list)
            {
                if (elem.face != currentFace)
                {
                    retval.Add(new List<SplitListElement>());
                    currentFace = elem.face;
                    currentIndex++;
                }

                retval[currentIndex].Add(elem);
            }

            return retval;
        }

        public const int TOP_NORTH_WEST = 0;
        public const int TOP_SOUTH_WEST = 1;
        public const int TOP_NORTH_EAST = 2;
        public const int TOP_SOUTH_EAST = 3;
        public const int BOT_NORTH_WEST = 4;
        public const int BOT_SOUTH_WEST = 5;
        public const int BOT_NORTH_EAST = 6;
        public const int BOT_SOUTH_EAST = 7;

        static byte[][, ,] ArraySubdivide(byte[, ,] array)
        {
            if (array.GetLength(0) == 1)
            {
                throw new Exception("Array is already at minimum size and cannot be subdivided further.");
            }

            byte[][, ,] retVal = new byte[8][, ,];
            int newDim = array.GetLength(0) / 2;

            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = new byte[newDim, newDim, newDim];
            }

            for (int x = 0; x < newDim * 2; x++)
            {
                for (int y = 0; y < newDim * 2; y++)
                {
                    for (int z = 0; z < newDim * 2; z++)
                    {
                        if (y < newDim)
                        {
                            if (x < newDim)
                            {
                                if (z < newDim)
                                {
                                    retVal[TOP_NORTH_WEST][x, y, z] = array[x, y, z];
                                }
                                else
                                {
                                    retVal[TOP_NORTH_EAST][x, y, z - newDim] = array[x, y, z];
                                }
                            }
                            else
                            {
                                if (z < newDim)
                                {
                                    retVal[TOP_SOUTH_WEST][x - newDim, y, z] = array[x, y, z];
                                }
                                else
                                {
                                    retVal[TOP_SOUTH_EAST][x - newDim, y, z - newDim] = array[x, y, z];
                                }
                            }
                        }
                        else
                        {
                            if (x < newDim)
                            {
                                if (z < newDim)
                                {
                                    retVal[BOT_NORTH_WEST][x, y - newDim, z] = array[x, y, z];
                                }
                                else
                                {
                                    retVal[BOT_NORTH_EAST][x, y - newDim, z - newDim] = array[x, y, z];
                                }
                            }
                            else
                            {
                                if (z < newDim)
                                {
                                    retVal[BOT_SOUTH_WEST][x - newDim, y - newDim, z] = array[x, y, z];
                                }
                                else
                                {
                                    retVal[BOT_SOUTH_EAST][x - newDim, y - newDim, z - newDim] = array[x, y, z];
                                }
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        static unsafe void BicubicGraphTest()
        {
            int width = 180;
            int height = 180;
            Bitmap bmp = new Bitmap(width, height, GlobalSettings.DefaultPixelFormat);

            var data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, GlobalSettings.DefaultPixelFormat);
            var stride = data.Stride;
            var ptr = (byte*)data.Scan0;
            int bpp = GlobalSettings.DefaultPixelFormatBpp;

            for (int x = 0; x < width; x++)
            {
                int y = (int)MathF.Cerp(23f, 0f, 2, 3, (float)x / width);

                int idx = (y * stride) + (x * bpp);

                ptr[idx] = 0;
                ptr[idx + 1] = 0;
                ptr[idx + 2] = 0;
                ptr[idx + 3] = 255;
            }

            bmp.UnlockBits(data);
            bmp.Save(GlobalSettings.TestFileOutputDirectory + "cubic_test.png");
        }
    }
}
