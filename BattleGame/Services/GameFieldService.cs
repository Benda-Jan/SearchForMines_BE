using System;
using BattleGame.Api;
using BattleGame.Data;
using BattleGame.Interfaces;
using BattleGame.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleGame.Services;

/// <summary>
/// Operations over game fields
/// </summary>
public class GameFieldService
	{
    private readonly ApplicationContext _context;

    public GameFieldService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<GameField[]> GetAll(int gameId)
    {
        var result = await _context.GameFields.Where(f => f.GameId == gameId).ToArrayAsync()
            ?? throw new KeyNotFoundException();
        return result;
    }

    public async Task<string> Selected(GameFieldInputDto input)
    {
        var result = await _context.GameFields.SingleOrDefaultAsync(f => f.GameId == input.GameId && f.posX == input.PosX && f.posY == input.PosY)
            ?? throw new KeyNotFoundException();

        var gameStatus = "Finished";

        if (result.HasMine)
        {
            var game = await _context.Games.FindAsync(result.GameId);
            if (game is not null)
            {
                game.State = GameState.Finished;
                game.TimeFinished = DateTime.UtcNow;
            }
        }
        else
        {
            var game = _context.GameFields.Where(f => f.GameId == input.GameId).ToArray()
                ?? throw new KeyNotFoundException();
            await Reveal(game, result.posX, result.posY);
            gameStatus = "Active";
        }
        
        await _context.SaveChangesAsync();

        return gameStatus;
    }

    private async Task Reveal(GameField[] game, int posX, int posY)
    {
        var actualField = game.Where(f => f.posX == posX && f.posY == posY).SingleOrDefault();
        if (actualField is null || actualField.Revield)
            return;

        actualField.Revield = true;

        if (actualField.MinesAround > 0)
            return;

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
                if (i != 0 || j != 0)
                    await Reveal(game, posX + i, posY + j);
    }
}

