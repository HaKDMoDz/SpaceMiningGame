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
	public class LoadingScreen : GameScreen
	{
		#region Fields

		private bool loadingIsSlow;
		private bool otherScreensAreGone;
		private GameScreen[] screensToLoad;

		#endregion Fields

		#region Properties

		#endregion Properties

		#region Constructor

		public LoadingScreen(ScreenManager manager, bool loadingIsSlow, params GameScreen[] screensToLoad)
			: base(manager)
		{
			this.loadingIsSlow = loadingIsSlow;
			this.screensToLoad = screensToLoad;
			this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
		}

		#endregion Constructor

		#region Deconstructor

		~LoadingScreen()
		{
		}

		#endregion Deconstructor

		#region Methods

		/// <summary>
		/// Draws the loading screen.
		/// </summary>
		public override void Draw(GameTime gameTime)
		{
			// If we are the only active screen, that means all the previous screens must have
			// finished transitioning off. We check for this in the Draw method, rather than in
			// Update, because it isn't enough just for the screens to be gone: in order for the
			// transition to look good we must have actually drawn a frame without them before we
			// perform the load.
			if ((ScreenState == ScreenState.Active) &&
				(ScreenManager.GetScreens().Length == 1))
			{
				otherScreensAreGone = true;
			}
		}

		public override void Load()
		{
			base.Load();
		}

		/// <summary>
		/// Updates the loading screen.
		/// </summary>
		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			// If all the previous screens have finished transitioning off, it is time to actually
			// perform the load.
			if (otherScreensAreGone)
			{
				//Remove the screen from the screen manager, this will call the unload()
				//method on this class, then return to the current method and then we call
				//the loadscreens() method to load the screens we are supposed to load
				ScreenManager.RemoveScreen(this);

				LoadScreens(screensToLoad);

				// Once the load has finished, we use ResetElapsedTime to tell the game timing
				// mechanism that we have just finished a very long frame, and that it should not
				// try to catch up.
				ScreenManager.Game.ResetElapsedTime();
			}
		}

		/// <summary>
		/// Override-able method that loads the screens. This is in a seperate method in case we
		/// make overrides of this class
		/// </summary>
		/// <param name="loadingQueue">The screens to load at this point in time</param>
		protected virtual void LoadScreens(GameScreen[] loadingQueue)
		{
			foreach (GameScreen screen in loadingQueue)
			{
				if (screen != null)
				{
					ScreenManager.AddScreen(screen);
				}
			}
		}

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}