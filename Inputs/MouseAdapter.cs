using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MizJam1.Inputs
{
    public static class MouseAdapter
    {
        private static bool lastLeftClick = false;
        private static bool currentLeftClick = false;
        private static bool lastRightClick = false;
        private static bool currentRightClick = false;
        private static int lastScrollValue = 0;
        private static int currentScrollValue = 0;

        private static MouseState mouseState;
        public static void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            lastLeftClick = currentLeftClick;
            lastRightClick = currentRightClick;
            lastScrollValue = currentScrollValue;
            currentLeftClick = Mouse.GetState().LeftButton == ButtonState.Pressed;
            currentRightClick = Mouse.GetState().RightButton == ButtonState.Pressed;
            currentScrollValue = mouseState.ScrollWheelValue;
        }
        public static bool LeftClick => currentLeftClick && !lastLeftClick;
        public static bool ConsumeLeftClick
        {
            get
            {
                bool res = LeftClick;
                lastLeftClick = currentLeftClick;
                return res;
            }
        }
        public static bool RightClick => currentRightClick && !lastRightClick;
        public static bool ConsumeRightClick
        {
            get
            {
                bool res = RightClick;
                lastRightClick = currentRightClick;
                return res;
            }
        }
        public static Point Position => mouseState.Position;
        public static bool ScrollUp => currentScrollValue > lastScrollValue;
        public static bool ScrollDown => currentScrollValue < lastScrollValue;
    }
}
