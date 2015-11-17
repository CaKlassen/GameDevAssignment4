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
    /// This class is the Breakout game's ball.
    /// </summary>
    public class Ball : EntityCollide
    {
        public static int START_X = Game1.instance.GraphicsDevice.Viewport.Bounds.Width / 2;
        public static int START_Y = Game1.instance.GraphicsDevice.Viewport.Bounds.Height / 2;

        public static int REBOUND_ADD = 2;

        public static int START_SPD = 5;

        bool start = false;

        Texture2D sprite;
        Vector2 spd;

        /// <summary>
        /// This is the default ball constructor.
        /// </summary>
        /// <param name="Content">The Content Manager to use for loading</param>
        public Ball(ContentManager Content)
        {
            sprite = Content.Load<Texture2D>("Art/Ball");
            mask = sprite;

            pos = new Vector2(START_X - sprite.Width / 2, START_Y);
            spd = new Vector2(0, 0);
        }

        /// <summary>
        /// This function updates the ball every frame.
        /// </summary>
        /// <param name="gamepadState">The state of the gamepad</param>
        /// <param name="keyboardState">The state of the keyboard</param>
        /// <param name="mouseState">The state of the mouse</param>
        public override void update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {
            base.update(gamepadState, keyboardState, mouseState);

            // Starting
            if (!start && !Console.instance.isOpen() && (keyboardState.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.A)))
            {
                start = true;

                // Start the music
                MediaPlayer.Play(AudioUtils.getInstance().mscGame);

                // Set the randomized start speed
                Random r = new Random();

                spd.X = (float) r.NextDouble() * (START_SPD - 2) + 1;
                spd.Y = (float) Math.Sqrt(Math.Pow(START_SPD, 2) - Math.Pow(spd.X, 2));

                // Randomize the horizontal direction
                if (r.NextDouble() > 0.5)
                {
                    spd.X = -spd.X;
                }
            }

            // Bouncing off room edges
            if (pos.X <= 0 || pos.X >= Game1.instance.GraphicsDevice.Viewport.Bounds.Width - mask.Width)
            {
                bounce(true);
            }

            if (pos.Y <= 0)
            {
                bounce(false);
            }

            // Losing
            if (pos.Y > Game1.instance.GraphicsDevice.Viewport.Bounds.Height)
            {
                Game1.instance.gameOver();

                // Stop the music
                MediaPlayer.Stop();
            }

            // Movement
            pos.X += spd.X;
            pos.Y += spd.Y;


            // COLLISIONS //
            List<EntityCollide> collisionList = Game1.instance.getCollisionList();

            foreach (EntityCollide entity in collisionList)
            {
                if (entity is Brick && collide(entity))
                {
                    // Collision

                    if (collideLeft(entity) || collideRight(entity))
                    {
                        bounce(true);
                    }
                    else
                    {
                        bounce(false);
                    }

                    Game1.instance.removeBrick((Brick) entity);

                    break;
                }
                else if (entity is Paddle && collide(entity))
                {
                    // Reset the combo
                    Game1.instance.resetCombo();

                    // Bounce
                    if (collideLeft(entity) || collideRight(entity))
                    {
                        bounce(true);
                    }
                    else if(collideTop(entity) || collideBottom(entity))
                    {
                        bounce(false);

                        if (keyboardState.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.LeftThumbstickRight))
                        {
                            spd.X += REBOUND_ADD;
                        }
                        else if (keyboardState.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.LeftThumbstickLeft))
                        {
                            spd.X -= REBOUND_ADD;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// This function bounces the ball in one direction.
        /// </summary>
        /// <param name="horizontal">Whether or not to bounce horizontally</param>
        public void bounce(bool horizontal)
        {
            AudioUtils.getInstance().sndBounce.Play();

            if (horizontal)
            {
                spd.X = -spd.X;
            }
            else
            {
                spd.Y = -spd.Y;
            }
        }
        
        /// <summary>
        /// This function stops the ball's movement.
        /// </summary>
        public void stop()
        {
            spd = new Vector2(0, 0);
        }

        /// <summary>
        /// This function renders the ball.
        /// </summary>
        /// <param name="sb">The sprite batch object to use for rendering</param>
        public override void draw(SpriteBatch sb)
        {
            sb.Begin();

            sb.Draw(sprite, pos, Color.White);

            sb.End();
        }
    }
}
