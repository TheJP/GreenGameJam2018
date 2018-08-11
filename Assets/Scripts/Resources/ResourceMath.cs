using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Resources
{
    public static class ResourceMath
    {
        public static Blood Min(Blood a, Blood b) => a < b ? a : b;
        public static Blood Max(Blood a, Blood b) => a > b ? a : b;
        public static ConstructionMaterial Min(ConstructionMaterial a, ConstructionMaterial b) => a < b ? a : b;
        public static ConstructionMaterial Max(ConstructionMaterial a, ConstructionMaterial b) => a > b ? a : b;
        public static Energy Min(Energy a, Energy b) => a < b ? a : b;
        public static Energy Max(Energy a, Energy b) => a > b ? a : b;
        public static Oxygen Min(Oxygen a, Oxygen b) => a < b ? a : b;
        public static Oxygen Max(Oxygen a, Oxygen b) => a > b ? a : b;
    }
}
