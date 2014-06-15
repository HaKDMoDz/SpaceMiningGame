using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMiningGame
{
	/// <summary>
	///
	/// </summary>
	public abstract class GameState
	{
		#region Members

		protected GameStateManager manager;

		#endregion Members

		#region Constructor & Deconstructor

		#endregion Constructor & Deconstructor

		#region Methods

		/// <summary>
		/// Called by the <see cref="GameStateManager"/> to initialize the gamestate
		/// </summary>
		public abstract void Initialize();

		/// <summary>
		/// Called when the gamestate should update inself
		/// </summary>
		/// <param name="gameTime"></param>
		public abstract void Update(GameTime gameTime);

		/// <summary>
		/// Called when the gamestate should draw inself
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spritebatch"></param>
		public abstract void Draw(GameTime gameTime, SpriteBatch spritebatch);

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}