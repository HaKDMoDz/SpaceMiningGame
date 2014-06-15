#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame.Screens
{
	public class ScreenManager : DrawableGameComponent
	{
		#region Fields

		private Texture2D blankTexture;
		private InputState input = new InputState();
		private bool isInitialized;
		private List<GameScreen> screens = new List<GameScreen>();
		private SpriteBatch spriteBatch;
		private List<GameScreen> tempScreensList = new List<GameScreen>();

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets a blank texture that can be used by the screens.
		/// </summary>
		public Texture2D BlankTexture
		{
			get { return blankTexture; }
		}

		/// <summary>
		/// A default SpriteBatch shared by all the screens. This saves each screen having to bother
		/// creating their own local instance.
		/// </summary>
		public SpriteBatch SpriteBatch
		{
			get { return spriteBatch; }
		}

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Constructs a new screen manager component.
		/// </summary>
		public ScreenManager(Game game)
			: base(game)
		{
		}

		#endregion Constructor

		#region Deconstructor

		~ScreenManager()
		{
		}

		#endregion Deconstructor

		#region Methods

		/// <summary>
		/// Adds a new screen to the screen manager.
		/// </summary>
		public void AddScreen(GameScreen screen)
		{
			screen.ScreenManager = this;
			screen.IsExiting = false;

			// If we have a graphics device, tell the screen to load content.
			if (isInitialized)
			{
				screen.Load();
			}

			screens.Add(screen);
		}

		/// <summary>
		/// Tells each screen to draw itself.
		/// </summary>
		public override void Draw(GameTime gameTime)
		{
			foreach (GameScreen screen in screens)
			{
				if (screen.ScreenState == ScreenState.Hidden)
					continue;

				screen.Draw(gameTime);
			}
		}

		/// <summary>
		/// Helper draws a translucent black fullscreen sprite, used for fading screens in and out,
		/// and for darkening the background behind popups.
		/// </summary>
		public void FadeBackBufferToBlack(float alpha)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(blankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
			spriteBatch.End();
		}

		/// <summary>
		/// Expose an array holding all the screens. We return a copy rather than the real master
		/// list, because screens should only ever be added or removed using the AddScreen and
		/// RemoveScreen methods.
		/// </summary>
		public GameScreen[] GetScreens()
		{
			return screens.ToArray();
		}

		/// <summary>
		/// Initializes the screen manager component.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			isInitialized = true;
		}

		/// <summary>
		/// Removes a screen from the screen manager. You should normally use GameScreen.ExitScreen
		/// instead of calling this directly, so the screen can gradually transition off rather than
		/// just being instantly removed.
		/// </summary>
		public void RemoveScreen(GameScreen screen)
		{
			// If we have a graphics device, tell the screen to unload content.
			if (isInitialized)
			{
				screen.Unload();
			}

			screens.Remove(screen);
			tempScreensList.Remove(screen);
		}

		/// <summary>
		/// Allows each screen to run logic.
		/// </summary>
		public override void Update(GameTime gameTime)
		{
			// Update the keyboard and mouse states.
			input.Update();

			// Make a copy of the master screen list, to avoid confusion if the process of updating
			// one screen adds or removes others.
			tempScreensList.Clear();

			foreach (GameScreen screen in screens)
				tempScreensList.Add(screen);

			bool otherScreenHasFocus = !Game.IsActive;
			bool coveredByOtherScreen = false;

			// Loop as long as there are screens waiting to be updated.
			while (tempScreensList.Count > 0)
			{
				// Pop the topmost screen off the waiting list.
				GameScreen screen = tempScreensList[tempScreensList.Count - 1];

				tempScreensList.RemoveAt(tempScreensList.Count - 1);

				// Update the screen.
				screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

				if (screen.ScreenState == ScreenState.TransitionOn ||
					screen.ScreenState == ScreenState.Active)
				{
					// If this is the first active screen we came across, give it a chance to handle
					// input.
					if (!otherScreenHasFocus)
					{
						screen.HandleInput(gameTime, input);

						otherScreenHasFocus = true;
					}

					// If this is an active non-popup, inform any subsequent screens that they are
					// covered by it.
					if (!screen.IsPopup)
						coveredByOtherScreen = true;
				}
			}
		}

		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected override void LoadContent()
		{
			// Load content belonging to the screen manager.
			ContentManager content = Game.Content;

			spriteBatch = new SpriteBatch(GraphicsDevice);

			blankTexture = new Texture2D(GraphicsDevice, 1, 1);
			blankTexture.SetData<Color>(new Color[] { Color.White });

			// Tell each of the screens to load their content.
			foreach (GameScreen screen in screens)
			{
				screen.Load();
			}
		}

		/// <summary>
		/// Unload your graphics content.
		/// </summary>
		protected override void UnloadContent()
		{
			// Tell each of the screens to unload their content.
			foreach (GameScreen screen in screens)
			{
				screen.Unload();
			}
			blankTexture.Dispose();
		}

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}