using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialZombie
{
    internal static class Utils
    {
        public static int GetColor(int r, int g, int b)
        {
            return r << 16 | g << 8 | b;
        }
    }
}
