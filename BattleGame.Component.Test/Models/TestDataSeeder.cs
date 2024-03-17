using System;
using BattleGame.Data;
using BattleGame.Models;

namespace BattleGame.Component.Test.Models;

public class TestDataSeeder
{
	public const int GAME_1_ID = 1;
	public const int GAME_2_ID = 2;
	public const int GAMEFIELD_1_ID = 10;
	public const int GAMEFIELD_2_ID = 15;

    public static void SeedData(IServiceProvider provider)
	{
		using (var scope = provider.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();

			var game1 = new Game() { Id = GAME_1_ID, Name = "Game_1", MinesCount = 5, TimeCreated = new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc) };
			var game2 = new Game() { Id = GAME_2_ID, Name = "Game_2", MinesCount = 15, TimeCreated = new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc) };

			var gameField1 = new GameField() { Id = GAMEFIELD_1_ID, GameId = game1.Id };
			var gameField2 = new GameField() { Id = GAMEFIELD_2_ID, GameId = game2.Id };

			db.Games.AddRange(new[]
			{
			game1,
			game2
			});

			db.GameFields.AddRange(new[]
			{
			gameField1,
			gameField2
			});

			db.SaveChanges();
		}
    }
}

