namespace Front.Client.Core
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.PortableExecutable;
    using System.Runtime.Intrinsics.Arm;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Content;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Xna.Framework.Media;
    using System.Xml.Linq;
    using RenderingLibrary;
    using MonoGameGum.GueDeriving;
    using Gum.DataTypes;
    using Gum.Managers;
    using GumRuntime;

    public class SampleGame : Game
    {
        private KeyboardState keyboardPrev = new KeyboardState();
        private MouseState mousePrev = new MouseState();
        private GamePadState gpPrev = new GamePadState();

        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;

        public SampleGame()
        {
            // This gets assigned to something internally, don't worry...
            this.graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Typically you would load a config here...
            this.graphicsDeviceManager.PreferredBackBufferWidth = 1280;
            this.graphicsDeviceManager.PreferredBackBufferHeight = 720;
            this.graphicsDeviceManager.IsFullScreen = false;
            this.graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            /* This is a nice place to start up the engine, after
             * loading configuration stuff in the constructor
             */

            base.Initialize();

            SystemManagers.Default = new SystemManagers();
            SystemManagers.Default.Initialize(this.graphicsDeviceManager.GraphicsDevice, fullInstantiation: true);

            // ToolsUtilities.FileManager.RelativeDirectory += "Gum\\";
            ToolsUtilities.FileManager.RelativeDirectory = "Content\\Gum\\";

            var gumProject = GumProjectSave.Load("SampleUI.gumx");
            ObjectFinder.Self.GumProjectSave = gumProject;
            gumProject.Initialize();

            var screen = gumProject.Screens.First().ToGraphicalUiElement(SystemManagers.Default, addToManagers: false);
            screen.AddToManagers(SystemManagers.Default, layer: null);
        }

        protected override void LoadContent()
        {
            // Load textures, sounds, and so on in here...
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // Clean up after yourself!
            
            // Dispose of loaded songs.

            base.UnloadContent();
        }

        // Run game logic in here. Do NOT render anything here!
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Poll input
            KeyboardState keyboardCur = Keyboard.GetState();
            MouseState mouseCur = Mouse.GetState();
            GamePadState gpCur = GamePad.GetState(PlayerIndex.One);

            // Current is now previous!
            keyboardPrev = keyboardCur;
            mousePrev = mouseCur;
            gpPrev = gpCur;

            SystemManagers.Default.Activity(gameTime.TotalGameTime.TotalSeconds);

            // TODO: Is a correct way to update base at the end or at the start?
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Render stuff in here. Do NOT run game logic in here!
            GraphicsDevice.Clear(Color.Black);

            SystemManagers.Default.Draw();

            // TODO: Is a correct way to draw base at the end or at the start?
            base.Draw(gameTime);
        }
    }
}
