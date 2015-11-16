using System;
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
    /// This class is the breakout game's paddle.
    /// </summary>
    public class Paddle : EntityCollide
    {
        public static int START_X = Game1.instance.GraphicsDevice.Viewport.Bounds.Width / 2;
        public static int START_Y = Game1.instance.GraphicsDevice.Viewport.Bounds.Height - 64;

        public static int MIN_X = 32;
        public static int MAX_X = Game1.instance.GraphicsDevice.Viewport.Bounds.Width - MIN_X - 160;

        public static int MOVE_SPEED = 7;
        
        Texture2D sprite;

        /// <summary>
        /// This is the constructor for the paddle.
        /// </summary>
        /// <param name="Content">The content manager for loading resources</param>
        public Paddle(ContentManager Content)
        {
            sprite = Content.Load<Texture2D>("Art/Paddle");
            mask = sprite;

            pos = new Vector2(START_X - sprite.Width / 2, START_Y);
        }

        /// <summary>
        /// This function updates the paddle every frame.
        /// </summary>
        /// <param name="gamepadState">The gamepad state</param>
        /// <param name="keyboardState">The keyboard state</param>
        /// <param name="mouseState">The mouse state</param>
        public override void update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {
            base.update(gamepadState, keyboardState, mouseState);

            // MOVEMENT //

            if (keyboardState.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                // Move left
                pos.X -= MOVE_SPEED;
                pos.X = MathHelper.Clamp(pos.X, MIN_X, MAX_X);
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                // Move right
                pos.X += MOVE_SPEED;
                pos.X = MathHelper.Clamp(pos.X, MIN_X, MAX_X);
            }
        }

        /// <summary>
        /// This function handles the rendering of the paddle.
        /// </summary>
        /// <param name="sb">The sprite batch used for rendering</param>
        public override void draw(SpriteBatch sb)
        {
            sb.Begin();

            sb.Draw(sprite, pos, Color.White);

            sb.End();
        }
    }
}
