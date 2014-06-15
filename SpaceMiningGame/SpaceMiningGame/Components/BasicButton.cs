#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

		private Texture2D texture;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets or sets the position and size of the button
		/// </summary>
		public Rectangle bounds;

		/// <summary>
		/// Gets or sets the texture the button displays to the user
		/// </summary>
		public Texture2D Texture
		{
			get { return texture; }
			set { texture = value; }
		}

		#endregion Properties

		#region Constructor

		public BasicButton(GameScreen screen, Rectangle bounds)

			: base(screen)
		{
			this.bounds = bounds;
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
			base.Draw(gameTime);

			//Get the spritebatch and draw the image of the button
			SpriteBatch batch = Screen.SpriteBatch;
			batch.Begin();
			{
				batch.Draw(Texture, bounds, Color.White);
			}
			batch.End();
		}

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}