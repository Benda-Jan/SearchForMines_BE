using System;
using BattleGame.Models;

namespace BattleGame.Interfaces;

public interface IGame
{
	int Id { get; set; }
	string Name { get; set; }
	GameState State { get; set; }
	int MinesCount { get; set; }
	DateTime TimeCreated { get; set; }
	List<GameField>? GameFields { get; set; }

	string StateToString(GameState state);
			
}

