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

    internal enum LightType
    {
        PRML,
        SECL,
        WRNL,
        SBRN
    }

    public class Light
    {

        Entity _vehicle;
        int _modkit;
        private LightType LightType { get; set; }
        private bool _state;
        private bool _patternRunning;
        private int _patternNumber;
        private string _pattern;
        private string _patternType;

        private int red;
        private int green;
        private int blue;

        private int count = 0;
        private int flashrate = 0;
        private bool firstTime = true;

        internal string Pattern
        {
            get { return _pattern; }
            set
            {
                if (count > value.Length - 1)
                {
                    count = 0;
                }
                _pattern = value;
            }
        }

        internal string PatternType
        {
            get { return _patternType; }
            set
            {
                _patternType = value;
            }
        }

        internal int PatternNumber
        {
            get { return _patternNumber; }
            set
            {
                _patternNumber = value;
            }
        }

        internal int modkit
        {
            get { return _modkit; }
        }

        internal void CleanUp()
        {
            SetState(false);
        }

        bool _on;
        internal bool TurnedOn
        {
            get { return _on; }
            set
            {
                _on = value;
                if (TurnedOn)
                {
                    SetState(true);
                }
                else
                {
                    SetState(false);
                }
            }
        }

        internal bool IsPatternRunning
        {
            get { return _patternRunning; }
            set
            {
                _patternRunning = value;
                if (!IsPatternRunning)
                {
                    CleanUp();
                    count = 0;
                    flashrate = 0;
                }
                else
                {
                    flashrate = Game.GameTime;
                }
            }

        }

        internal int Delay { get; set; }
        internal bool State
        {
            private set
            {
                _state = value;
                if (value)
                {
                    SetVehicleMod(_vehicle.Handle, _modkit, 1, false);
                }
                else
                {
                    SetVehicleMod(_vehicle.Handle, _modkit, 0, false);
                }
            }
            get
            {
                return _state;
            }
        }
        
        internal async void LightTicker()
        {
            if (firstTime) { flashrate = Game.GameTime; firstTime = false; }

            if (flashrate != 0 && Game.GameTime - flashrate >= 28)
            {
                if (IsPatternRunning)
                {
                    if (!IsPatternRunning)
                    {
                        CleanUp();
                        return;
                    }
                    
                    if (Pattern.ToCharArray()[count].Equals('0'))
                    {
                        DrawEnvLight();
                        SetState(true);
                        if (!IsPatternRunning)
                        {
                            CleanUp();
                            return;
                        }
                    }
                    else
                    {
                        SetState(false);
                        if (!IsPatternRunning)
                        {
                            CleanUp();
                            return;
                        }

                    }
                    count++;
                    if (count == Pattern.Length - 1)
                    {
                        count = 0;
                    }
                    if (!IsPatternRunning)
                    {
                        CleanUp();
                        return;
                    }
                }
                flashrate = Game.GameTime;

            }
        }

        internal void SetState(bool state)
        {
            this.State = state;
        }

        internal Vector3 GetBone()
        {

            Vector3 bonePos = new Vector3();
            if (_modkit == 0)
            {
                bonePos = ((Vehicle)_vehicle).Bones[$"misc_a"].Position;
            }
            if (_modkit == 1)
            {
                bonePos = ((Vehicle)_vehicle).Bones[$"misc_b"].Position;
            }
            if (_modkit == 2)
            {
                bonePos = ((Vehicle)_vehicle).Bones[$"misc_c"].Position;
            }
            if (_modkit == 3)
            {
                bonePos = ((Vehicle)_vehicle).Bones[$"misc_d"].Position;
            }
            if (_modkit == 5)
            {
                bonePos = ((Vehicle)_vehicle).Bones[$"misc_e"].Position;
            }
            if (_modkit == 6)
            {
                bonePos = ((Vehicle)_vehicle).Bones[$"misc_f"].Position;
            }
            if (_modkit == 25)
            {
                bonePos = ((Vehicle)_vehicle).Bones[$"misc_k"].Position;
            }
            if (_modkit == 27)
            {
                bonePos = ((Vehicle)_vehicle).Bones[$"misc_m"].Position;
            }


            return bonePos;
        }

        internal void DrawEnvLight()
        {
            if (!IsPatternRunning)
            {
                return;
            }
            if (_vehicle == null)
            {
                CitizenFX.Core.Debug.WriteLine("Vehicle is null!!!");
                return;
            }
            var off = _vehicle.GetPositionOffset(GetBone());
            if (off == null)
            {
                CitizenFX.Core.Debug.WriteLine("Bone is null for some reason!!!");
                return;
            }
            var extraoffset = _vehicle.GetOffsetPosition(off + new Vector3(0.0f, 0.0f, 0.0f));
            DrawLightWithRangeAndShadow(extraoffset.X, extraoffset.Y, extraoffset.Z, red, green, blue, 50.0f, 0.007f, 0.01f);
        }

        internal Light(Entity entity, int modkit, int red, int green, int blue, string format = "", bool state = false)
        {
            _vehicle = entity;
            _modkit = modkit;
            CleanUp();
            SetInfo();
            PatternType = format;
            TurnedOn = false;

            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        internal void SetInfo()
        {
            Delay = 38;
            switch (modkit)
            {
                case 0:
                    LightType = LightType.PRML;
                    Pattern = "";
                    break;
                case 1:
                    LightType = LightType.PRML;
                    Pattern = "";
                    break;
                case 2:
                    LightType = LightType.PRML;
                    Pattern = "";
                    break;
                case 25:
                    LightType = LightType.PRML;
                    Pattern = "";
                    break;
                case 27:
                    LightType = LightType.PRML;
                    Pattern = "";
                    break;
                case 3:
                    LightType = LightType.PRML;
                    Pattern = "";
                    break;
                case 5:
                    LightType = LightType.PRML;
                    Pattern = "";
                    break;
                case 6:
                    LightType = LightType.PRML;
                    Pattern = "";
                    break;

                case 7:
                    LightType = LightType.SECL;
                    Pattern = "";
                    break;
                case 8:
                    LightType = LightType.SECL;
                    Pattern = "";
                    break;
                case 9:
                    LightType = LightType.SECL;
                    Pattern = "";
                    break;
                case 10:
                    LightType = LightType.SECL;
                    Pattern = "";
                    break;
                case 26:
                    LightType = LightType.SECL;
                    Pattern = "";
                    break;
            }
        }
    }
}
