using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Monogame___Making_a_Class
{
    enum Screen
    {
        Title,
        House,
        End
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<Texture2D> ghostTextures;
        Texture2D introBackground;
        Texture2D gameBackground;
        Texture2D endBackground;
        Texture2D marioTexture;
        MouseState mouseState;
        KeyboardState keyboardState;
        Random generator;
        Screen screen;
        Rectangle window;
        List<Ghost> ghosts;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();

            generator = new Random();
            ghostTextures = new List<Texture2D>();
            ghosts = new List<Ghost>();
            window = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            screen = Screen.Title;

            base.Initialize();

            for (int i = 0; i < 30; i++)
            {
                ghosts.Add(new Ghost(ghostTextures, new Rectangle(generator.Next(0, 760), generator.Next(0, 460), 40, 40)));
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ghostTextures.Add(Content.Load<Texture2D>("Images/boo-stopped"));
            for (int i = 1; i <= 8; i++)
                ghostTextures.Add(Content.Load<Texture2D>("Images/boo-move-" + i));
            introBackground = Content.Load<Texture2D>("Images/haunted-title");
            gameBackground = Content.Load<Texture2D>("Images/haunted-background");
            endBackground = Content.Load<Texture2D>("Images/haunted-end-screen");
            marioTexture = Content.Load<Texture2D>("Images/mario");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            if (screen == Screen.Title)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                    screen = Screen.House;

            }
            else if (screen == Screen.House)
            {
                for (int i = 0; i < ghosts.Count; i++)
                {
                    ghosts[i].Update(gameTime, mouseState);
                    if (ghosts[i].Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
                        screen = Screen.End;
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (screen == Screen.Title)
                _spriteBatch.Draw(introBackground, window, Color.White);
            else if (screen == Screen.House)
            {
                _spriteBatch.Draw(gameBackground, window, Color.White);
                _spriteBatch.Draw(marioTexture, mouseState.Position.ToVector2(), Color.White);
                foreach (Ghost ghost in ghosts)
                    ghost.Draw(_spriteBatch);
            }
            else
                _spriteBatch.Draw(endBackground, window, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
