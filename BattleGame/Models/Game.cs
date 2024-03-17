using System;
using System.ComponentModel.DataAnnotations;

namespace BattleGame.Models;

public enum GameState
{
	Active,
	Finished,
	Undefined
};

public class Game
{
    public int Id { get; set; }
	[Required]
	[MaxLength(50)]
	public required string Name { get; set; } = String.Empty;
	public GameState State { get; set; } = GameState.Active;
	[Required]
	public required int MinesCount { get; set; }
	public DateTime? TimeCreated { get; set; } = null!;
	public DateTime? TimeFinished { get; set; }
    public List<GameField> GameFields { get; set; } = new List<GameField>();

	/*public static string ToString(GameState state)
		=> state switch
		{
			GameState.Active => nameof(GameState.Active),
			GameState.Finished => nameof(GameState.Finished),
			_ => "Undefined"
		};*/
}

