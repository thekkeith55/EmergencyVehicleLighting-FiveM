using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVLServer.Utils
{
    internal class SettingsType
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

        internal enum Type
        {
            GLOBAL,
            VCF
        }
    }
}
