namespace SpaceMiningGame
{
#if WINDOWS || XBOX

	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		private static void Main(string[] args)
		{
			using (SpaceGame game = new SpaceGame())
			{
				game.Run();
			}
		}
	}

#endif
}