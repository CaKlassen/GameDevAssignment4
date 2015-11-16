﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Assignment2.Entities
{
    /// <summary>
    /// This class is responsible for managing and rendering the game's HUD to the screen.
    /// </summary>
    public class HUD : Entity
    {
        public static int START_OFFSET = 32;
        public static int COMBO_OFFSET = 40;

        private SpriteFont font;
        private SpriteFont smallFont;
        private SpriteFont endFont;

        private int score = 0;
        private bool end = false;
        private bool win = false;

        /// <summary>
        /// This is the constructor for the HUD.
        /// </summary>
        /// <param name="Content"></param>
        public HUD(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("HUD");
            smallFont = Content.Load<SpriteFont>("HUDsmall");
            endFont = Content.Load<SpriteFont>("EndFont");
        }

        /// <summary>
        /// This function handles the updating of the HUD every frame.
        /// </summary>
        /// <param name="gamepadState">The state of the gamepad</param>
        /// <param name="keyboardState">The state of the keyboard</param>
        /// <param name="mouseState">The state of the mouse</param>
        public override void update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {
            
        }

        /// <summary>
        /// This function handles the rendering of the HUD.
        /// </summary>
        /// <param name="sb">The sprite batch object to use</param>
        public override void draw(SpriteBatch sb)
        {
            sb.Begin();

            sb.DrawString(font, "" + score, new Vector2(START_OFFSET, START_OFFSET), Color.White);
            sb.DrawString(smallFont, "x" + Game1.instance.getCombo(), new Vector2(START_OFFSET, START_OFFSET + COMBO_OFFSET), Color.White);

            if (end)
            {
                sb.DrawString(endFont, "GAME OVER", new Vector2(Game1.instance.GraphicsDevice.Viewport.Bounds.Width / 2, Game1.instance.GraphicsDevice.Viewport.Bounds.Height / 2),
                    Color.White, 0, new Vector2(endFont.MeasureString("GAME OVER").X / 2, endFont.MeasureString("GAME OVER").Y / 2), 1, 0, 0);
            }

            if (win)
            {
                sb.DrawString(endFont, "YOU WIN", new Vector2(Game1.instance.GraphicsDevice.Viewport.Bounds.Width / 2, Game1.instance.GraphicsDevice.Viewport.Bounds.Height / 2),
                    Color.White, 0, new Vector2(endFont.MeasureString("YOU WIN").X / 2, endFont.MeasureString("YOU WIN").Y / 2), 1, 0, 0);
                sb.DrawString(endFont, "" + score, new Vector2(Game1.instance.GraphicsDevice.Viewport.Bounds.Width / 2, Game1.instance.GraphicsDevice.Viewport.Bounds.Height / 2 + COMBO_OFFSET * 2),
                    Color.White, 0, new Vector2(endFont.MeasureString("" + score).X / 2, endFont.MeasureString("" + score).Y / 2), 1, 0, 0);
            }

            sb.End();
        }

        /// <summary>
        /// This function adds to the total score.
        /// </summary>
        /// <param name="score">The score to add</param>
        public void addScore(int score)
        {
            this.score += score;
        }

        /// <summary>
        /// This function triggers the game over scene.
        /// </summary>
        public void gameOver()
        {
            end = true;
        }

        /// <summary>
        /// This function triggers the game win scnee.
        /// </summary>
        public void gameWin()
        {
            win = true;
        }

        /// <summary>
        /// This function resets the HUD.
        /// </summary>
        public void reset()
        {
            end = false;
            win = false;

            score = 0;
        }
    }
}