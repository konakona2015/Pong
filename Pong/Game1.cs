using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game1 : Game
    {
        int screenWidth = 1024;
        int screenHeight = 576;
        int paddleLength = 100;
        int paddleThickness = 10;
        bool isPlaying = true;
        static Random rand = new Random();

        int randDir = rand.Next(0, 3);

        Texture2D paddle;
        Texture2D ball;
        Texture2D line;

        Vector2 coor = new Vector2(8, (576 / 2) - 50);
        Vector2 coor2 = new Vector2(1024 - 18, (576 / 2) - 50);
        Vector2 ballPos = new Vector2((1024 / 2) - 5, (576 / 2) - 5);
        Vector2 ballVelocity = new Vector2(0, 0);

        float paddleSpeed = 700f;
        float ballSpeed = 400f;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();

            paddle = new Texture2D(_graphics.GraphicsDevice, paddleThickness, paddleLength);

            Color[] data = new Color[paddleThickness * paddleLength];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            paddle.SetData(data);

            ball = new Texture2D(_graphics.GraphicsDevice, 10, 10);

            Color[] datab = new Color[10 * 10];
            for (int i = 0; i < datab.Length; ++i) datab[i] = Color.White;
            ball.SetData(datab);

            line = new Texture2D(_graphics.GraphicsDevice, 2, 576);

            Color[] dataL = new Color[2 * 576];
            for (int i = 0; i < dataL.Length; ++i) dataL[i] = Color.White;
            line.SetData(dataL);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            ballPos += ballVelocity;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            // Paddle Player 1
            if (kstate.IsKeyDown(Keys.W))
                coor.Y -= paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.S))
                coor.Y += paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Paddle Player 2
            if (coor.Y > screenHeight - paddle.Height)
                coor.Y = screenHeight - paddle.Height;
            else if (coor.Y < 0)
                coor.Y = 0;

            if (kstate.IsKeyDown(Keys.Up))
                coor2.Y -= paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Down))
                coor2.Y += paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (coor2.Y > screenHeight - paddle.Height)
                coor2.Y = screenHeight - paddle.Height;
            else if (coor2.Y < 0)
                coor2.Y = 0;

            // Start Game
            if (kstate.IsKeyDown(Keys.Space) && isPlaying)
            { 

                ballVelocity.Y = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                ballVelocity.X = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                isPlaying = false;
            }

            // Ball

            // Ball goes off screen
            if (ballPos.X < 0 || ballPos.X > screenWidth)
                isPlaying = true;

            // Resets the ball to center
            if (isPlaying == true)
            {
                ballPos.X = (1024 / 2) - 5;
                ballPos.Y = (576 / 2) - 5;
            }

            // Bottom boundary check
            if (ballPos.Y > screenHeight - ball.Height)
            {
                ballVelocity.Y = -ballVelocity.Y;
            }
            // Top boundary check
            else if (ballPos.Y < 0)
            {
                ballVelocity.Y = -ballVelocity.Y;
            }

            if (ballPos.X < coor.X + paddleThickness && ballPos.Y > coor.Y && ballPos.Y < coor.Y + paddleLength)
            {
                ballVelocity.X = -ballVelocity.X;
            }

            if (ballPos.X > coor2.X - paddleThickness && ballPos.Y > coor2.Y && ballPos.Y < coor2.Y + paddleLength)
            {
                ballVelocity.X = -ballVelocity.X;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(paddle, coor, Color.White);
            _spriteBatch.Draw(paddle, coor2, Color.White);
            _spriteBatch.Draw(ball, ballPos, Color.White);
            _spriteBatch.Draw(line, new Vector2((1024 / 2) - 1, 0), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
