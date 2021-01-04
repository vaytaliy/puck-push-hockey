using Hockey.Entities;
using Hockey.Menu;
using Hockey.Multiplayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Hockey
{
    public class Game1 : Game
    {
        Texture2D ballTexture;
        Texture2D gatesTexture;
        Vector2 ballPosition;
        private SpriteFont inputFont;
        private SpriteFont scoreFont;
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardManager anyKeyManager;
        PlayerStriker playerOne;
        Dictionary<string, Keys> playerOneBindings;
        List<PlayerStriker> playerStrikers;
        Dictionary<string, Keys> playerTwoBindings;
        PlayerStriker playerTwo;
        Puck puck;
        GameManager gameManager;
        Startup multiplayerStartup;

        MenuButton playButton;
        RoomInput roomInput;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //ballSpeed = 100f;
            anyKeyManager = new KeyboardManager(Keyboard.GetState());
            playerOneBindings = new Dictionary<string, Keys>() {
                { "left", Keys.A },
                { "right", Keys.D },
                { "down", Keys.S },
                { "up", Keys.W },
            };

            playerTwoBindings = new Dictionary<string, Keys>(){
                { "left", Keys.H },
                { "right", Keys.K },
                { "down", Keys.J },
                { "up", Keys.U },
            };

            base.Initialize();

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 450;
            ballPosition = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            graphics.ApplyChanges();

            playerOne = new PlayerStriker(0 + ballTexture.Height * 2, graphics.PreferredBackBufferHeight / 2, 10f, ballTexture.Height, graphics, playerOneBindings);
            playerTwo = new PlayerStriker(graphics.PreferredBackBufferWidth - ballTexture.Height * 2, graphics.PreferredBackBufferHeight / 2, 10f, ballTexture.Height, graphics, playerTwoBindings);

            playerStrikers = new List<PlayerStriker> { playerOne, playerTwo };

            puck = new Puck(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2, graphics, ballTexture.Height, gatesTexture.Bounds);

            gameManager = new GameManager(puck, playerStrikers, graphics);
            puck.gameManager = gameManager;
            multiplayerStartup = new Startup(gameManager);
            multiplayerStartup.Initialize();
        }

        protected override void LoadContent()
        {
            ballTexture = Content.Load<Texture2D>("ball");
            gatesTexture = Content.Load<Texture2D>("gates");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            inputFont = Content.Load<SpriteFont>("MainFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            // TODO: use this.Content to load your game content here
            playButton = new MenuButton(Content.Load<Texture2D>("LocalGame"), graphics.GraphicsDevice);
            playButton.setPosition(new Vector2(350, 300));
            roomInput = new RoomInput(Content.Load<Texture2D>("RoomNumber"), graphics.GraphicsDevice);
            roomInput.setPosition(new Vector2(350, 400));
        }

        protected override void Update(GameTime gameTime)
        {

            MouseState mouse = Mouse.GetState();
            if (gameManager.inputLock == false)
            {
                KeyboardManager.Update();
                puck.HandleCollision(playerStrikers, gameTime);// updates state for any keyboard manager
            }
           
            if (gameManager.currentState == GameManager.GameState.MainMenu)
            {
                if (playButton.isClicked == true) gameManager.currentState = GameManager.GameState.Playing;
                playButton.Update(mouse);
                roomInput.Update(mouse);
            }
            else if (gameManager.currentState == GameManager.GameState.Options)
            {

            }
            else if (gameManager.currentState == GameManager.GameState.Playing)
            {
                playButton.isClicked = false;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            playerOne.HandlePlayer(kstate, gameTime, puck);
            playerTwo.HandlePlayer(kstate, gameTime, puck);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (gameManager.currentState == GameManager.GameState.MainMenu)
            {
                var kstate = Keyboard.GetState();
                var newInput = roomInput.HandleInput(kstate);
                var currentInput = roomInput.userInput;
                roomInput.userInput = anyKeyManager.TryUpdateInputText(currentInput, newInput, 10, roomInput);

                _spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"),
                    new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                playButton.Draw(_spriteBatch);
                roomInput.Draw(_spriteBatch);
                _spriteBatch.DrawString(inputFont, roomInput.userInput, new Vector2(360, 410), Color.Black);
                _spriteBatch.End();
            }
            else if (gameManager.currentState == GameManager.GameState.Options)
            {

            }
            else if (gameManager.currentState == GameManager.GameState.Playing)
            {
                _spriteBatch.DrawString(
                scoreFont,
                gameManager.scores[GameManager.Teams.Left].ToString(),
                new Vector2(200,
                graphics.PreferredBackBufferHeight / 2 - 160),
                Color.LightGray);

                _spriteBatch.DrawString(
                scoreFont,
                gameManager.scores[GameManager.Teams.Right].ToString(),
                new Vector2(432,
                graphics.PreferredBackBufferHeight / 2 - 160),
                Color.LightGray);

                _spriteBatch.Draw(
                gatesTexture,
                new Vector2(0, graphics.PreferredBackBufferHeight / 2),
                null,
                Color.Red,
                0f,
                new Vector2(gatesTexture.Width / 2, gatesTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);

                _spriteBatch.Draw(
                gatesTexture,
                new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight / 2),
                null,
                Color.Blue,
                0f,
                new Vector2(gatesTexture.Width / 2, gatesTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);


                _spriteBatch.Draw(
                ballTexture,
                playerOne.position,
                null,
                Color.Red,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);

                _spriteBatch.Draw(
                ballTexture,
                playerTwo.position,
                null,
                Color.Blue,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);

                _spriteBatch.Draw(
                ballTexture,
                puck.position,
                null,
                Color.Black,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
