using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine.Utils
{
    public static class SystemEnvironment
    {
        public static int ProcessorCountOptimizedForEnvironment()
        {
            int processorCount = Environment.ProcessorCount;

            int factor = 2;

            return processorCount * factor;
        }
    }
}
