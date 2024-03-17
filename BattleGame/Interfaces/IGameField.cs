using System;
using BattleGame.Models;

namespace BattleGame.Interfaces;

public interface IGameField
{
    int Id { get; set; }
	Game Game { get; set; }
	int posX { get; set; }
	int posY { get; set; }
	bool Revield { get; set; }
	bool HasMine { get; set; }
	int MinesAround { get; set; }

}

