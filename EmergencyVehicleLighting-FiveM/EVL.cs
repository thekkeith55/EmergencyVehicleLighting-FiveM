using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVLClient.Pattern;
using EVLClient.Utils;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace EVLClient
{
    internal struct ControlData
    {
        public string functionName;
        public int keyID;
        public bool modifierNeeded;

        public ControlData(string fn, int key, bool modifier)
        {
            functionName = fn;
            keyID = key;
            modifierNeeded = modifier;
        }
    }

    public class EVL : BaseScript
    {

        public int curVehicleStage = 0;
        public int curVehiclePat = 1;

        public static bool ELCSReady = false;
        bool panelOpen = true;
        bool crsl = false;
        bool sb = false;

        public static Dictionary<int, ELCVeh> elcs_vehicles = new Dictionary<int, ELCVeh>();

        public EVL()
        {
            TriggerServerEvent("EVL:Init:Server", Game.Player.ServerId);

            Controls.LoadConfig();

            EventHandlers["EVL:VcfSync:Client"] += new Action<List<dynamic>>((vcfs) =>
            {
                VCF.ParseVcfs(vcfs);
            });

            EventHandlers["elcs:enteredVehicle"] += new Action<int>((veh) =>
            {
                Vehicle vehicle = new Vehicle(veh);
                string name = vehicle.DisplayName.ToString().ToLower();
                if (vehicle.IsELCS())
                {
                    
                    if (!elcs_vehicles.ContainsKey(NetworkGetNetworkIdFromEntity(vehicle.Handle)))
                    {
                        NetworkRegisterEntityAsNetworked(vehicle.Handle);
                        int id = NetworkGetNetworkIdFromEntity(vehicle.Handle);
                        SetNetworkIdExistsOnAllMachines(id, true);
                        SetNetworkIdCanMigrate(id, true);

                        elcs_vehicles.Add(id, new ELCVeh(vehicle, 1, 0));

                        SetVehicleModKit(vehicle.Handle, 0);
                        
                        SetVehicleColours(vehicle.Handle, 111, VCF.ELCSVehicles[name].leftColor);
                        SetVehicleDashboardColour(vehicle.Handle, VCF.ELCSVehicles[name].rightColor);
                    }
                    else {
                        curVehicleStage = elcs_vehicles[VehToNet(vehicle.Handle)].stage;
                        curVehiclePat = elcs_vehicles[VehToNet(vehicle.Handle)].pat;
                    }
                }
            });

            EventHandlers["elcs:changeVehicleData"] += new Action<int, int, int>((id, pat, stage) =>
            {
                if (elcs_vehicles.ContainsKey(id))
                {
                    elcs_vehicles[id].pat = pat;
                    elcs_vehicles[id].stage = stage;
                    Vehicle veh = new Vehicle(NetToVeh(id));

                    if (stage == 3)
                    {
                        foreach (Light light in elcs_vehicles[id]._lights.PRML.Values)
                        {
                            light.IsPatternRunning = true;
                        }
                    }
                    else
                    {
                        foreach (Light light in elcs_vehicles[id]._lights.PRML.Values)
                        {
                            light.IsPatternRunning = false;
                        }
                    }

                    if (stage == 2 || stage == 3)
                    {
                        foreach (Light light in elcs_vehicles[id]._lights.SECL.Values)
                        {
                            light.IsPatternRunning = true;
                        }
                    }
                    else
                    {
                        foreach (Light light in elcs_vehicles[id]._lights.SECL.Values)
                        {
                            light.IsPatternRunning = false;
                        }
                    }

                    foreach (Light light in elcs_vehicles[id]._lights.PRML.Values) {
                        try
                        {
                            switch (VCF.ELCSVehicles[veh.DisplayName.ToString().ToLower()].lightType)
                            {
                                
                                case "leds":
                                    light.Pattern = _led.primary[pat][light.modkit];
                                    break;
                            }
                        }
                        catch
                        {

                        }
                    }

                    foreach (Light light in elcs_vehicles[id]._lights.SECL.Values)
                    {
                        try
                        {
                            switch (VCF.ELCSVehicles[veh.DisplayName.ToString().ToLower()].lightType)
                            {

                                case "leds":
                                    light.Pattern = _led.secondary[pat][light.modkit];
                                    break;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    Vehicle vehicle = new Vehicle(NetworkGetEntityFromNetworkId(id));
                    NetworkRegisterEntityAsNetworked(vehicle.Handle);
                    int nid = NetworkGetNetworkIdFromEntity(vehicle.Handle);
                    SetNetworkIdExistsOnAllMachines(nid, true);
                    SetNetworkIdCanMigrate(nid, true);

                    elcs_vehicles.Add(nid, new ELCVeh(vehicle, 1, 0));
                }
            });

            Tick += Main;
            Tick += GUI;
            Tick += ControlLogic;
        }

        private async Task GUI()
        {
            if (ELCSReady && panelOpen && Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.IsELCS())
            {
                Vehicle veh = Game.PlayerPed.CurrentVehicle;
                Model hash = GetHashKey(veh.DisplayName);
                string name = veh.DisplayName.ToString().ToLower();

                int infoButtonLightRed = VCF.ELCSVehicles[name].interface_r;
                int infoButtonLightGreen = VCF.ELCSVehicles[name].interface_g;
                int infoButtonLightBlue = VCF.ELCSVehicles[name].interface_b;

                int leftR = 255;
                int leftG = 60;
                int leftB = 60;

                if (VCF.ELCSVehicles[name].leftColor == 28)
                {
                    leftR = 255;
                    leftG = 60;
                    leftB = 60;
                }
                else if (VCF.ELCSVehicles[name].leftColor == 73)
                {
                    leftR = 60;
                    leftG = 60;
                    leftB = 255;
                }

                int rightR = 60;
                int rightG = 60;
                int rightB = 255;

                if (VCF.ELCSVehicles[name].rightColor == 28)
                {
                    rightR = 255;
                    rightG = 60;
                    rightB = 60;
                }
                else if (VCF.ELCSVehicles[name].rightColor == 73)
                {
                    rightR = 0;
                    rightG = 150;
                    rightB = 255;
                }


                await Panel._DrawRect(0.8445f, 0.491f, 0.238f, 0.198f, 0, 0, 0, 255);
                await Panel._DrawRect(0.8445f, 0.491f, 0.232f, 0.188f, 20, 20, 20, 255);
                
                await Panel._DrawText("EMERGENCY LIGHTING CONTROL SYSTEM", 140, 140, 140, 255, 0.83f, 0.57f, 0.155f, 0.155f, false, 0);
                await Panel._DrawText("DEVELOPER VERSION", 12, 185, 242, 255, 0.745f, 0.57f, 0.155f, 0.155f, false, 0);

                await Panel._DrawRect(0.878f, 0.434f, 0.12f, 0.06f, 140, 140, 140, 255);
                await Panel._DrawRect(0.878f, 0.434f, 0.1175f, 0.057f, 0, 0, 0, 255);
                await Panel._DrawText("SmartController", 140, 140, 140, 255, 0.8365f, 0.41f, 0.6f, 0.6f, false, 4);

                if (GetVehicleMod(veh.Handle, 0) == 0)
                    await Panel._DrawRect(0.842f, 0.452f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                else
                    await Panel._DrawRect(0.842f, 0.452f, 0.0079f, 0.0049f, leftR, leftG, leftB, 255);

                if (GetVehicleMod(veh.Handle, 1) == 0)
                    await Panel._DrawRect(0.852f, 0.452f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                else
                    await Panel._DrawRect(0.852f, 0.452f, 0.0079f, 0.0049f, leftR, leftG, leftB, 255);


                if (GetVehicleMod(veh.Handle, 2) == 0)
                    await Panel._DrawRect(0.862f, 0.452f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                else
                    await Panel._DrawRect(0.862f, 0.452f, 0.0079f, 0.0049f, leftR, leftG, leftB, 255);


                if (GetVehicleMod(veh.Handle, 25) == 0)
                    await Panel._DrawRect(0.872f, 0.452f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                else
                    await Panel._DrawRect(0.872f, 0.452f, 0.0079f, 0.0049f, leftR, leftG, leftB, 255);


                if (GetVehicleMod(veh.Handle, 27) == 0)
                    await Panel._DrawRect(0.882f, 0.452f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                else
                    await Panel._DrawRect(0.882f, 0.452f, 0.0079f, 0.0049f, rightR, rightG, rightB, 255);

                if (GetVehicleMod(veh.Handle, 3) == 0)
                    await Panel._DrawRect(0.892f, 0.452f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                else
                    await Panel._DrawRect(0.892f, 0.452f, 0.0079f, 0.0049f, rightR, rightG, rightB, 255);

                if (GetVehicleMod(veh.Handle, 5) == 0)
                    await Panel._DrawRect(0.902f, 0.452f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                else
                    await Panel._DrawRect(0.902f, 0.452f, 0.0079f, 0.0049f, rightR, rightG, rightB, 255);

                if (GetVehicleMod(veh.Handle, 6) == 0)
                    await Panel._DrawRect(0.912f, 0.452f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                else
                    await Panel._DrawRect(0.912f, 0.452f, 0.0079f, 0.0049f, rightR, rightG, rightB, 255);
                

                await Panel._DrawRect(0.7753f, 0.434f, 0.069f, 0.06f, 140, 140, 140, 255);
                await Panel._DrawRect(0.775f, 0.434f, 0.067f, 0.057f, 0, 0, 0, 255);
                await Panel._DrawText("PATTERN", 140, 140, 140, 255, 0.786f, 0.404f, 0.23f, 0.23f, true, 0);

                if (curVehicleStage == 1)
                    await Panel._DrawRect(0.749f, 0.416f, 0.0059f, 0.01f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue, 255);
                else
                    await Panel._DrawRect(0.749f, 0.416f, 0.0059f, 0.01f, 50, 50, 50, 255);
                await Panel._DrawText("1", 140, 140, 140, 255, 0.757f, 0.406f, 0.23f, 0.23f, true, 0);
                
                if (curVehicleStage == 2)
                    await Panel._DrawRect(0.749f, 0.434f, 0.0059f, 0.01f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue, 255);
                else
                    await Panel._DrawRect(0.749f, 0.434f, 0.0059f, 0.01f, 50, 50, 50, 255);
                await Panel._DrawText("2", 140, 140, 140, 255, 0.757f, 0.424f, 0.23f, 0.23f, true, 0);

                if (curVehicleStage == 3)
                    await Panel._DrawRect(0.749f, 0.452f, 0.0059f, 0.01f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue, 255);
                else
                    await Panel._DrawRect(0.749f, 0.452f, 0.0059f, 0.01f, 50, 50, 50, 255);
                await Panel._DrawText("3", 140, 140, 140, 255, 0.757f, 0.442f, 0.23f, 0.23f, true, 0);

                if (!crsl)
                    await Panel.DrawPanelButton("CSL", false, 0.750f, 0.494f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                else
                {
                    await Panel.DrawPanelButton("CSL", true, 0.750f, 0.494f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                }

                if (!sb)
                    await Panel.DrawPanelButton("SB", false, 0.828f, 0.535f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                else
                {
                    await Panel.DrawPanelButton("SB", true, 0.828f, 0.535f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                }


                await Panel._DrawSprite("EVL", "on_0", 0.776f, 0.441f, 0.015f, 0.024f, 0, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue, 255);
                await Panel._DrawSprite("EVL", "on_0", 0.788f, 0.441f, 0.015f, 0.024f, 0, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue, 255);
                await Panel._DrawSprite("EVL", "on_" + curVehiclePat, 0.8f, 0.441f, 0.015f, 0.024f, 0, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue, 255);

                await Panel.DrawPanelButton("WL", false, 0.775f, 0.494f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                await Panel.DrawPanelButton("YLP", false, 0.8f, 0.494f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                await Panel.DrawPanelButton("AX", false, 0.828f, 0.494f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                await Panel.DrawPanelButton("AH", false, 0.853f, 0.494f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);

                await Panel.DrawPanelButton("LALY", false, 0.750f, 0.535f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                await Panel.DrawPanelButton("TD", false, 0.775f, 0.535f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);
                await Panel.DrawPanelButton("RALY", false, 0.8f, 0.535f, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue);

                await Task.FromResult(0);
            }
        }

        bool isEnteringVehicle = false;

        private async Task Main()
        {

            if (!IsPedInAnyVehicle(Game.PlayerPed.Handle, false) && !IsPlayerDead(Game.Player.Handle))
            {
                if (DoesEntityExist(GetVehiclePedIsTryingToEnter(Game.PlayerPed.Handle)) && !isEnteringVehicle) {
                    Vehicle veh = new Vehicle(GetVehiclePedIsTryingToEnter(Game.PlayerPed.Handle));
                    isEnteringVehicle = true;
                    
                    TriggerEvent("elcs:enteredVehicle", veh.Handle);

                    isEnteringVehicle = false;
                }
            }

            foreach (KeyValuePair<int, ELCVeh> veh in elcs_vehicles) {
                Vehicle vehicle = new Vehicle(NetToVeh(veh.Key));
                
                if (vehicle.IsOnScreen && DoesEntityExist(vehicle.Handle) && !IsEntityDead(vehicle.Handle))
                {
                    if (veh.Value.stage > 0)
                    {
                        SetVehicleEngineOn(NetToVeh(veh.Key), true, true, false);
                        if (veh.Value.stage == 3)
                        {
                            foreach (Light prim in veh.Value._lights.PRML.Values)
                            {
                                prim.LightTicker();
                            }
                        }

                        if (veh.Value.stage == 3 || veh.Value.stage == 2)
                        {
                            foreach (Light secl in veh.Value._lights.SECL.Values)
                            {
                                secl.LightTicker();
                            }
                        }

                    }
                }
            }

            await Task.FromResult(0);
        }

        public async Task ControlLogic()
        {
            if (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.IsELCS())
            {
                Vehicle curVehicle = Game.PlayerPed.CurrentVehicle;
                DisableControlAction(0, Controls.KeyBindings.ChgPat, true);
                DisableControlAction(0, Controls.KeyBindings.ChgPatType, true);
                DisableControlAction(0, Controls.KeyBindings.Snd_SrnTone1, true);
                DisableControlAction(0, Controls.KeyBindings.Snd_SrnTone2, true);
                DisableControlAction(0, Controls.KeyBindings.Snd_SrnTone3, true);
                DisableControlAction(0, Controls.KeyBindings.Sound_AHorn, true);
                //BLKT is temp. SB key.
                DisableControlAction(0, Controls.KeyBindings.Toggle_BLKT, true);
                DisableControlAction(0, Controls.KeyBindings.Toggle_CRSL, true);
                DisableControlAction(0, Controls.KeyBindings.Toggle_LSTG, true);
                DisableControlAction(0, Controls.KeyBindings.Toggle_SIRN, true);
                DisableControlAction(0, Controls.KeyBindings.Toggle_TKDL, true);
                DisableControlAction(0, Controls.KeyBindings.TogInfoPanel, true);
                DisableControlAction(0, Controls.KeyBindings.TogKeysLock, true);
                DisableControlAction(0, Controls.KeyBindings.TogLeftAlley, true);
                DisableControlAction(0, Controls.KeyBindings.TogRightAlley, true);
                DisableControlAction(0, 19, true);


                if (IsDisabledControlPressed(0, 19) && IsDisabledControlJustPressed(0, Controls.KeyBindings.TogInfoPanel)) {
                    panelOpen = !panelOpen;
                }

                if (Controls.KeyBindings.Toggle_LSTG == 37) {
                    HideHudComponentThisFrame(19);
                }

                if (IsDisabledControlPressed(0, 83) && IsDisabledControlJustPressed(0, Controls.KeyBindings.Toggle_CRSL)) 
                {
                    if (!crsl)
                    {
                        crsl = !crsl;
                        SetVehicleMod(curVehicle.Handle, 0, 1, false);
                        SetVehicleMod(curVehicle.Handle, 6, 1, false);
                        PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                    else if (crsl)
                    {
                        crsl = !crsl;
                        SetVehicleMod(curVehicle.Handle, 0, 0, false);
                        SetVehicleMod(curVehicle.Handle, 6, 0, false);
                        PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
                if (IsDisabledControlPressed(0, 84) && IsDisabledControlJustPressed(0, Controls.KeyBindings.Toggle_BLKT))
                {
                    if (!sb)
                    {
                        sb = !sb;
                        SetVehicleMod(curVehicle.Handle, 33, 1, false);
                        PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                    else if (sb)
                    {
                        sb = !sb;
                        SetVehicleMod(curVehicle.Handle, 33, 0, false);
                        PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
                if (!IsDisabledControlPressed(0, 19) && IsDisabledControlJustPressed(0, Controls.KeyBindings.Toggle_LSTG))
                {
                    if (curVehicleStage < 3) curVehicleStage++; else curVehicleStage = 0;
                    TriggerServerEvent("elcs:stateChange", Game.Player.ServerId, VehToNet(Game.PlayerPed.CurrentVehicle.Handle),
                        curVehiclePat, curVehicleStage);
                    PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    
                }
                else if (IsDisabledControlPressed(0, 19) && IsDisabledControlJustPressed(0, Controls.KeyBindings.Toggle_LSTG)) {
                    if (curVehicleStage > 0) curVehicleStage--; else curVehicleStage = 3;
                    TriggerServerEvent("elcs:stateChange", Game.Player.ServerId, VehToNet(Game.PlayerPed.CurrentVehicle.Handle),
                        curVehiclePat, curVehicleStage);
                    PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                }

                if (!IsDisabledControlPressed(0, 19) && IsDisabledControlJustPressed(0, Controls.KeyBindings.ChgPat))
                {
                    if (curVehiclePat < _led.primary.Count) curVehiclePat++; else curVehiclePat = 1;
                    TriggerServerEvent("elcs:stateChange", Game.Player.ServerId, VehToNet(Game.PlayerPed.CurrentVehicle.Handle),
                        curVehiclePat, curVehicleStage);
                    PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                }
                else if (IsDisabledControlPressed(0, 19) && IsDisabledControlJustPressed(0, Controls.KeyBindings.ChgPat))
                {
                    if (curVehiclePat > 0) curVehiclePat--; else curVehiclePat = _led.primary.Count;
                    TriggerServerEvent("elcs:stateChange", Game.Player.ServerId, VehToNet(Game.PlayerPed.CurrentVehicle.Handle),
                        curVehiclePat, curVehicleStage);
                    PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                }

            }

            await Task.FromResult(0);
        }
    }
}
