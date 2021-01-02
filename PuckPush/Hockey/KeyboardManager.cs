using System;
using System.Collections.Generic;
using System.Text;
using Hockey.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hockey
{
    class KeyboardManager
    {
        public KeyboardManager(Keys Key)
        {
            key = Key;
            isHeld = false;
        }

        public KeyboardManager(KeyboardState kstate)
        {
            state = kstate;
        }

        public bool IsPressed { get { return KeyIsPressed(); } }

        public static void Update() { state = Keyboard.GetState(); }

        private Keys key;
        private bool isHeld;
        private static KeyboardState state;
        public bool KeyIsPressed()
        {
            if (state.IsKeyDown(key))
            {
                if (isHeld) return false;
                else
                {
                    isHeld = true;
                    return true;
                }
            }
            else
            {
                if (isHeld) isHeld = false;
                return false;
            }
        }

        public string TryUpdateInputText(string userInput, Keys newInput, int maxLength, RoomInput roomInput)
        {
            if (!roomInput.isClicked)
            {
                return userInput;
            }
            key = newInput;
            if (KeyIsPressed())
            {
                if (newInput.ToString() != "None")
                {
                    var textLength = userInput.ToString().Length;
                    if (newInput.ToString() == "Back")
                    {
                        if (textLength - 1 > 0)
                        {
                            var oldInput = userInput;
                            userInput = oldInput.Substring(0, oldInput.Length - 1);
                            return userInput;
                        } else
                        {
                            return "";
                        }
                    } else
                    {
                        if (userInput.Length < maxLength)
                        {
                            userInput += newInput;
                        }
                    }
                }
            }
            return userInput;
        }

    }
}
