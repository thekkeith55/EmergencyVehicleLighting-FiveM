using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVLClient.Pattern;
using EVLClient.Utils;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace EVLClient.Utils
{

    public struct Extras
    {
        public Dictionary<int, Light> PRML;
        internal Dictionary<int, Light> WRNL;
        internal Dictionary<int, Light> SECL;
    }

    public class ELCVeh
    {
        private int veh;
        private Vehicle vehicle;
        public int pat;
        public int stage;

        public Extras _lights = new Extras
        {
            PRML = new Dictionary<int, Light>(),
            WRNL = new Dictionary<int, Light>(),
            SECL = new Dictionary<int, Light>(),
        };

        public ELCVeh(Vehicle veh, int pat, int stage)
        {
            this.vehicle = veh;
            this.veh = veh.Handle;
            this.pat = pat;
            this.stage = stage;

            AddAllLightModkits();
        }

        private void AddAllLightModkits() {
            for (int i = 0; i <= 6; i++)
            {
                if (i >= 0 && i <= 3)
                {
                    string name = new Vehicle(veh).DisplayName.ToString().ToLower();

                    int red = 255;
                    int green = 60;
                    int blue = 60;

                    if (VCF.ELCSVehicles[name].leftColor == 28)
                    {
                        red = 255;
                        green = 60;
                        blue = 60;
                    }
                    else if (VCF.ELCSVehicles[name].leftColor == 73)
                    {
                        red = 60;
                        green = 60;
                        blue = 255;
                    }

                    _lights.PRML.Add(i, new Light(vehicle, i, red, green, blue));
                }

                if (i >= 5 && i <= 6)
                {
                    string name = new Vehicle(veh).DisplayName.ToString().ToLower();

                    int red = 255;
                    int green = 60;
                    int blue = 60;

                    if (VCF.ELCSVehicles[name].rightColor == 28)
                    {
                        red = 255;
                        green = 60;
                        blue = 60;
                    }
                    else if (VCF.ELCSVehicles[name].rightColor == 73)
                    {
                        red = 60;
                        green = 60;
                        blue = 255;
                    }

                    _lights.PRML.Add(i, new Light(vehicle, i, red, green, blue));
                }
            }

            for (int i = 27; i <28; i++)
            {
                if (i == 27)
                {
                    string name = new Vehicle(veh).DisplayName.ToString().ToLower();

                    int red = 255;
                    int green = 60;
                    int blue = 60;

                    if (VCF.ELCSVehicles[name].rightColor == 28)
                    {
                        red = 255;
                        green = 60;
                        blue = 60;
                    }
                    else if (VCF.ELCSVehicles[name].rightColor == 73)
                    {
                        red = 60;
                        green = 60;
                        blue = 255;
                    }

                    _lights.PRML.Add(i, new Light(vehicle, i, red, green, blue));
                }
            }

            for (int i = 25; i < 26; i++)
            {
                if (i == 25)
                {
                    string name = new Vehicle(veh).DisplayName.ToString().ToLower();

                    int red = 255;
                    int green = 60;
                    int blue = 60;

                    if (VCF.ELCSVehicles[name].leftColor == 28)
                    {
                        red = 255;
                        green = 60;
                        blue = 60;
                    }
                    else if (VCF.ELCSVehicles[name].leftColor == 73)
                    {
                        red = 60;
                        green = 60;
                        blue = 255;
                    }

                    _lights.PRML.Add(i, new Light(vehicle, i, red, green, blue));
                }
            }

            for (int i = 7; i <= 10; i++)
            {
                _lights.SECL.Add(i, new Light(vehicle, i, 255,60,60));
            }
            _lights.SECL.Add(26, new Light(vehicle, 26, 255, 60, 60));
        }
    }
}
