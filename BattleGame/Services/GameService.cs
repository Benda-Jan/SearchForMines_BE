using System;
using BattleGame.Data;
using BattleGame.Models;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace BattleGame.Services;

public class GameService
{
	private readonly ApplicationContext _context;
	private readonly IClock _clock;

	public GameService(ApplicationContext context, IClock clock)
	{
		_context = context;
		_clock = clock;
	}

	public async Task<Game> Get(int id)
	{
		var result = await _context.Games.FindAsync(id)
			?? throw new KeyNotFoundException();
		return result;
	}

	public Task<Game[]> GetAll()
		=> _context.Games.AsNoTracking().ToArrayAsync();

	public async Task<Game> Create(string name, int minesCount)
	{
		//gameDto.Name.ValidateLettersOrNumbers();
		if (minesCount <= 0 || minesCount >= 100)
			return null;

		var game = new Game
		{
			Name = name,
			MinesCount = minesCount,
			TimeCreated = _clock.GetCurrentInstant().ToDateTimeUtc()
		};

		// Generate Mine positions
		var mines = new List<(int x, int y)>();
		while (mines.Count < minesCount)
		{
			int tempX = Random.Shared.Next(0, 9);
			int tempY = Random.Shared.Next(0, 9);
			if (!mines.Contains((tempX, tempY)))
			{
				mines.Add((tempX, tempY));
			}
        }

        for (int x = 0; x < 10; x++) {
			for (int y = 0; y < 10; y++)
			{
				GameField gameField = new()
				{
					GameId = game.Id,
					posX = x,
					posY = y,
					Revield = false,
					HasMine = mines.Contains((x, y)),
					MinesAround = 0
				};
				game.GameFields.Add(gameField);
			}
		}

        var query = game.GameFields?.Where(x => x.GameId == game.Id && x.HasMine).ToArray() ?? Array.Empty<GameField>();

		foreach(var field in query)
		{
			for (int i = -1; i <= 1; i++)
				for (int j = -1; j <= 1; j++)
					if (i != 0 || j != 0)
					{
						var temp = game.GameFields?.Where(f => f.posX == field.posX + i && f.posY == field.posY + j && !f.HasMine).FirstOrDefault();
						if (temp is not null)
							temp.MinesAround++;
					}
        }

        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return game;
	}

	public async Task Delete(int id)
	{
		var game = await _context.Games.FindAsync(id)
			?? throw new KeyNotFoundException();
		_context.Games.Remove(game);
		await _context.SaveChangesAsync();
	}
}

