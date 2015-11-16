using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;

namespace Assignment2.Entities
{
    /// <summary>
    /// This class is a game console that can be used to manipulate the game.
    /// </summary>
    class Console
    {
        public static int WIDTH = 500;
        public static int HEIGHT = 300;
        public static int TEXT_BUFFER = 10;
        public static int MAX_LENGTH = 42;
        public static int MAX_LINES = 10;

        public static int ALPHA = 150;

        private bool open = false;
        private bool keyDown = false;

        private string text = "";

        private KeyboardState oldState;

        Texture2D rectangle;
        SpriteFont font;

        private Dictionary<string, Func<string, bool>> commandList;
        private Queue<string> chatLines;

        public static Console instance;

        /// <summary>
        /// This is the constructor for the console.
        /// </summary>
        /// <param name="font">The font for the console</param>
        /// <param name="graphics">The graphics device for creating the console texture</param>
        public Console(SpriteFont font, GraphicsDeviceManager graphics)
        {
            instance = this;

            this.font = font;

            // Create the rectangle
            rectangle = new Texture2D(Game1.instance.GraphicsDevice, WIDTH, HEIGHT, false, SurfaceFormat.Color);

            // set the color to the amount of pixels
            Color[] color = new Color[WIDTH * HEIGHT];

            // loop through all the colors setting them to whatever values we want
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = new Color(0, 0, 0, ALPHA);
            }

            // set the color data on the texture
            rectangle.SetData(color);

            // Set up the command list
            commandList = new Dictionary<string, Func<string, bool>>()
            {
                { "setBackground", setBackground },
                { "resetGame", resetGame }
            };

            // Create the chat queue
            chatLines = new Queue<string>();
        }

        /// <summary>
        /// This function updates the console.
        /// </summary>
        public void update()
        {
            if (!keyDown && open)
            {
                // Take key input
                Keys[] keys = Keyboard.GetState().GetPressedKeys();

                if (keys.Length > 0)
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (keys[i].Equals(Keys.Back) && text.Length > 0)
                        {
                            // Backspace
                            if (oldState.IsKeyUp(keys[i]))
                            {
                                text = text.Substring(0, text.Length - 1);
                            }
                        }
                        else if (keys[i].Equals(Keys.Enter) && text.Length > 0)
                        {
                            // Backspace
                            if (oldState.IsKeyUp(keys[i]))
                            {
                                handleCommand(text);
                                text = "";
                            }
                        }
                        else if (isChar(keys[i]) || isDigit(keys[i]) || keys[i].Equals(Keys.Space) || keys[i].Equals(Keys.OemPeriod))
                        {
                            if (text.Length < MAX_LENGTH && oldState.IsKeyUp(keys[i]))
                            {
                                // Add the key to the command string
                                if (keys[i].Equals(Keys.Space))
                                {
                                    text += " ";
                                }
                                else if (keys[i].Equals(Keys.OemPeriod))
                                {
                                    text += ".";
                                }
                                else if (isDigit(keys[i]))
                                {
                                    text += keys[i].ToString().Substring(1);
                                }
                                else
                                {
                                    text += keys[i];
                                }
                            }
                        }
                    }
                }

