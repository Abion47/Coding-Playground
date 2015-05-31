using org.general;
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
    class Program
    {
        static void Main(string[] args)
        {
            OneShotPrograms.GetJfifResolutionData();
            
            Console.WriteLine("\n\nDone!");
            Console.ReadKey();
        }
    }
}
