using System;
using System.Text.Json.Serialization;
using BattleGame.Models;

namespace BattleGame.Api;

	public class GameDto
	{

    [JsonConstructor]
    public GameDto(int Id, string Name, string State, int MinesCount, DateTime? TimeCreated, DateTime? TimeFinished, List<GameFieldDto>? GameFields)
    {
        this.Id = Id;
        this.Name = Name;
        this.State = State;
        this.MinesCount = MinesCount;
        this.TimeCreated = TimeCreated;
        this.TimeFinished = TimeFinished;
        this.GameFields = GameFields;
    }

    public GameDto(Game source)
    {
        Id = source.Id;
        Name = source.Name;
        State = source.State.ToString();
        MinesCount = source.MinesCount;
        TimeCreated = source.TimeCreated;
        TimeFinished = source.TimeFinished;
        GameFields = source.GameFields?.Select(f => GameFieldDto.From(f)).Where(x => x is not null).Select(x => x!).ToList();
    }

    public int Id { get; }
    public string Name { get; } = String.Empty;
    public string State { get; } = String.Empty;
    public int MinesCount { get; }
    public DateTime? TimeCreated { get; }
    public DateTime? TimeFinished { get; }
    public List<GameFieldDto>? GameFields { get; }

    public static GameDto? From(Game? source)
        => source is null ? null : new GameDto(source);
	}

