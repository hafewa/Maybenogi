using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Maybenogi.Server.Mabinogi
{
    public static class MabiManager
    {
        public static void SetProcessOrder(int pid, bool isMain)
        {
            var process = Process.GetProcessById(pid);

            var threadCount = Environment.ProcessorCount;
            
            ulong maximum = 0;
            ulong minimum = 0x0000000000000001;

            for (int i = 1; i < threadCount; i++)
            {
                minimum <<= 1;
            }

            if (!Environment.Is64BitOperatingSystem)
            {
                minimum &= 0xffffffff;
            }

            if (isMain)
            {
                ulong flag = 0x0000000000000001;

                for (int i = 0; i < threadCount; i++)
                {
                    maximum |= flag;
                    flag <<= 1;
                }

                if (!Environment.Is64BitOperatingSystem)
                {
                    maximum &= 0xffffffff;
                }

                maximum &= (~minimum);
                process.ProcessorAffinity = (IntPtr)maximum;
            }
            else
            {
                process.ProcessorAffinity = (IntPtr)minimum;
            }
        }
    }
}