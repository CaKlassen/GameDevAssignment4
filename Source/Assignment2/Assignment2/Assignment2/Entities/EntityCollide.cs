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
    /// This class represents an entity that can be collided with.
    /// </summary>
    public abstract class EntityCollide : Entity
    {
        protected Texture2D mask;
        protected Vector2 pos;
        protected Vector2 prevPos;

        /// <summary>
        /// This is the constructor for the collidable entity.
        /// </summary>
        public EntityCollide()
        {

        }

        /// <summary>
        /// This function updates the collidable entity every frame.
        /// </summary>
        /// <param name="gamepadState">The state of the gamepad</param>
        /// <param name="keyboardState">The state of the keyboard</param>
        /// <param name="mouseState">The state of the mouse</param>
        public override void update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {
            prevPos = pos;
        }

        /// <summary>
        /// This function returns the entity's collision mask.
        /// </summary>
        /// <returns>The collision mask</returns>
        public Texture2D getMask()
        {
            return mask;
        }

        /// <summary>
        /// This function returns the position of the collidable entity.
        /// </summary>
        /// <returns>The entity's position</returns>
        public Vector2 getPos()
        {
            return pos;
        }

        public Vector2 getPrevPos()
        {
            return prevPos;
        }

        /// <summary>
        /// This function checks if the entity has collided with another entity.
        /// </summary>
        /// <param name="entity">The other entity</param>
        /// <returns>Whether or not a collision occurred</returns>
        public bool collide(EntityCollide entity)
        {
            if (new Rectangle((int) pos.X, (int) pos.Y, mask.Width, mask.Height).Intersects(
                    new Rectangle((int) entity.getPos().X, (int) entity.getPos().Y, entity.getMask().Width, entity.getMask().Height)
                    ))
            {
                // Collision
                return true;
            }

            return false;
        }

        /// <summary>
        /// This function checks for a collision from the top.
        /// </summary>
        /// <param name="entity">The entity to check for a collision against</param>
        /// <returns>Whether or not the collision occurred</returns>
        public bool collideTop(EntityCollide entity)
        {
            return (((int) prevPos.Y) + mask.Height <= entity.getPrevPos().Y &&
                ((int) pos.Y) + mask.Height >= entity.getPos().Y);
        }

        /// <summary>
        /// This function checks for a collision from the left.
        /// </summary>
        /// <param name="entity">The entity to check for a collision against</param>
        /// <returns>Whether or not the collision occurred</returns>
        public bool collideLeft(EntityCollide entity)
        {
            return (((int)prevPos.X) + mask.Width <= entity.getPrevPos().X &&
                ((int)pos.X) + mask.Width >= entity.getPos().X);
        }

        /// <summary>
        /// This function checks for a collision from the right.
        /// </summary>
        /// <param name="entity">The entity to check for a collision against</param>
        /// <returns>Whether or not the collision occurred</returns>
        public bool collideRight(EntityCollide entity)
        {
            return (((int)prevPos.X) >= entity.getPrevPos().X + entity.getMask().Width &&
                ((int)pos.X) <= entity.getPos().X + entity.getMask().Width);
        }

        /// <summary>
        /// This function checks for a collision from the bottom.
        /// </summary>
        /// <param name="entity">The entity to check for a collision against</param>
        /// <returns>Whether or not the collision occurred</returns>
        public bool collideBottom(EntityCollide entity)
        {
            return (((int)prevPos.Y) >= entity.getPrevPos().Y + entity.getMask().Height &&
                ((int)pos.Y) <= entity.getPos().Y + entity.getMask().Height);
        }
    }
}
