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

namespace EVLClient.Patterns
{
    class Pattern
    {
        public static async Task Run(Vehicle veh, EVLVehicleData data)
        {
            if (data.stage > 0)
            {
                if (!IsPedInVehicle(Game.PlayerPed.Handle, veh.Handle, true))
                    SetVehicleEngineOn(veh.Handle, true, true, false);
                await ledPattern(veh, 1, 0);
                await ledPattern(veh, 1, 1);
                await ledPattern(veh, 1, 2);
                await ledPattern(veh, 1, 3);
                await ledPattern(veh, 1, 4);
                await ledPattern(veh, 1, 5);
                await ledPattern(veh, 1, 6);
                await ledPattern(veh, 1, 7);
                await ledPattern(veh, 1, 8);
                await ledPattern(veh, 1, 9);
                await ledPattern(veh, 1, 10);
            }
            else
            {
                if (!IsPedInVehicle(Game.PlayerPed.Handle, veh.Handle, true))
                    SetVehicleEngineOn(veh.Handle, true, true, false);
                SetVehicleModKit(veh.Handle, 0);
                SetVehicleMod(veh.Handle, 0, 0, false);
                SetVehicleMod(veh.Handle, 1, 0, false);
                SetVehicleMod(veh.Handle, 2, 0, false);
                SetVehicleMod(veh.Handle, 3, 0, false);
                SetVehicleMod(veh.Handle, 4, 0, false);
                SetVehicleMod(veh.Handle, 5, 0, false);
                SetVehicleMod(veh.Handle, 6, 0, false);
                SetVehicleMod(veh.Handle, 7, 0, false);
                SetVehicleMod(veh.Handle, 8, 0, false);
                SetVehicleMod(veh.Handle, 9, 0, false);
                SetVehicleMod(veh.Handle, 10, 0, false);
                SetVehicleMod(veh.Handle, 25, 0, false);
                SetVehicleMod(veh.Handle, 26, 0, false);
                SetVehicleMod(veh.Handle, 27, 0, false);
                SetVehicleMod(veh.Handle, 33, 0, false);
            }

            await Task.FromResult(0);
        }

        static int count = 0;
        private static async Task ledPattern(Vehicle veh, int pattern, int light)
        {
            SetVehicleModKit(veh.Handle, 0);
            if (Leds.LightStageOne[pattern][light].ToCharArray()[count].Equals('1'))
            {
                SetVehicleMod(veh.Handle, light, 1, false);
            }
            else
            {
                SetVehicleMod(veh.Handle, light, 0, false);
            }

            count++;
            if (count == Leds.LightStageOne[pattern][light].Length - 1)
            {
                count = 0;
            }

            await Task.FromResult(0);
        }
    }
}
