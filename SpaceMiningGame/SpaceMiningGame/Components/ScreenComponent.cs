#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

		private Texture2D baseTexture;

		/// <summary>
		/// Indicates whether the left mousebutton is being tracked on being a propper button click.
		/// This value becomes true when the user starts holding down the left mouse button while
		/// hovering over the component. And becomes false when the button is released. If the
		/// button is released while the mouse is hovering over the component, the user made a
		/// propper left mouse click.
		/// </summary>
		private bool cp_left = false;

		/// <summary>
		/// Indicates whether the middle mousebutton is being tracked on being a propper button
		/// click. This value becomes true when the user starts holding down the middle mouse button
		/// while hovering over the component. And becomes false when the button is released. If the
		/// button is released while the mouse is hovering over the component, the user made a
		/// propper middle mouse click.
		/// </summary>
		private bool cp_middle = false;

		/// <summary>
		/// Indicates whether the right mousebutton is being tracked on being a propper button
		/// click. This value becomes true when the user starts holding down the right mouse button
		/// while hovering over the component. And becomes false when the button is released. If the
		/// button is released while the mouse is hovering over the component, the user made a
		/// propper right mouse click.
		/// </summary>
		private bool cp_right = false;

		/// <summary>
		/// Indicates whether the mouse was over the component during the last HandleInput() call.
		/// </summary>
		private bool previousHover;

		private GameScreen screen;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets or sets the position of the screen component
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Gets or sets the size of the component (in pixels)
		/// </summary>
		public Vector2 Size;

		/// <summary>
		/// Gets or sets the base texture of the component
		/// </summary>
		public Texture2D BaseTexture
		{
			get { return baseTexture; }
			set { baseTexture = value; }
		}

		/// <summary>
		/// Gets the bounds of the component. Warning: this converts the Vector2 float values to
		/// integers!
		/// </summary>
		public Rectangle Bounds
		{
			get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); }
		}

		public GameScreen Screen
		{
			get { return screen; }
		}

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Creates
		/// </summary>
		/// <param name="screen"></param>
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
			if (baseTexture != null)
			{
				SpriteBatch batch = screen.SpriteBatch;
				batch.Begin();
				{
					Vector2 scale = new Vector2(Size.X / baseTexture.Width, Size.Y / baseTexture.Height);
					batch.Draw(baseTexture, Position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
				}
				batch.End();
			}
		}

		/// <summary>
		/// Makes the component parse user input, this is called by the screen. When overriding this
		/// function, make sure you call it in order for events to work
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="input">   </param>
		public virtual void HandleInput(GameTime gameTime, InputState input)
		{
			//Get local version, for cleaner code
			MouseState ms = input.CurrentMouseState;

			//Determine mouse hovering status
			if (IsMouseHovering(ms))
			{
				if (!previousHover)
				{
					onMouseEnter(new MouseEvent(this, input));
				}
				onMouseHover(new MouseEvent(this, input));

				//To prevent calling the MouseDown and MouseUp events multiple times,
				//we use these flags and call it after checking all cases
				bool invokeMouseDown = false;
				bool invokeMouseUp = false;

				//Left button events
				if (ms.LeftButton == ButtonState.Pressed)
				{
					invokeMouseDown = true;
					cp_left = true;
				}
				else
				{
					invokeMouseUp = false;

					//If the button was being held down while hovering over the component,
					//and its released while over the component, its a propper click. This
					//allows the user to move the mouse away from the component to cancel a
					//full click event.
					if (cp_left)
					{
						onMouseClick(new MouseClickEvent(this, input, MouseButton.LeftMouseButton));
					}
					cp_left = false;
				}

				//Right button events
				if (ms.RightButton == ButtonState.Pressed)
				{
					invokeMouseDown = true;
					cp_right = true;
				}
				else
				{
					invokeMouseUp = false;
					if (cp_right)
					{
						onMouseClick(new MouseClickEvent(this, input, MouseButton.RightMouseButton));
					}
					cp_right = false;
				}

				//Middle button events
				if (ms.MiddleButton == ButtonState.Pressed)
				{
					invokeMouseDown = true;
					cp_middle = true;
				}
				else
				{
					invokeMouseUp = false;
					if (cp_middle)
					{
						onMouseClick(new MouseClickEvent(this, input, MouseButton.MiddleMouseButton));
					}
					cp_middle = false;
				}

				//We call the MouseDown and MouseUp event only once, even if multiple buttons
				//are down, because we can use MouseEvent.Input to get the inputstate object
				//and check the buttons from there.
				if (invokeMouseDown)
					onMouseDown(new MouseEvent(this, input));
				if (invokeMouseUp)
					onMouseUp(new MouseEvent(this, input));

				//Indicate for the next call that we were hovering
				previousHover = true;
			}
			else
			{
				//Detecting whether we just left the bounds of the component
				if (previousHover)
				{
					onMouseLeave(new MouseEvent(this, input));
				}

				//We dont call the mouse up events, because the mouse is no longer hovering over the component
				if (ms.LeftButton == ButtonState.Released)
				{
					cp_left = false;
				}
				if (ms.MiddleButton == ButtonState.Released)
				{
					cp_middle = false;
				}
				if (ms.RightButton == ButtonState.Released)
				{
					cp_right = false;
				}
				previousHover = false;
			}
		}

		/// <summary>
		/// Helper function that sets the base texture of the component.
		/// </summary>
		/// <param name="texture">The new base texture</param>
		/// <param name="resize"> Whether to resize the component to fit the new texture</param>
		public virtual void SetBaseTexture(Texture2D texture, bool resize)
		{
			baseTexture = texture;
			if (resize)
			{
				Size = new Vector2(texture.Width, texture.Height);
			}
		}

		public virtual void Unload()
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		/// <summary>
		/// Helper function for determining whether the user is hovering the mouse cursor over the
		/// component
		/// </summary>
		/// <param name="state"></param>
		/// <returns>
		/// Returns true if the mouse cursor is within the bounds of the component, false if
		/// otherwise
		/// </returns>
		private bool IsMouseHovering(MouseState state)
		{
			return Bounds.Intersects(new Rectangle(state.X, state.Y, 0, 0));
		}

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		/// <summary>
		/// This event occurs when the user releases a mousebutton while hovering over the button
		/// </summary>
		public event EventHandler<MouseEvent> MouseUp;

		/// <summary>
		/// This event occurs when the user presses the mousebutton while hovering over the
		/// component, and releasing the mousebutton while hovering over the component
		/// </summary>
		public event EventHandler<MouseClickEvent> MouseClick;

		/// <summary>
		/// This event occurs while the user is holding a mousebutton while hovering over the button
		/// </summary>
		public event EventHandler<MouseEvent> MouseDown;

		/// <summary>
		/// This event occurs when the user moves the mouse cursor into the bounds of the component
		/// </summary>
		public event EventHandler<MouseEvent> MouseEnter;

		/// <summary>
		/// This event occurs when the user hovers over the button
		/// </summary>
		public event EventHandler<MouseEvent> MouseHover;

		/// <summary>
		/// This event occurs when the user moves the mouse cursor outside of the components bounds.
		/// </summary>
		public event EventHandler<MouseEvent> MouseLeave;

		/// <summary>
		/// Safely invokes the MouseClick event
		/// </summary>
		/// <param name="e"></param>
		protected void onMouseClick(MouseClickEvent e)
		{
			if (MouseClick != null)
			{
				MouseClick(this, e);
			}
		}

		/// <summary>
		/// Safely invokes the MouseDown event
		/// </summary>
		/// <param name="e"></param>
		protected void onMouseDown(MouseEvent e)
		{
			if (MouseDown != null)
			{
				MouseDown(this, e);
			}
		}

		/// <summary>
		/// Safely invokes the MouseEnter event
		/// </summary>
		/// <param name="e"></param>
		protected void onMouseEnter(MouseEvent e)
		{
			if (MouseEnter != null)
			{
				MouseEnter(this, e);
			}
		}

		/// <summary>
		/// Safely invokes the MouseHover event
		/// </summary>
		/// <param name="e"></param>
		protected void onMouseHover(MouseEvent e)
		{
			if (MouseHover != null)
			{
				MouseHover(this, e);
			}
		}

		/// <summary>
		/// Safely invokes the MouseLeave event
		/// </summary>
		/// <param name="e"></param>
		protected void onMouseLeave(MouseEvent e)
		{
			if (MouseLeave != null)
			{
				MouseLeave(this, e);
			}
		}

		/// <summary>
		/// Safely invokes the MouseUp event
		/// </summary>
		/// <param name="e"></param>
		protected void onMouseUp(MouseEvent e)
		{
			if (MouseUp != null)
			{
				MouseUp(this, e);
			}
		}

		#endregion Events
	}
}