                oldState = Keyboard.GetState();
            }

            // Toggle the open state
            if (!open && Keyboard.GetState().IsKeyDown(Keys.C) ||
                open && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (!keyDown)
                {
                    keyDown = true;
                    text = "";

                    open = !open;
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyUp(Keys.C))
                {
                    keyDown = false;
                }
            }

        }
        
        /// <summary>
        /// This function renders the console
        /// </summary>
        /// <param name="sb">The sprite batch for rendering</param>
        public void draw(SpriteBatch sb)
        {
            if (open)
            {
                sb.Begin();

                // Draw the rectangle
                sb.Draw(rectangle, new Vector2(0, 0), Color.White);

                sb.DrawString(font, text, new Vector2(TEXT_BUFFER, HEIGHT - TEXT_BUFFER - font.MeasureString(text).Y), Color.White);

                for (int i = 0; i < chatLines.Count; i++)
                { 
                    sb.DrawString(font, chatLines.ElementAt(i), new Vector2(TEXT_BUFFER, TEXT_BUFFER + font.MeasureString("A").Y * i), Color.White);
                }
                
                sb.End();
            }
        }

        /// <summary>
        /// This function prints a line to the console.
        /// </summary>
        /// <param name="line">The line to print</param>
        public void printLine(string line)
        {
            if (chatLines.Count == MAX_LINES)
            {
                // We have to push a line out
                chatLines.Dequeue();
                chatLines.Enqueue(line);
            }
            else
            {
                chatLines.Enqueue(line);
            }
        }

        /// <summary>
        /// This function interprets a command from the user.
        /// </summary>
        /// <param name="text">The text from the user</param>
        public void handleCommand(string text)
        {
            // Print the command given to the console
            printLine(text);

            // Split the text to retrieve the command name
            string[] splitText = new string[2];

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals(' '))
                {
                    splitText[0] = text.Substring(0, i);
                    splitText[1] = text.Substring(i + 1);
                    break;
                }
            }

            if (splitText[0] == null)
            {
                splitText[0] = text;
            }


            if (splitText[0].Length > 0)
            {
                // Loop through each command in the database
                foreach (string command in commandList.Keys)
                {
                    if (splitText[0].Equals(command.ToUpper()))
                    {
                        bool result;

                        // Call the corresponding function
                        if (splitText.Length > 1)
                        {
                            result = commandList[command](splitText[1]);
                        }
                        else
                        {
                            result = commandList[command](null);
                        }

                        // Check result
                        if (!result)
                        {
                            // Print an error message
                            printLine("Invalid command format.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function sets the background colour of the game
        /// </summary>
        /// <param name="value">The parameters given by the player</param>
        /// <returns>Whether or not the user's input was valid</returns>
        public bool setBackground(string value)
        {
            if (value == null)
            {
                return false;
            }

            string[] components = new string[3];
            int numComponents = 0;
            int prevEnd = 0;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Equals(' '))
                {
                    components[numComponents++] = value.Substring(prevEnd, i - prevEnd);
                    prevEnd = i + 1;
                }
            }

            components[numComponents++] = value.Substring(prevEnd, value.Length - prevEnd);

            int[] colourComponents = new int[3];

            if (numComponents == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    // Parse the component and check its value
                    colourComponents[i] = Int32.Parse(components[i]);

                    if (colourComponents[i] < 0 || colourComponents[i] > 255)
                    {
                        return false;
                    }
                }

                // Check to see if the colour provided is valid
                Game1.instance.setBackgroundColor(new Color(colourComponents[0], colourComponents[1], colourComponents[2]));
                printLine("Background colour changed to: (" + colourComponents[0] + ", " + colourComponents[1] + ", " + colourComponents[2] + ")");
            }
            else
            {
                printLine("Invalid colour entered.");
            }


            return true;
        }

        /// <summary>
        /// This function resets the game.
        /// </summary>
        /// <param name="value">The parameters given by the user</param>
        /// <returns>Whether or not the user's input was valid</returns>
        public bool resetGame(string value)
        {
            Game1.instance.reset();
            printLine("Game reset.");

            return true;
        }

        /// <summary>
        /// This function returns whether or not the console is open.
        /// </summary>
        /// <returns>Whether or not the console is open</returns>
        public bool isOpen()
        {
            return open;
        }

        /// <summary>
        /// This function toggles the open state of the console.
        /// </summary>
        public void toggleOpen()
        {
            open = !open;
        }

        /// <summary>
        /// This function checks if a user entered character is an alpha character.
        /// </summary>
        /// <param name="key">The given key</param>
        /// <returns>Whether or not the key is a character</returns>
        private bool isChar(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }

        /// <summary>
        /// This function checks if a user entered character is a number.
        /// </summary>
        /// <param name="key">The given key</param>
        /// <returns>Whether or not the key is a number</returns>
        private bool isDigit(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }
    }
}
