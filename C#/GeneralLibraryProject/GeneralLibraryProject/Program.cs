using org.general.Units;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace org.general
{
    class Program
    {
        static void Main(string[] args)
        {
            //Bitmap bmp;
            //Random r = new Random();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //Imaging.ImageFilters.InvertFilter(bmp).Save("invert.png");
            //Console.WriteLine(Matrix2D.RunUnitTest());
            /*bmp = Imaging.Transforms.ResizeImage(
                Imaging.Conversion.ConvertPixelFormat(
                    new Bitmap(GlobalSettings.TestFileInputDirectory + "test_image_small.png"),
                    GlobalSettings.DefaultPixelFormat),
                2f, 2f, Imaging.Transforms.ResizeMethod.BICUBIC);*/
            //bmp = Imaging.Gradients.DrawFourCornerGradient(500, 200, Color.Cyan, Color.Magenta, Color.Yellow, Color.Black);
            /*bmp = Drawing.Quadrilateral.Draw(Color.Red, 
                50, 50, 
                50, 250, 
                250, 250, 
                250, 75);*/
            //BicubicGraphTest();

            int start = 600000;
            Console.WriteLine("ASCII Code [" + start + "] as a Char is: [" + SimpleFunctions.AsciiToChar(start) + "]");

            watch.Stop();

            //bmp.Save(GlobalSettings.TestFileOutputDirectory + "bic_resize_2.png");
            Console.WriteLine("Time: " + watch.ElapsedMilliseconds + "ms");

            Console.WriteLine("\n\nDone!");
            Console.ReadKey();
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
                int y = (int)Functions.Interpolation.Cerp(new float[] { 23f, 0f, 2, 3 }, (float)x / width);

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
