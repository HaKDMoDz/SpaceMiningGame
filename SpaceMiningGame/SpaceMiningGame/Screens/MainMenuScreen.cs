#region Using statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion Using statements

namespace SpaceMiningGame.Screens
{
	public class MainMenuScreen : GameScreen
	{
		#region Fields

		#endregion Fields

		#region Properties

		#endregion Properties

		#region Constructor

		public MainMenuScreen(ScreenManager manager)
			: base(manager)
		{
		}

		#endregion Constructor

		#region Deconstructor

		~MainMenuScreen()
		{
		}

		#endregion Deconstructor

		#region Methods

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			base.HandleInput(gameTime, input);
		}

		public override void Load()
		{
			base.Load();
		}

		public override void Unload()
		{
			base.Unload();
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		#endregion Methods

		#region Static Methods

		#endregion Static Methods

		#region Events

		#endregion Events
	}
}