using Xunit;
using NodaTime.Testing;
using NodaTime;
using BattleGame.Data;
using BattleGame.Services;
using BattleGame.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BattleGame.Test;

public class GameServiceTest
{
    [Fact]
    public async Task Get_CorrectItem()
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var sut = new GameService(context, new FakeClock(currentTime));

        var addedItem1 = await sut.Create("NewGame1", 10);
        var addedItem2 = await sut.Create("NewGame2", 10);
        var result = await sut.Get(addedItem1.Id);

        Assert.NotNull(result);
        Assert.Equal(addedItem1.Id, result.Id);
    }

    [Fact]
    public async Task Get_IncorrectItem()
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var sut = new GameService(context, new FakeClock(currentTime));

        var addedItem = await sut.Create("NewGame", 10);
        Game result = new Game { Name = "t", MinesCount = 1 };
        try
        {
            result = await sut.Get(addedItem.Id + 5);
        }
        catch (KeyNotFoundException ex)
        {
            Assert.NotEqual(result.Id, addedItem.Id);
        } 
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public async Task Get_All_Items(int N)
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var sut = new GameService(context, new FakeClock(currentTime));

        for (int i = 0; i < N; i++)
        {
            var addedItem1 = await sut.Create($"NewGame{i}", i + 2);
        }

        var result = await sut.GetAll();

        Assert.NotNull(result);
        Assert.Equal(result.Length, N);
    }

    [Fact]
    public async Task Delete_CorrectItem()
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var sut = new GameService(context, new FakeClock(currentTime));

        var addedItem1 = await sut.Create("NewGame1", 10);
        var addedItem2 = await sut.Create("NewGame2", 10);

        await sut.Delete(addedItem1.Id);

        var result = await sut.Get(addedItem2.Id);

        Assert.Single(context.Games);
        Assert.Equal(addedItem2.Id, result.Id);
    }

    [Fact]
    public async Task Create_CorrectNameAndMinesCount()
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var sut = new GameService(context, new FakeClock(currentTime));

        var result = await sut.Create("NewGame", 10);
        
        Assert.NotNull(result);
        Assert.Single(context.Games);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(120)]
    public async Task Create_IncorrectMinesCount(int count)
    {
        var currentTime = Instant.FromDateTimeUtc(new DateTime(2024, 5, 5, 5, 5, 5, DateTimeKind.Utc));
        await using var context = CreateContext();
        var sut = new GameService(context, new FakeClock(currentTime));

        var result = await sut.Create("NewGame", count);

        Assert.Null(result);
        Assert.Empty(context.Games);
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
