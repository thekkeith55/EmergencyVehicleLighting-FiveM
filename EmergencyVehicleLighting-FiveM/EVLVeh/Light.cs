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
using System.Threading;
namespace EVLClient.EVLVeh
{
    class Light
    {
        Model vehModel;
        public int id;
        public string bone;
        public bool state;
        public bool isPatternRunning;
        public string pattern;

        public Light(Model veh, int id, string bone, int pat, string type) {
            this.id = id;
            this.bone = bone;
            this.vehModel = veh;
            this.pattern = type;
        }
        
    }
}
