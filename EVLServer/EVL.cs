using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVLServer.Utils;
using static CitizenFX.Core.Native.API;
using System.IO;
using System.Reflection;

namespace EVLServer
{
    public class EVL : BaseScript
    {

        public static List<Tuple<string, string>> vcfDataFiles = new List<Tuple<string, string>>();
        public static List<Tuple<string, int, bool>> controls = new List<Tuple<string, int, bool>>();
        
        public EVL()
        {
            Debug.WriteLine("------------- Loaded ELC FiveM ---------------");
            Debug.WriteLine("                Made by MrDaGree");
            Debug.WriteLine("        Further modified by TheKeith");
            Debug.WriteLine("--------------------------------------------------");

            string folder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\resources\evl\vcf\";
            string filter = "*.xml";
            string[] files = Directory.GetFiles(folder, filter);

            foreach (string file in files) {
                Debug.WriteLine("[ELC] Found file in VCF folder (" + Path.GetFileNameWithoutExtension(file) + ") to load.");
                vcfDataFiles.Add(new Tuple<string, string>(Path.GetFileNameWithoutExtension(file), LoadResourceFile("evl", "vcf/" + Path.GetFileName(file))));
            }
            
            EventHandlers.Add("EVL:Init:Server", new Action<int>(ClientVCFSync));
            EventHandlers.Add("elcs:stateChange", new Action<int, int, int, int>(StateChange));
        }

        private void StateChange(int source, int id, int pat, int stage) {
            Debug.WriteLine($"[ELC] {id} {pat} {stage}");

            TriggerClientEvent("elcs:changeVehicleData", id, pat, stage);
        }

        private void ClientVCFSync(int source)
        {
            Debug.WriteLine($"[ELC] Sending Data to {Players[source].Name}");

            TriggerClientEvent(Players[source], "EVL:VcfSync:Client", vcfDataFiles);
        }
    }
}
