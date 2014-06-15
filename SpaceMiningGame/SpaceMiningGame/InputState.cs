#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame
{
	/// <summary>
	/// Helper class for reading user input. This class keeps track of the current and previous
	/// keyboard states.
	/// </summary>
	public class InputState
	{
		#region Fields

		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets the current keyboard state instance
		/// </summary>
		public KeyboardState CurrentKeyboardState
		{
			get { return currentKeyboardState; }
		}

		/// <summary>
		/// Gets the previous keyboard state instance
		/// </summary>
		public KeyboardState PreviousKeyboardState
		{
			get { return previousKeyboardState; }
		}

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Constructs a new input state.
		/// </summary>
		public InputState()
		{
			currentKeyboardState = Keyboard.GetState();
			previousKeyboardState = Keyboard.GetState();
		}

		#endregion Constructor

		#region Deconstructor

		~InputState()
		{
		}

		#endregion Deconstructor

		#region Public Methods

		/// <summary>
		/// Reads in the latest inputs from the user(s)
		/// </summary>
		public void Update()
		{
			previousKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState();
		}

		#endregion Public Methods

		#region Private Methods

		#endregion Private Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}