using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVLClient.Utils;
using EVLClient.EVLVeh;
using EVLClient.Patterns;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
namespace EVLClient.EVLVeh
{

    
    class Lights
    {
        public static Dictionary<Model, Dictionary<int, Light>> lightKits = new Dictionary<Model, Dictionary<int, Light>>();

    }
}
