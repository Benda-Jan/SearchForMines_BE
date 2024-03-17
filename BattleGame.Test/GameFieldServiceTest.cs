using Xunit;
using NodaTime.Testing;
using NodaTime;
using BattleGame.Data;
using BattleGame.Services;
using BattleGame.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BattleGame.Api;

namespace BattleGame.Test;

public class GameFieldServiceTest
{
    [Fact]
    public async Task GetAll_CorrectItem()
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var gameService = new GameService(context, new FakeClock(currentTime));

        var addedItem1 = await gameService.Create("NewGame1", 10);

        var sut = new GameFieldService(context);
        var result = await sut.GetAll(addedItem1.Id);

        Assert.NotNull(result);
        Assert.Equal(result.Count(), addedItem1.GameFields.Count);
    }

    [Fact]
    public async Task Selected_Correct()
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var gameService = new GameService(context, new FakeClock(currentTime));
        var addedItem1 = await gameService.Create("NewGame1", 1);

        var sut = new GameFieldService(context);
        var selected1 = new GameFieldInputDto() { GameId = addedItem1.Id, PosX = 0, PosY = 0};
        var selected2 = new GameFieldInputDto() { GameId = addedItem1.Id, PosX = 1, PosY = 0};
        var result1 = await sut.Selected(selected1);
        var result2 = await sut.Selected(selected2);

        bool result = (result1 == "Active") || (result2 == "Active");

        Assert.NotNull(result1);
        Assert.NotNull(result2);

        Assert.True(result);
    }

    [Fact]
    public async Task Selected_Incorrect()
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var gameService = new GameService(context, new FakeClock(currentTime));
        var addedItem1 = await gameService.Create("NewGame1", 1);

        var sut = new GameFieldService(context);
        var selected1 = new GameFieldInputDto() { GameId = addedItem1.Id, PosX = 0, PosY = 0 };
        var selected2 = new GameFieldInputDto() { GameId = addedItem1.Id, PosX = 1, PosY = 0 };

        var result1 = await sut.Selected(selected1);
        var result2 = await sut.Selected(selected2);

        bool result = (result1 == "Finished") || (result2 == "Finished");

        Assert.NotNull(result1);
        Assert.NotNull(result2);

       //Assert.False(result);
    }

    [Fact]
    public async Task Reveal_Correct()
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var gameService = new GameService(context, new FakeClock(currentTime));
        var addedItem1 = await gameService.Create("NewGame1", 1);
        var sut = new GameFieldService(context);
        var selected1 = new GameFieldInputDto() { GameId = addedItem1.Id, PosX = 0, PosY = 0 };

        await sut.Selected(selected1);

        Assert.True(context.GameFields.Single(x => x.GameId == addedItem1.Id && x.posX == selected1.PosX && x.posY == selected1.PosY).Revield);
    }

    private static ApplicationContext CreateContext()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var builder = new DbContextOptionsBuilder<ApplicationContext>();
        builder.UseInMemoryDatabase("InMemoryForTesting").UseInternalServiceProvider(serviceProvider);

        var context = new ApplicationContext(builder.Options);
        return context;
    }
}
