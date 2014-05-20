using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTFTableViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            string path = @"F:\Users\Abion47\Projects\XNA\Samples\RedistributableTTFs_ARCHIVE_3_1\RedistributableTTFs\SegoeUIMono-Regular.ttf";
            ParseTTF(path);

            Console.WriteLine("test");
            Console.ReadKey();
        }

        static void ParseTTF(string path)
        {
            FileStream stream = File.OpenRead(path);
            byte[] fileContents = new byte[stream.Length];
            stream.Read(fileContents, 0, fileContents.Length);
            stream.Close();

            byte[] versionBin = new byte[4];
            byte[] numTablesBin = new byte[2];
            byte[] searchRangeBin = new byte[2];
            byte[] entrySelectorBin = new byte[2];
            byte[] rangeShiftBin = new byte[2];

            Array.Copy(fileContents, 0, versionBin, 0, versionBin.Length);
            Array.Copy(fileContents, 4, numTablesBin, 0, numTablesBin.Length);
            Array.Copy(fileContents, 6, searchRangeBin, 0, searchRangeBin.Length);
            Array.Copy(fileContents, 8, entrySelectorBin, 0, entrySelectorBin.Length);
            Array.Copy(fileContents, 10, rangeShiftBin, 0, rangeShiftBin.Length);

            float version = Conversions.ByteArrayToFixed(versionBin);
            ushort numTables = BitConverter.ToUInt16(numTablesBin, 0);
            ushort searchRange = BitConverter.ToUInt16(searchRangeBin, 0);
            ushort entrySelector = BitConverter.ToUInt16(entrySelectorBin, 0);
            ushort rangeShift = BitConverter.ToUInt16(rangeShiftBin, 0);

            Console.WriteLine("Byte Array To Short: 01111111 11111111 - " + Conversions.ByteArrayToShort(new byte[] { 0x7F, 0xFF }));
            Console.WriteLine("Byte Array To Short: 11111111 11111111 - " + Conversions.ByteArrayToShort(new byte[] { 0xFF, 0xFF }));
            Console.WriteLine("Byte Array To UShort: 01111111 11111111 - " + Conversions.ByteArrayToUnsignedShort(new byte[] { 0x7F, 0xFF }));
            Console.WriteLine("Byte Array To UShort: 11111111 11111111 - " + Conversions.ByteArrayToUnsignedShort(new byte[] { 0xFF, 0xFF }));

            int z = 0;
        }
    }
}
