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
    public enum BrickTypes
    {
        HIGH,
        MED,
        LOW
    }

    /// <summary>
    /// This class is a brick in the breakout game.
    /// </summary>
    public class Brick : EntityCollide
    {
        Texture2D sprite;
        private int points;

        /// <summary>
        /// This is the constructor for the brick.
        /// </summary>
        /// <param name="Content">The content manager for loading resources</param>
        /// <param name="pos">The starting position of the brick</param>
        /// <param name="type">The type of brick</param>
        public Brick(ContentManager Content, Vector2 pos, BrickTypes type)
        {
            switch(type)
            {
                case BrickTypes.LOW:
                {
                    sprite = Content.Load<Texture2D>("Art/Brick1");
                    points = 100;
                    break;
                }

                case BrickTypes.MED:
                {
                    sprite = Content.Load<Texture2D>("Art/Brick2");
                    points = 200;
                    break;
                }

                case BrickTypes.HIGH:
                {
                    sprite = Content.Load<Texture2D>("Art/Brick3");
                    points = 300;
                    break;
                }
            }

            
            mask = sprite;

            this.pos = pos;
            this.prevPos = pos;
        }

        /// <summary>
        /// The update function for the brick
        /// </summary>
        /// <param name="gamepadState">The state of the gamepad</param>
        /// <param name="keyboardState">The state of the keyboard</param>
        /// <param name="mouseState">The state of the mouse</param>
        public override void update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {
            
        }

        /// <summary>
        /// This function renders the brick to the screen.
        /// </summary>
        /// <param name="sb">The sprite batch to use for rendering</param>
        public override void draw(SpriteBatch sb)
        {
            sb.Begin();

            sb.Draw(sprite, pos, Color.White);

            sb.End();
        }

        /// <summary>
        /// 
        /// This function returns the points earned for destroying the brick.
        /// </summary>
        /// <returns>the points earned for breaking the brick</returns>
        public int getPoints()
        {
            return points;
        }
    }
}
