using System;
using UnityEngine;

namespace RainbowLighting
{
    class ColorManagerPlus : ColorManager
    {
        ColorManagerPlus()
        {
            this._hueA = 240;
            this._hueB = 0;
        }
        public override Color ColorForSaberType(Saber.SaberType type) //currently this doesnt work
        {
            Color rgbColor;
            float hue, saturation, value;
            if (type == Saber.SaberType.SaberB)
            {
                rgbColor = this._saberBColor;
                Color.RGBToHSV(_saberBColor, out hue, out saturation, out value);
                saturation = 1f; value = 1f;
                hue = _hueB;
                return Color.HSVToRGB(hue, saturation, value);
            }
            else
            {
                rgbColor = this._saberAColor;
                Color.RGBToHSV(_saberAColor, out hue, out saturation, out value);
                saturation = 1f; value = 1f;
                hue = _hueA;
                return Color.HSVToRGB(hue, saturation, value);
            }
        }
        public void UpdateColorFrame()
        {
            RotateColor(ref this._hueA);
            RotateColor(ref this._hueB);
            
        }
        
        private void RotateColor(ref int hue)
        {
            hue = ((hue + 2) % 360);
        }
        #region Unused
        private void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        private int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
        #endregion
        private int _hueA, _hueB;
    }
}