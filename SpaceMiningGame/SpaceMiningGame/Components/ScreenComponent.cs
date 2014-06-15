#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceMiningGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame.Components
{
	public abstract class ScreenComponent
	{
		#region Fields

		private GameScreen screen;

		#endregion Fields

		#region Properties

		public GameScreen Screen
		{
			get { return screen; }
		}

		#endregion Properties

		#region Constructor

		public ScreenComponent(GameScreen screen)
		{
			this.screen = screen;
		}

		#endregion Constructor

		#region Deconstructor

		~ScreenComponent()
		{
		}

		#endregion Deconstructor

		#region Methods

		/// <summary>
		/// Draws the component, is called by the screen only if the screen itself is not in hidden
		/// state.
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Draw(GameTime gameTime)
		{
		}

		/// <summary>
		/// Makes the component parse user input, this is called by the screen
		/// </summary>
		/// <param name="gameTime">  </param>
		/// <param name="inputState"></param>
		public virtual void HandleInput(GameTime gameTime, InputState inputState)
		{
		}

		public virtual void Unload()
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}