using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVLClient.Utils;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace EVLClient.Utils
{
    class Panel
    {

        public static async Task _DrawText(string text, int red, int green, int blue, int alpha, float x, float y, float width, float height, bool center, int font)
        {
            SetTextColour(red, green, blue, alpha);
            SetTextFont(font);
            SetTextScale(width, height);
            SetTextWrap(0.0f, 1.0f);
            SetTextCentre(center);
            SetTextDropshadow(0, 0, 0, 0, 0);
            SetTextEdge(1, 0, 0, 0, 255);
            SetTextEntry("STRING");
            AddTextComponentString(text);
            DrawText(x, y);
            await Task.FromResult(0);
        }

        public static async Task _DrawRect(float x, float y, float w, float h, int r, int g, int b, int a) {
            DrawRect(x, y, w, h, r, g, b, a);
            await Task.FromResult(0);
        }

        public static async Task _DrawSprite(string dict, string texture, float x, float y, float w, float h, float head, int r, int g, int b, int a) {
            if (!HasStreamedTextureDictLoaded(dict))
                RequestStreamedTextureDict(dict, true);
            DrawSprite(dict, texture, x, y, w, h, head, r, g, b, a);
            await Task.FromResult(0);
        }

        public static async Task DrawStageDisplay(int stage) {
            if (!HasStreamedTextureDictLoaded("shared")) RequestStreamedTextureDict("shared", true);

            switch (stage) {
                case 0: 
                    {
                        await _DrawRect(0.781f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawRect(0.795f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawRect(0.81f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawSprite("shared", "medaldot_32", 0.757f, 0.546f, 0.03f, 0.04f, 0, 140, 140, 140, 255);
                        break; 
                    }
                case 1:
                    {
                        await _DrawRect(0.781f, 0.531f, 0.0079f, 0.0049f, 0, 255, 0, 255);
                        await _DrawRect(0.795f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawRect(0.81f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawSprite("shared", "medaldot_32", 0.782f, 0.546f, 0.03f, 0.04f, 0, 140, 140, 140, 255);
                        break;
                    }
                case 2:
                    {
                        await _DrawRect(0.781f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawRect(0.795f, 0.531f, 0.0079f, 0.0049f, 255, 150, 0, 255);
                        await _DrawRect(0.81f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawSprite("shared", "medaldot_32", 0.795f, 0.546f, 0.03f, 0.04f, 0, 140, 140, 140, 255);
                        break;
                    }
                case 3:
                    {
                        await _DrawRect(0.781f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawRect(0.795f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawRect(0.81f, 0.531f, 0.0079f, 0.0049f, 255, 60, 60, 255);
                        await _DrawSprite("shared", "medaldot_32", 0.81f, 0.546f, 0.03f, 0.04f, 0, 140, 140, 140, 255);
                        break;
                    }
                default:
                    {
                        await _DrawRect(0.781f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawRect(0.795f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawRect(0.81f, 0.531f, 0.0079f, 0.0049f, 50, 50, 50, 255);
                        await _DrawSprite("shared", "medaldot_32", 0.757f, 0.546f, 0.03f, 0.04f, 0, 140, 140, 140, 255);
                        break;
                    }
            }

            await Task.FromResult(0);
        }

        public static async Task DrawPanelButton(string text, bool on, float x, float y, int infoButtonLightRed, int infoButtonLightGreen, int infoButtonLightBlue) {

            if (on)
            {
                await _DrawSprite("EVL", "round_box", x, y, 0.024f, 0.042f, 0, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue, 255);
                await _DrawRect(x, y, 0.016f, 0.024f, 20, 20, 20, 255);
                await _DrawText(text, infoButtonLightRed, infoButtonLightGreen, infoButtonLightBlue, 255, x, (y - 0.008f), 0.21f, 0.21f, true, 0);
            }
            else
            {
                await _DrawSprite("EVL", "round_box", x, y, 0.024f, 0.042f, 0, 140, 140, 140, 255);
                await _DrawRect(x, y, 0.016f, 0.024f, 20, 20, 20, 255);
                await _DrawText(text, 140, 140, 140, 255, x, (y - 0.008f), 0.21f, 0.21f, true, 0);
            }

            await Task.FromResult(0);
        }
    }
}
