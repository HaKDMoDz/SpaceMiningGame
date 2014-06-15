#region Using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame
{
	/// <summary>
	/// Enum describes the screen transition state.
	/// </summary>
	public enum ScreenState
	{
		/// <summary>
		/// Indicates the screen is transitioning into the screen
		/// </summary>
		TransitionOn,

		/// <summary>
		/// Indicates the screen is fully active
		/// </summary>
		Active,

		/// <summary>
		/// Indicates the scree is transitioning away.
		/// </summary>
		TransitionOff,

		/// <summary>
		/// Indicates the screen is not visible
		/// </summary>
		Hidden,
	}
}