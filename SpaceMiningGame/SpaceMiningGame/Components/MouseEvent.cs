#region Using statements

using SpaceMiningGame.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame.Components
{
	public class MouseEvent : EventArgs
	{
		#region Fields

		private ScreenComponent component;
		private InputState input;

		#endregion Fields

		/// <summary>
		/// Gets the component that send the event
		/// </summary>
		public ScreenComponent Component
		{
			get { return component; }
		}

		/// <summary>
		/// Gets the inputstate when the event occured
		/// </summary>
		public InputState Input
		{
			get { return input; }
		}

		#region Properties

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Creates a new MouseEvent instance used for ScreenComponent events
		/// </summary>
		/// <param name="component">The component that throws the event</param>
		/// <param name="input">    The inputstate at the time of throwing the event</param>
		public MouseEvent(ScreenComponent component, InputState input)
		{
			this.component = component;
			this.input = input;
		}

		#endregion Constructor

		#region Deconstructor

		~MouseEvent()
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