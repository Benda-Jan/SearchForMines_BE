using System;
namespace BattleGame.Models;

public class GameField
{
    public int Id { get; set; }
	public int GameId { get; set; }
	public int posX { get; set; }
	public int posY { get; set; }
	public bool Revield { get; set; }
	public bool HasMine { get; set; }
	public int MinesAround { get; set; }

}

