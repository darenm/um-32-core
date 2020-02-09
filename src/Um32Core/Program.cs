using System;
using System.Collections.Generic;
using System.IO;

namespace Um32Core
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] bytes = File.ReadAllBytes("map.um");
            byte[] bytes = File.ReadAllBytes("sandmark.umz");
            var platters = new List<Platter>();
            var index = 0;
            while (index < bytes.Length)
            {
                platters.Add(new Platter(bytes[index], bytes[index + 1], bytes[index + 2], bytes[index + 3]));
                index += 4;
            }
            bytes = new byte[0];

            var um32 = new Um32(platters.ToArray());
            um32.SpinCycle();
        }
    }
}
