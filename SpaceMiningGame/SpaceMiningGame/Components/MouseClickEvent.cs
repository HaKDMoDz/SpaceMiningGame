#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame.Components
{
	public class MouseClickEvent : MouseEvent
	{
		#region Fields

		private MouseButton button;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets the mousebutton that was used to click
		/// </summary>
		public MouseButton Button
		{
			get { return button; }
		}

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Creates new MouseClickEvent object, used for MouseClick events in the ScreenComponent
		/// class.
		/// </summary>
		/// <param name="component">The component that created the event</param>
		/// <param name="input">    The inputstate at the time of the event</param>
		/// <param name="button">   The button that was used to make the click</param>
		public MouseClickEvent(ScreenComponent component, InputState input, MouseButton button)
			: base(component, input)
		{
			this.button = button;
		}

		#endregion Constructor

		#region Deconstructor

		~MouseClickEvent()
		{
		}

		#endregion Deconstructor

		#region Methods

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}