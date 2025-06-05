using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet
{
    public static class PoziomWyksztalceniaHelper
    {
        public static Array AllValues => Enum.GetValues(typeof(PoziomWyksztalcenia));
    }
}
