using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Config.Reader;
using static CitizenFX.Core.Native.API;

namespace EVLClient.Utils
{
    class Controls
    {

        public static EVLControls KeyBindings = new EVLControls();

        public static void LoadConfig()
        {
            iniconfig config = new iniconfig("evl", "config.ini");
            // tab
            KeyBindings.Toggle_LSTG = config.GetIntValue("CONTROL", "Toggle_LSTG", 37);
            // =
            KeyBindings.Toggle_CRSL = config.GetIntValue("CONTROL", "Toggle_CRSL", 83);
            // 6
            KeyBindings.Sound_AHorn = config.GetIntValue("CONTROL", "Toggle_TKDL", 159);
            //-
            KeyBindings.Toggle_BLKT = config.GetIntValue("CONTROL", "Toggle_BLKT", 84);
            // g
            KeyBindings.Toggle_SIRN = config.GetIntValue("CONTROL", "Toggle_SIRN", 58);
            // r
            KeyBindings.ChgPat = config.GetIntValue("CONTROL", "ChgPat", 80);
            // 7
            KeyBindings.ChgPatType = config.GetIntValue("CONTROL", "ChgPatType", 161);
            //e
            KeyBindings.Sound_AHorn = config.GetIntValue("CONTROL", "Sound_Ahorn", 86);
            // 1
            KeyBindings.Snd_SrnTone1 = config.GetIntValue("CONTROL", "Snd_SrnTon1", 157);
            // 2
            KeyBindings.Snd_SrnTone2 = config.GetIntValue("CONTROL", "Snd_SrnTon2", 158);
            // 3
            KeyBindings.Snd_SrnTone3 = config.GetIntValue("CONTROL", "Snd_SrnTon3", 160);
            // [
            KeyBindings.TogLeftAlley = config.GetIntValue("CONTROL", "TogLeftAlley", 39);
            // ]
            KeyBindings.TogRightAlley = config.GetIntValue("CONTROL", "TogRightAlley", 40);
            // p
            KeyBindings.TogInfoPanel = config.GetIntValue("CONTROL", "TogInfoPanl", 199);
            // home, RB
            KeyBindings.TogKeysLock = config.GetIntValue("CONTROL", "TogKeysLock", 213);

            Debug.WriteLine("[EVL] Loaded config file.");
        }

        public class EVLControls
        {
            internal int Toggle_LSTG { get; set; }
            internal int Toggle_CRSL { get; set; }
            internal int Toggle_TKDL { get; set; }
            internal int Toggle_BLKT { get; set; }
            internal int Toggle_SIRN { get; set; }
            internal int ChgPat { get; set; }
            internal int ChgPatType { get; set; }
            internal int Sound_AHorn { get; set; }
            internal int Snd_SrnTone1 { get; set; }
            internal int Snd_SrnTone2 { get; set; }
            internal int Snd_SrnTone3 { get; set; }
            internal int TogLeftAlley { get; set; }
            internal int TogRightAlley { get; set; }
            internal int TogInfoPanel { get; set; }
            internal int TogKeysLock { get; set; }
        }
    }
}
