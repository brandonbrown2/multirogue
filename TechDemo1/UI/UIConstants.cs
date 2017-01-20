using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Consoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDemo1.UI
{
    static class UIConstants
    {

        public static AnimatedTextSurface playerAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
        public static AnimatedTextSurface targetAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
        public static AnimatedTextSurface SecondPlayerAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
        public static Random rand;

        public static void init()
        {
            rand = new Random();

            playerAnimation.CreateFrame();
            playerAnimation.CurrentFrame[0].Foreground = Color.Orange;
            playerAnimation.CurrentFrame[0].GlyphIndex = '@';

            targetAnimation.CreateFrame();
            targetAnimation.CurrentFrame[0].Foreground = Color.Red;
            targetAnimation.CurrentFrame[0].GlyphIndex = 'X';

            SecondPlayerAnimation.CreateFrame();
            SecondPlayerAnimation.CurrentFrame[0].Foreground = Color.LightBlue;
            SecondPlayerAnimation.CurrentFrame[0].GlyphIndex = '@';
        }
    }
}
