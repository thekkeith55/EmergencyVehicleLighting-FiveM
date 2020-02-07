using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVLClient.Utils;

namespace EVLClient.EVLVeh
{
    public static class VehicleData
    {

        public static bool IsEVL(this Vehicle veh) {
            return VCF.EVLVehicle.ContainsKey(veh.Model);
        }
    }

    public class EVLVehicleData
    {
        internal int stage { get; set; }
        internal int pattern { get; set; }

        public EVLVehicleData(int stage, int pattern) {
            this.stage = stage;
            this.pattern = pattern;
        }
    }
}
