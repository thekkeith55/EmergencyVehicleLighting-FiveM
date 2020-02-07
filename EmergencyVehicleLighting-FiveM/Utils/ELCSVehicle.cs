using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVLClient.Utils
{
    class ELCSVehicle
    {
        public string model { get; set; }

        public int interface_r { get; set; }
        public int interface_g { get; set; }
        public int interface_b { get; set; }

        public string activation { get; set; }

        public string lightType { get; set; }

        public int leftColor { get; set; }
        public int rightColor { get; set; }
        public int auxColor { get; set; }

        public ELCSVehicle() {
        }
    }
}
