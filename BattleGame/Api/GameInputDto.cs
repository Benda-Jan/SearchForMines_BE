using System;
namespace BattleGame.Api
{
    /// <summary>
    /// Create object for Game object
    /// </summary>
    public class GameInputDto
	{
        /// <summary>
        /// Name of game
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Number of mines in the game
        /// </summary>
        public int MinesCount { get; set; }
    }
}

