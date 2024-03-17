using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BattleGame.Services;
using BattleGame.Api;
using Microsoft.AspNetCore.Authorization;

namespace BattleGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameService _service;

        public GameController(GameService service)
        {
            _service = service;
        }

        // GET: /Game/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GameDto>> Get(int id)
        {
            var game = await _service.Get(id);
            return Ok(GameDto.From(game));
        }

        // GET: /Game/
        [HttpGet]
        public async Task<ActionResult<GameDto[]>> GetAll()
        {
            var games = await _service.GetAll();
            var gameDtos = games.Select(GameDto.From).ToArray();
            return Ok(gameDtos);
        }

        // POST: /Game/
        [HttpPost]
        public async Task<ActionResult> Create(GameInputDto input)
        {
            var result = await _service.Create(input.Name, input.MinesCount);
            var gameDto = GameDto.From(result);
            return CreatedAtAction(nameof(Get), new { result.Id }, result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}

