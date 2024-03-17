using System;
using System.Text;
using Xunit;
using System.Net.Http.Headers;
using BattleGame.Api;
using BattleGame.Data;
using BattleGame.Component.Test.Models;

namespace BattleGame.Component.Test;

public class ControllerTests : IClassFixture<TestApplicationFactory>
{
	private const string Route = "Game";
	private const string RouteField = "GameField";
    private readonly HttpClient _httpClient;
	private readonly TestApplicationFactory _factory;

	public ControllerTests(TestApplicationFactory factory)
	{
		_factory = factory;
		_httpClient = _factory.CreateClient();

		var authenticationString = "JanBenda:Password123";
		var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }

    [Fact]
    public async Task Get_Correct()
    {
        var result = await _httpClient.GetFromJsonAsync<GameDto>($"{Route}/{TestDataSeeder.GAME_1_ID}");

        Assert.NotNull(result);
        Assert.Equal(result.Id, TestDataSeeder.GAME_1_ID);
    }

    [Fact]
	public async Task GetAll_Correct()
	{
		var result = await _httpClient.GetFromJsonAsync<GameDto[]>(Route);

		Assert.NotNull(result);
		Assert.NotEmpty(result);
	}

	[Fact]
	public async Task Create_Correct()
	{
		var context = _factory.Services.GetRequiredService<ApplicationContext>();
		var game = new GameInputDto() { Name = "Game3", MinesCount = 20 };

		var result = await _httpClient.PostAsJsonAsync(Route, game);

		Assert.NotNull(result);
		Assert.True(result.IsSuccessStatusCode);
		Assert.NotNull(result.Headers.Location);
		var resultGame = context.Games.SingleOrDefault(x => x.Name == game.Name); // DbSet<Game> does not contain async version of 'SingleOrDefault' ???
		Assert.NotNull(resultGame);
    }

    [Fact]
    public async Task Delete_Correct()
    {
        var context = _factory.Services.GetRequiredService<ApplicationContext>();

        var result = await _httpClient.DeleteAsync($"{Route}/{TestDataSeeder.GAME_1_ID}");

        Assert.NotNull(result);
        Assert.True(result.IsSuccessStatusCode);
        var resultGame = context.Games.SingleOrDefault(x => x.Id == TestDataSeeder.GAME_1_ID); // DbSet<Game> does not contain async version of 'SingleOrDefault' ???
        Assert.Null(resultGame);
    }

    [Fact]
    public async Task FindAll_Correct()
    {
        var result = await _httpClient.GetFromJsonAsync<GameFieldDto[]>($"{RouteField}/{TestDataSeeder.GAME_1_ID}");

        Assert.NotNull(result);
        Assert.NotEmpty(result);

    }

    [Fact]
    public async Task Select_Correct()
    {

        var context = _factory.Services.GetRequiredService<ApplicationContext>();
        var gameField = context.GameFields.Where(x => x.GameId == TestDataSeeder.GAME_1_ID).Single();
        var input = new GameFieldInputDto() { GameId = gameField.GameId, PosX = gameField.posX, PosY = gameField.posY };
        var result = await _httpClient.PutAsJsonAsync(RouteField, input);

        Assert.NotNull(result);
        Assert.True(result.IsSuccessStatusCode);
    }
}

