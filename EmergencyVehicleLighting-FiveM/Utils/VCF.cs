using CitizenFX.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVLClient.Utils
{
    public static class VehicleData
    {

        public static bool IsELCS(this Vehicle veh)
        {
            return VCF.ELCSVehicles.ContainsKey(veh.DisplayName.ToLower());
        }
    }

    class VCF
    {
        public static Dictionary<string, ELCSVehicle> ELCSVehicles = new Dictionary<string, ELCSVehicle>();
        

        internal static void ParseVcfs(List<dynamic> VcfData)
        {
            foreach (dynamic vcf in VcfData)
            {
                Debug.WriteLine($"[ELCS] Currently adding {vcf.Item1}");
                load(SettingsType.Type.VCF, vcf.Item1, vcf.Item2);
            }

            EVL.ELCSReady = true;
        }

        static void load(SettingsType.Type type, string name, string Data)
        {
            ELCSVehicle veh = new ELCSVehicle();

            var bytes = Encoding.UTF8.GetBytes(Data);
            if (bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                var ex = new Exception($"Error Loading:{name}\n" +
                                    $"Please save {name} with UTF-8 no BOM/Signature Encoding");
                throw (ex);
            }
            Encoding.UTF8.GetPreamble();
            Model hash = CitizenFX.Core.Native.API.GetHashKey(Path.GetFileNameWithoutExtension(name));
            if (type == SettingsType.Type.VCF)
            {
                NanoXMLDocument doc = new NanoXMLDocument(Data);
                if (doc.RootNode == null)
                {
                    CitizenFX.Core.Debug.WriteLine("Null issue");
                    return;
                }
                Dictionary<string, NanoXMLNode> subNodes = new Dictionary<string, NanoXMLNode>();
                foreach (NanoXMLNode node in doc.RootNode.SubNodes)
                {
                    subNodes.Add(node.Name, node);
                }

                #region Interface
                try
                {
                    foreach (NanoXMLNode n in subNodes["INTERFACE"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "ActivationType":
                                veh.activation = n.Value;
                                break;
                            case "InfoPanelButtonLightColor":
                                veh.interface_r = Int32.Parse(n.GetAttribute("red").Value);
                                veh.interface_g = Int32.Parse(n.GetAttribute("green").Value);
                                veh.interface_b = Int32.Parse(n.GetAttribute("blue").Value);
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Interface for {name} failed to parse due to {e.Message} with inner of\n {e.StackTrace}");
                }
                #endregion

                #region MISC
                try
                {
                    foreach (NanoXMLNode n in subNodes["MISC"].SubNodes)
                    {
                        switch (n.Name)
                        {
                            case "LightColors":
                                if (n.GetAttribute("Left").Value.ToLower() == "red")
                                {
                                    veh.leftColor = 28;
                                }
                                else if (n.GetAttribute("Left").Value.ToLower() == "blue")
                                {
                                    veh.leftColor = 73;
                                }

                                if (n.GetAttribute("Right").Value.ToLower() == "red")
                                {
                                    veh.rightColor = 28;
                                }
                                else if (n.GetAttribute("Right").Value.ToLower() == "blue")
                                {
                                    veh.rightColor = 73;
                                }
                                
                                break;
                            case "LightingFormat":
                                veh.lightType = n.Value.ToLower();
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Interface for {name} failed to parse due to {e.Message} with inner of\n {e.StackTrace}");
                }
                #endregion

                #region PRML
                try
                {
                    
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Interface for {name} failed to parse due to {e.Message} with inner of\n {e.StackTrace}");
                }
                #endregion

                #region SECL
                try
                {
                    
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Interface for {name} failed to parse due to {e.Message} with inner of\n {e.StackTrace}");
                }
                #endregion

                #region WRNL
                try
                {
                    
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Interface for {name} failed to parse due to {e.Message} with inner of\n {e.StackTrace}");
                }
                #endregion

                veh.model = Path.GetFileNameWithoutExtension(name);

                if (ELCSVehicles.ContainsKey(Path.GetFileNameWithoutExtension(name).ToLower()))
                {
                    ELCSVehicles.Remove(Path.GetFileNameWithoutExtension(name).ToLower());
                }

                ELCSVehicles.Add(Path.GetFileNameWithoutExtension(name).ToLower(), veh);
                Debug.WriteLine($"[ELCS] Parsed vehicle {Path.GetFileNameWithoutExtension(name)}");
            }
        }
    }
}
