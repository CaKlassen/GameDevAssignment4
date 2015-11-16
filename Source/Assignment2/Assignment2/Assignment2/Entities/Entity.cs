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
    /// The abstract base class for a game entity.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// The base entity constructor.
        /// </summary>
        public Entity()
        {

        }

        /// <summary>
        /// This function updates the entity each frame.
        /// </summary>
        /// <param name="gamepadState">The gamepad state</param>
        /// <param name="keyboardState">The keyboard state</param>
        /// <param name="mouseState">The mouse state</param>
        public abstract void update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState);

        /// <summary>
        /// This function renders the entity each frame.
        /// </summary>
        /// <param name="sb"></param>
        public abstract void draw(SpriteBatch sb);
    }
}
