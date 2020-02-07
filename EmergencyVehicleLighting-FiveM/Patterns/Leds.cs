using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVLClient.Patterns
{
    class Leds
    {

        public static Dictionary<int, Dictionary<int, string>> LightStageOne = new Dictionary<int, Dictionary<int, string>>
        {
            {1, new Dictionary<int, string>{
                {0,  "11111111111000000000011111111"},
                {1,  "11111111111100000000000000000"},
                {2,  "11111111111100000000000000000"},
                {3,  "11111111111100000000000000000"},
                {4,  "11111111111100000000000000000"},
                {5,  "00000000000000000111111111111"},
                {6,  "00000000000111111111100000000"},
                {7,  "11111000011110000001111110000"},
                {8,  "00000111100001111110000001111"},
                {9,  "11111000011110000001111110000"},
                {10,  "00000111100001111110000001111"},
            } }
        };
    }
}
