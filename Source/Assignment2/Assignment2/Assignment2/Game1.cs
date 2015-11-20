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
using Assignment2.Entities;

namespace Assignment2
{
    /// <summary>
    /// This is the main scene of the game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 instance;

        public static int SCREEN_WIDTH = 1366;
        public static int SCREEN_HEIGHT = 768;

        public static int BRICK_WIDTH = 128;
        public static int BRICK_HEIGHT = 48;
        public static int NUM_BRICK_W = 8;
        public static int NUM_BRICK_H = 3;
        public static int BRICK_START_X = (SCREEN_WIDTH - (NUM_BRICK_W * 128)) / 2;
        public static int BRICK_START_Y = 128;

        public static int BRICK_SCORE = 100;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Entities.Console console;
        Paddle paddle;
        Ball ball;
        HUD hud;

        private AudioUtils audioUtils;

        private int combo = 1;

        private Color bgColor = Color.CornflowerBlue;
        private List<EntityCollide> collisionList = new List<EntityCollide>();
        private List<Brick> bricks = new List<Brick>();

        private SaveUtils save;

        /// <summary>
        /// The constructor for the game.
        /// </summary>
        public Game1()
        {
            instance = this;
            audioUtils = AudioUtils.getInstance();

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set default window properties
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            save = SaveUtils.getInstance();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            SpriteFont consoleFont = Content.Load<SpriteFont>("fontConsole");
            console = new Entities.Console(consoleFont, graphics);

            paddle = new Paddle(Content);
            collisionList.Add(paddle);

            ball = new Ball(Content);
            collisionList.Add(ball);

            // Generate the bricks
            for (int i = 0; i < NUM_BRICK_H; i++)
            {
                for (int j = 0; j < NUM_BRICK_W; j++)
                {
                    Brick b = new Brick(Content, new Vector2(BRICK_START_X + j * BRICK_WIDTH, BRICK_START_Y + i * BRICK_HEIGHT), (BrickTypes) i);
                    bricks.Add(b);
                    collisionList.Add(b);
                }
            }

            // Create the HUD
            hud = new HUD(Content);

            // Load the audio
            audioUtils.loadContent(Content);
                       

            // Initialize the high score list
#if XBOX360
            if (!save.storageRegistered())
                save.RegisterStorage();
#endif

            if (!save.CheckHighScoreExists())
            {
                List<int> highscores = HighScoreUtils.createInitialHighScores();
                save.saveHighScores(highscores);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Resetting
            if (!console.isOpen() && (gamepad.IsButtonDown(Buttons.Y) || keyboard.IsKeyDown(Keys.R)))
            {
                MediaPlayer.Stop();
                reset();
            }
            
            console.update();
            hud.update(gamepad, keyboard, mouse);

            if (!hud.getPausedState())
            {
                paddle.update(gamepad, keyboard, mouse);
                ball.update(gamepad, keyboard, mouse);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);

            paddle.draw(spriteBatch);
            ball.draw(spriteBatch);

            foreach (Brick b in bricks)
            {
                b.draw(spriteBatch);
            }


            hud.draw(spriteBatch);
            console.draw(spriteBatch);

            base.Draw(gameTime);
        }

        /// <summary>
        /// This function changes the background colour of the game.
        /// </summary>
        /// <param name="c">The colour to set</param>
        public void setBackgroundColor(Color c)
        {
            this.bgColor = c;
        }

        /// <summary>
        /// This returns a list of all collidable entities in the game.
        /// </summary>
        /// <returns>The list of collidable entities.</returns>
        public List<EntityCollide> getCollisionList()
        {
            return collisionList;
        }

        /// <summary>
        /// This function removes a block from the game and adds score.
        /// </summary>
        /// <param name="b">The brick to remove</param>
        public void removeBrick(Brick b)
        {
            hud.addScore(b.getPoints() * combo);

            bricks.Remove(b);
            collisionList.Remove(b);

            combo++;

            if (bricks.Count == 0)
            {
                gameWin();
            }
        }
        
        /// <summary>
        /// This function resets the current combo.
        /// </summary>
        public void resetCombo()
        {
            combo = 1;
        }

        /// <summary>
        /// This function returns the current combo.
        /// </summary>
        /// <returns>The current combo</returns>
        public int getCombo()
        {
            return combo;
        }

        /// <summary>
        /// This function sets the game in the win state.
        /// </summary>
        public void gameWin()
        {
            ball.stop();

            MediaPlayer.Stop();

            hud.gameWin();

            // Update the high scores
            HighscoreData data = save.loadHighScores();
            List<int> scores = data.highscores;
            HighScoreUtils.updateHighScores(scores, hud.getScore());

            save.saveHighScores(scores);
        }

        /// <summary>
        /// This function sets the game in the lose state.
        /// </summary>
        public void gameOver()
        {
            ball.stop();

            hud.gameOver();
        }

        /// <summary>
        /// This function resets the game.
        /// </summary>
        public void reset()
        {
            hud.reset();

            combo = 1;

            // Remake the ball
            paddle = new Paddle(Content);
            ball = new Ball(Content);

            // Reset the bricks
            bricks.Clear();

            // Reset collisions
            collisionList.Clear();

            collisionList.Add(paddle);
            collisionList.Add(ball);

            // Generate the bricks
            for (int i = 0; i < NUM_BRICK_H; i++)
            {
                for (int j = 0; j < NUM_BRICK_W; j++)
                {
                    Brick b = new Brick(Content, new Vector2(BRICK_START_X + j * BRICK_WIDTH, BRICK_START_Y + i * BRICK_HEIGHT), (BrickTypes)i);
                    bricks.Add(b);
                    collisionList.Add(b);
                }
            }
        }
    }
}
