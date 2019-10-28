using System;
using UnityEngine;
using System.Collections.Generic;
using IPALogger = IPA.Logging.Logger;

namespace RainbowLighting
{
    class ColorManagerPlus : ColorManager
    {
        const int skipCount = 10;
        List<Color> colorsA;
        List<Color> colorsB;
        int counterA = 0;
        int counterB = 0;
        int idleA = skipCount;
        int idleB = skipCount;
        IPALogger logger;
        public ColorManagerPlus()
        {
            colorsA = Rainbows();
            colorsB = Rainbows2();
            
            
        }
        public void SetLogger(IPALogger log)
        {
            logger = log;
            /*
            logger.Log(IPALogger.Level.Info, "In Constructor");
            foreach (Color i in colorsA)
            {
                logger.Log(IPALogger.Level.Info, i.ToString());
            }
            foreach (Color i in colorsB)
            {
                logger.Log(IPALogger.Level.Info, i.ToString());
            }
            */
        }
        public override Color ColorForSaberType(Saber.SaberType type) //currently this doesnt work
        {
            if (type == Saber.SaberType.SaberB)
            {
                if (--idleB == 0)
                {
                    idleB = skipCount;
                    if(++counterB == colorsB.Count)
                    {
                        counterB = 0;
                    }
                    //logger.Log(IPALogger.Level.Info, string.Format("Saber color B index = {0}", counterB));
                    _saberBColor.SetColor(colorsB[counterB]);
                    //logger.Log(IPALogger.Level.Info, _saberBColor.ToString());
                }
                return _saberBColor;
            }
            else
            {
                if (--idleA == 0)
                {
                    idleA = skipCount;
                    if (++counterA == colorsA.Count)
                    {
                        counterA = 0;
                    }
                    //logger.Log(IPALogger.Level.Info, string.Format("Saber color A index = {0}", counterA));
                    _saberAColor.SetColor(colorsA[counterA]);
                    //logger.Log(IPALogger.Level.Info, _saberAColor.ToString());
                }
                return _saberAColor;
            }
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
        #region Literal Rainbows
        public List<Color> Rainbows()
        {
            List<Color> colors = new List<Color>();
            int r = 255;
            int g = 0;
            int b = 255;
            while(b > 0)
            {
                b--;
                colors.Add(new Color(r, g, b));
                
            }
            while(g < 255)
            {
                g++;
                colors.Add(new Color(r, g, b));
            }
            while(r > 0)
            {
                r--;
                colors.Add(new Color(r, g, b));
            }
            while(b < 255)
            {
                b++;
                colors.Add(new Color(r, g, b));
            }
            while(g > 0)
            {
                g--;
                colors.Add(new Color(r, g, b));
            }
            while(r < 255)
            {
                r++;
                colors.Add(new Color(r, g, b));
            }
            colors.Add(new Color(r, g, b));
            return colors;
        }
        public List<Color> Rainbows2()
        {
            List<Color> colors = new List<Color>();
            int r = 255;
            int g = 255;
            int b = 0;
            while (r > 0)
            {
                r--;
                colors.Add(new Color(r, g, b));
            }
            while (b < 255)
            {
                b++;
                colors.Add(new Color(r, g, b));
            }
            while (g > 0)
            {
                g--;
                colors.Add(new Color(r, g, b));
            }
            while (r < 255)
            {
                r++;
                colors.Add(new Color(r, g, b));
            }
            while (b > 0)
            {
                b--;
                colors.Add(new Color(r, g, b));
            }
            while (g < 255)
            {
                g++;
                colors.Add(new Color(r, g, b));
            }
            colors.Add(new Color(r, g, b));
            return colors;
        }
        #endregion
        
    }
}