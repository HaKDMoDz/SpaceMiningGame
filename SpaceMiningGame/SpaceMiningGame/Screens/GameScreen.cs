#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMiningGame.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame.Screens
{
	/// <summary>
	/// A screen is a single layer that has update and draw logic, and which can be combined with
	/// other layers to build up a complex menu system. For instance the main menu, the options
	/// menu, the "are you sure you want to quit" message box, and the main game itself are all
	/// implemented as screens.
	/// </summary>
	public abstract class GameScreen
	{
		#region Fields

		private List<ScreenComponent> components;
		private bool isExiting = false;
		private bool isPopup = false;
		private bool otherScreenHasFocus;
		private ScreenManager screenManager;
		private ScreenState screenState = ScreenState.TransitionOn;
		private TimeSpan transitionOffTime = TimeSpan.Zero;
		private TimeSpan transitionOnTime = TimeSpan.Zero;
		private float transitionPosition = 1;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Checks whether this screen is active and can respond to user input.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return !otherScreenHasFocus &&
					   (screenState == ScreenState.TransitionOn ||
						screenState == ScreenState.Active);
			}
		}

		/// <summary>
		/// There are two possible reasons why a screen might be transitioning off. It could be
		/// temporarily going away to make room for another screen that is on top of it, or it could
		/// be going away for good. This property indicates whether the screen is exiting for real:
		/// if set, the screen will automatically remove itself as soon as the transition finishes.
		/// </summary>
		public bool IsExiting
		{
			get { return isExiting; }
			protected internal set { isExiting = value; }
		}

		/// <summary>
		/// Normally when one screen is brought up over the top of another, the first screen will
		/// transition off to make room for the new one. This property indicates whether the screen
		/// is only a small popup, in which case screens underneath it do not need to bother
		/// transitioning off.
		/// </summary>
		public bool IsPopup
		{
			get { return isPopup; }
			protected set { isPopup = value; }
		}

		/// <summary>
		/// Gets the manager that this screen belongs to.
		/// </summary>
		public ScreenManager ScreenManager
		{
			get { return screenManager; }
			internal set { screenManager = value; }
		}

		/// <summary>
		/// Gets the current screen transition state.
		/// </summary>
		public ScreenState ScreenState
		{
			get { return screenState; }
			protected set { screenState = value; }
		}

		/// <summary>
		/// Gets the spritebatch from the screenmanager
		/// </summary>
		public SpriteBatch SpriteBatch
		{
			get { return screenManager.SpriteBatch; }
		}

		/// <summary>
		/// Gets the current alpha of the screen transition, ranging from 1 (fully active, no
		/// transition) to 0 (transitioned fully off to nothing).
		/// </summary>
		public float TransitionAlpha
		{
			get { return 1f - TransitionPosition; }
		}

		/// <summary>
		/// Indicates how long the screen takes to transition off when it is deactivated.
		/// </summary>
		public TimeSpan TransitionOffTime
		{
			get { return transitionOffTime; }
			protected set { transitionOffTime = value; }
		}

		/// <summary>
		/// Indicates how long the screen takes to transition on when it is activated.
		/// </summary>
		public TimeSpan TransitionOnTime
		{
			get { return transitionOnTime; }
			protected set { transitionOnTime = value; }
		}

		/// <summary>
		/// Gets the current position of the screen transition, ranging from zero (fully active, no
		/// transition) to one (transitioned fully off to nothing).
		/// </summary>
		public float TransitionPosition
		{
			get { return transitionPosition; }
			protected set { transitionPosition = value; }
		}

		#endregion Properties

		#region Constructor

		public GameScreen(ScreenManager manager)
		{
			this.screenManager = manager;
			this.components = new List<ScreenComponent>();
		}

		#endregion Constructor

		#region Deconstructor

		~GameScreen()
		{
		}

		#endregion Deconstructor

		#region Methods

		public void AddComponent(ScreenComponent component)
		{
			components.Add(component);
		}

		/// <summary>
		/// This is called when the screen should draw itself.
		/// </summary>
		public virtual void Draw(GameTime gameTime)
		{
			//Only draw the components when the screen is not hidden.
			if (this.screenState != ScreenState.Hidden)
			{
				foreach (ScreenComponent component in components)
				{
					component.Draw(gameTime);
				}
			}
		}

		/// <summary>
		/// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which instantly kills
		/// the screen, this method respects the transition timings and will give the screen a
		/// chance to gradually transition off.
		/// </summary>
		public void ExitScreen()
		{
			if (TransitionOffTime == TimeSpan.Zero)
			{
				// If the screen has a zero transition time, remove it immediately.
				ScreenManager.RemoveScreen(this);
			}
			else
			{
				// Otherwise flag that it should transition off and then exit.
				isExiting = true;
			}
		}

		public ScreenComponent[] GetComponents()
		{
			return components.ToArray();
		}

		/// <summary>
		/// Allows the screen to handle user input. Unlike Update, this method is only called when
		/// the screen is active, and not when some other screen has taken the focus.
		/// </summary>
		public virtual void HandleInput(GameTime gameTime, InputState input)
		{
			if (this.IsActive)
			{
				foreach (ScreenComponent component in components)
				{
					component.HandleInput(gameTime, input);
				}
			}
		}

		/// <summary>
		/// Loads the content for the screen, called by the screen manager when adding the screen to
		/// the game
		/// </summary>
		public virtual void Load()
		{
		}

		public bool RemoveComponent(ScreenComponent component)
		{
			return components.Remove(component);
		}

		/// <summary>
		/// Unload content for the screen. Called when the screen is removed from the screen
		/// manager.
		/// </summary>
		public virtual void Unload()
		{
		}

		/// <summary>
		/// Allows the screen to run logic, such as updating the transition position. Unlike
		/// HandleInput, this method is called regardless of whether the screen is active, hidden,
		/// or in the middle of a transition.
		/// </summary>
		public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			this.otherScreenHasFocus = otherScreenHasFocus;

			if (isExiting)
			{
				// If the screen is going away to die, it should transition off.
				screenState = ScreenState.TransitionOff;

				if (!UpdateTransition(gameTime, transitionOffTime, 1))
				{
					// When the transition finishes, remove the screen.
					ScreenManager.RemoveScreen(this);
				}
			}
			else if (coveredByOtherScreen)
			{
				// If the screen is covered by another, it should transition off.
				if (UpdateTransition(gameTime, transitionOffTime, 1))
				{
					// Still busy transitioning.
					screenState = ScreenState.TransitionOff;
				}
				else
				{
					// Transition finished!
					screenState = ScreenState.Hidden;
				}
			}
			else
			{
				// Otherwise the screen should transition on and become active.
				if (UpdateTransition(gameTime, transitionOnTime, -1))
				{
					// Still busy transitioning.
					screenState = ScreenState.TransitionOn;
				}
				else
				{
					// Transition finished!
					screenState = ScreenState.Active;
				}
			}

			//Update all the components on in the screen, even if the screen is hidden, the components should update
			foreach (ScreenComponent component in components)
			{
				component.Update(gameTime);
			}
		}

		/// <summary>
		/// Helper method for getting the contentmanager of the game
		/// </summary>
		/// <returns></returns>
		protected ContentManager GetContentManager()
		{
			return new ContentManager(ScreenManager.Game.Services, "Content");
		}

		/// <summary>
		/// Helper for updating the screen transition position.
		/// </summary>
		private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
		{
			// How much should we move by?
			float transitionDelta;

			if (time == TimeSpan.Zero)
				transitionDelta = 1;
			else
				transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

			// Update the transition position.
			transitionPosition += transitionDelta * direction;

			// Did we reach the end of the transition?
			if (((direction < 0) && (transitionPosition <= 0)) ||
				((direction > 0) && (transitionPosition >= 1)))
			{
				transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
				return false;
			}

			// Otherwise we are still busy transitioning.
			return true;
		}

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}