using System;
using BattleGame.Models;
using BattleGame.Interfaces;
using System.Text.Json.Serialization;

namespace BattleGame.Api;

	public class GameFieldDto
	{

    [JsonConstructor]
    public GameFieldDto(int Id, int GameId, int posX, int posY, bool Revield, bool HasMine, int MinesAround)
    {
        this.Id = Id;
        this.GameId = GameId;
        this.posX = posX;
        this.posY = posY;
        this.Revield = Revield;
        this.HasMine = HasMine;
        this.MinesAround = MinesAround;
    }


    public GameFieldDto(GameField source)
    {
        Id = source.Id;
        GameId = source.GameId;
        posX = source.posX;
        posY = source.posY;
        Revield = source.Revield;
        HasMine = source.HasMine;
        MinesAround = source.MinesAround;
    }

    public int Id { get; }
    public int GameId { get; }
    public int posX { get; }
    public int posY { get; }
    public bool Revield { get; }
    public bool HasMine { get; }
    public int MinesAround { get; }

    public static GameFieldDto? From(GameField? source)
        => source is null ? null : new GameFieldDto(source);
}

