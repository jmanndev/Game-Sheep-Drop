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

namespace WindowsGame2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rand = new Random();


        int windowHeight = 600;
        int windowWidth = 800;

        float windSpeed = 5.0f;
        Color lastRandom = Color.White;

        Texture2D sheepTexture;
        Color sheepColor = Color.White;
        Vector2 sheepPosition = new Vector2(0, 30);
        Rectangle sheepRectangle;
        int sheepWidth = 70;
        int sheepHeight = 70;
        float sheepHorSpeed = 5.0f;
        float sheepVerSpeed = 0;
        int score = 0;



        Texture2D faceTexture;
        Vector2 facePosition = new Vector2(50,50);
        Rectangle faceRectangle;
        Rectangle faceBoundingBox;
        int faceWidth = 100;
        int faceHeight = 100;

        EGameState gameState = EGameState.GAME;

        SpriteFont font;

        Boolean showRules = true;

        enum EGameState
        {
            GAME,
            WIN
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            facePosition.Y = windowHeight - faceHeight - 20;
            facePosition.X = (windowWidth / 2) - (faceWidth / 2);
            sheepRectangle = new Rectangle((int)sheepPosition.X, (int)sheepPosition.Y, sheepWidth, faceHeight);
            faceRectangle = new Rectangle((int)facePosition.X, (int)facePosition.Y, faceWidth, faceHeight);
            faceBoundingBox = new Rectangle((int)facePosition.X + faceWidth / 4, (int)facePosition.Y + faceHeight / 4, faceWidth / 2, faceHeight / 2);
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
            
            faceTexture = Content.Load<Texture2D>("face");
            sheepTexture = Content.Load<Texture2D>("sheep");

            font = Content.Load<SpriteFont>("font");

            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case EGameState.GAME: 
                    UpdateGame();
                    break;
                case EGameState.WIN:
                    UpdateWin();
                    break;
            }
            
            base.Update(gameTime);
        }

        void UpdateWin()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                gameState = EGameState.GAME;
                showRules = true;
                SheepReset();
                RandomFaceX();
                score = 0;
            }
        }

        void UpdateGame()
        {
            UpdateRectangle();
            SheepMove();
            UpdateControls();
            EdgeCheck();
            CollisionCheck();
        }

        void SheepReset()
        {
            sheepPosition.X = 0 - sheepWidth;
            sheepPosition.Y = 30;
            sheepHorSpeed = 5.0f;
            sheepVerSpeed = 0;
            ChangeColor();
        }

        void ChangeColor()
        {
            while (sheepColor == lastRandom)
            {
                sheepColor = RandomColor(rand.Next(7));
            }
            lastRandom = sheepColor;
        }

        void RandomFaceX()
        {
            int r = 0;
            while (r <= faceWidth * 2) {
                r = rand.Next(windowWidth - faceWidth);
            }
            facePosition.X = r;
        }

        void UpdateControls()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                showRules = false;
                sheepHorSpeed = 1.5f;
                sheepVerSpeed = 5.0f;
            }
        }

        void UpdateRectangle()
        {
            sheepRectangle = new Rectangle((int)sheepPosition.X, (int)sheepPosition.Y, sheepWidth, sheepHeight);
            faceRectangle = new Rectangle((int)facePosition.X, (int)facePosition.Y, faceWidth, faceHeight);
            faceBoundingBox = new Rectangle((int)facePosition.X + faceWidth / 4, (int)facePosition.Y + faceHeight / 4, faceWidth / 2, faceHeight / 2);
        }

        void CollisionCheck()
        {
            if (sheepRectangle.Intersects(faceBoundingBox))
            {
                score++;
                if (score == 5)
                {
                    gameState = EGameState.WIN;
                }
                else
                {
                    SheepReset();
                    RandomFaceX();
                }
            }
        }

        Color RandomColor(int colorNum)
        {
            switch (colorNum)
            {
                case 0:
                    return Color.Blue;
                case 1:
                    return Color.Red;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Gold;
                case 4:
                    return Color.Fuchsia;
                case 5:
                    return Color.Pink;
                case 6:
                    return Color.White;
                default:
                    return Color.White;
            }

        }

        void SheepMove()
        {
            sheepPosition.X += sheepHorSpeed;
            sheepPosition.Y += sheepVerSpeed;
        }

        void EdgeCheck()
        {
            if (sheepPosition.X > graphics.PreferredBackBufferWidth)
            {
                SheepReset();
            }

            if (sheepPosition.Y > graphics.PreferredBackBufferHeight)
            {
                score = 0;
                SheepReset();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);
            spriteBatch.Begin();
            // TODO: Add your drawing code here

            switch (gameState)
            {
                case EGameState.GAME:
                    DrawGame();
                    break;
                case EGameState.WIN:
                    DrawGame();
                    DrawWin();
                    break;
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void DrawGame()
        {
            spriteBatch.DrawString(font, "SCORE: " + score, new Vector2(windowWidth - 200, 0), Color.White);

            if (gameState == EGameState.GAME && showRules == true)
            {
                spriteBatch.DrawString(font, "CLICK SPACE TO DROP THE SHEEP", new Vector2(50, windowHeight / 3), Color.White);
                spriteBatch.DrawString(font, "LAND ON SAM 5 TIMES TO WIN", new Vector2(70, windowHeight / 2), Color.White);
            
            }

            spriteBatch.Draw(faceTexture, faceRectangle, Color.White);
            spriteBatch.Draw(sheepTexture, sheepRectangle, sheepColor);
        }

        void DrawWin()
        {
            spriteBatch.DrawString(font, "YOU WIN", new Vector2(windowWidth / 2 - 100, windowHeight / 3), Color.White);
            spriteBatch.DrawString(font, "CLICK ENTER TO RESTART", new Vector2(100, windowHeight / 2), Color.White); 
        }
    }
}
