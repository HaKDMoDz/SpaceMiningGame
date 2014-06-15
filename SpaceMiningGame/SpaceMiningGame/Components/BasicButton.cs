#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceMiningGame;
using SpaceMiningGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame.Components
{
	/// <summary>
	/// A basic button class, the button draws a single image, and has events for basic user mouse
	/// interaction
	/// </summary>
	public class BasicButton : ScreenComponent
	{
		#region Fields

		private bool hovering = false;

		#endregion Fields

		#region Properties

		#endregion Properties

		#region Constructor

		public BasicButton(GameScreen screen)
			: base(screen)
		{
			this.MouseEnter += BasicButton_MouseEnter;
			this.MouseLeave += BasicButton_MouseLeave;
		}

		#endregion Constructor

		#region Deconstructor

		~BasicButton()
		{
		}

		#endregion Deconstructor

		#region Methods

		/// <summary>
		/// Draws the button
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			if (BaseTexture != null)
			{
				SpriteBatch batch = Screen.SpriteBatch;
				batch.Begin();
				{
					Vector2 scale = new Vector2(Size.X / BaseTexture.Width, Size.Y / BaseTexture.Height);
					batch.Draw(BaseTexture, Position, null, (hovering) ? Color.LightGray : Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
				}
				batch.End();
			}

			//base.Draw(gameTime);
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			base.HandleInput(gameTime, input);
		}

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		private void BasicButton_MouseEnter(object sender, MouseEvent e)
		{
			hovering = true;
		}

		private void BasicButton_MouseLeave(object sender, MouseEvent e)
		{
			hovering = false;
		}

		#endregion Events
	}
}