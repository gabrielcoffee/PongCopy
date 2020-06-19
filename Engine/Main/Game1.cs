using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Engine.Entities;

namespace Pong {

    public class Game1 : Game {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public EntityManager entityManager;
        public static SpriteFont defaultFont;
        public static SoundEffect pongSound;

        public static int screenWidth  = 600;
        public static int screenHeight = 400;

        public enum GameState { Playing, Paused };
        public static GameState gameState = GameState.Playing;

        /// =============================== ///
        /// P R O J E C T   S E T T I N G S
        /// =============================== ///
        public Game1() {
            // Get device manager and set content directory
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Graphics settings
            graphics.HardwareModeSwitch = false;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;

            IsMouseVisible = false;
        }

        /// =================== ///
        /// I N I T I A L I Z E
        /// =================== ///
        protected override void Initialize() {
            // Initialize variables
            entityManager = new EntityManager(GraphicsDevice);

            
            base.Initialize();
        }

        /// ======================= ///
        /// L O A D   C O N T E N T
        /// ======================= ///
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>("font");
            pongSound = Content.Load<SoundEffect>("pongSound");

        }
        protected override void UnloadContent() {
        }

        /// =========== ///
        /// U P D A T E
        /// =========== ///
        protected override void Update(GameTime gameTime) {
            // Get input
            KeyboardState keyboard = Keyboard.GetState();

            // Exit game
            if (keyboard.IsKeyDown(Keys.Escape)) { Exit(); }

            // Pause and unpause the game
            if (Keyboard.HasBeenPressed(Keys.P)) {
                if (gameState == GameState.Paused) { gameState = GameState.Playing; }
                else
                if (gameState == GameState.Playing) { gameState = GameState.Paused; }
            }

            if (gameState == GameState.Playing)
                entityManager.Update();

            base.Update(gameTime);
        }

        /// ==== ///
        /// DRAW
        /// ==== ///
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            entityManager.Draw(spriteBatch);

            entityManager.DrawUI(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        } 
    }
}
