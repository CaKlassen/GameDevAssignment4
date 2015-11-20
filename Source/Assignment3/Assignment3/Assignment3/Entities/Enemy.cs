using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Assignment3.Scenes;
using Assignment3.Utilities;

namespace Assignment3.Entities
{
    public class Enemy : Entity
    {
        public static int WALL_LENGTH = 200;
        private static int MOVE_WAIT = 10;
        private static int MOVE_SPEED = 15;

        public Model model;
        private Vector3 mazePos;
        private Vector3 pos;
        private Texture2D texture;

        private float angle = 0;
        private int wait = MOVE_WAIT;
        private bool canMove = true;
        private Vector3 targetPos;
        private bool[,] maze;

        private float scale = 0.02f;

        public Enemy(ContentManager content, Vector3 position, bool[,] rawMaze)
        {
            maze = rawMaze;

            mazePos = position;
            pos = position;
            pos.X *= (WALL_LENGTH * scale);
            pos.Y *= (WALL_LENGTH * scale);
            pos.Z *= (WALL_LENGTH * scale);

            targetPos = pos;

            model = content.Load<Model>("Models/monster_textured");
            texture = content.Load<Texture2D>("Models/monster_texture");
        }
        
        public override void update(GameTime gameTime, GamePadState gamepad, KeyboardState keyboard)
        {
            if (canMove)
            {
                // Find a new target
                Random r = new Random();
                int newX, newY, newDir;

                do
                {
                    newDir = r.Next(4);

                    switch(newDir)
                    {
                        case 0:
                        {
                            newX = -1;
                            newY = 0;
                            angle = 90f;
                            break;
                        }
                        case 1:
                        {
                            newX = 1;
                            newY = 0;
                            angle = 270f;
                            break;
                        }
                        case 2:
                        {
                            newX = 0;
                            newY = -1;
                            angle = 0f;
                            break;
                        }
                        case 3:
                        {
                            newX = 0;
                            newY = 1;
                            angle = 180f;
                            break;
                        }
                        default:
                        {
                            newX = 0;
                            newY = 0;
                            break;
                        }
                    }
                }
                while (maze[(int)mazePos.Z + newY, (int)mazePos.X + newX]);

                targetPos = new Vector3(mazePos.X + newX, 0, mazePos.Z + newY);
                mazePos = targetPos;
                targetPos.X *= (WALL_LENGTH * scale);
                targetPos.Y *= (WALL_LENGTH * scale);
                targetPos.Z *= (WALL_LENGTH * scale);

                canMove = false;
            }
            else
            {
                // Move to the new position
                if (Vector3.Distance(pos, targetPos) > 0.1)
                {
                    pos.X += PhysicsUtil.smoothChange(pos.X, targetPos.X, MOVE_SPEED);
                    pos.Z += PhysicsUtil.smoothChange(pos.Z, targetPos.Z, MOVE_SPEED);
                }
                else
                {
                    // Wait to move again
                    if (wait > 0)
                    {
                        wait--;
                    }
                    else
                    {
                        wait = MOVE_WAIT;
                        canMove = true;
                    }
                }
            }

            
        }

        public override void draw(SpriteBatch sb, Effect effect)
        {
            // Copy any parent transforms.
            Matrix worldMatrix = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(pos);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in model.Meshes)
            {
                // This is where the mesh orientation is set, as well as our camera and projection.
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * worldMatrix);
                    effect.Parameters["ModelTexture"].SetValue(texture);


                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * worldMatrix));
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
        public Vector3 getPosition()
        {
            return pos;
        }
    }
}
