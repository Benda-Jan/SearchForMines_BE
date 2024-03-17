using System;
namespace BattleGame.Api
{
    /// <summary>
    /// Create object for Game object
    /// </summary>
    public class GameFieldInputDto
	{
        /// <summary>
        /// Id of game
        /// </summary>
        public required int GameId { get; set; }

        /// <summary>
        /// Position X in the game
        /// </summary>
        public required int PosX { get; set; }

        /// <summary>
        /// Position Y in the game
        /// </summary>
        public required int PosY { get; set; }
    }
}

