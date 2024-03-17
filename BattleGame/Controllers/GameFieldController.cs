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
    public class GameFieldController : Controller
    {
        private readonly GameFieldService _service;

        public GameFieldController(GameFieldService service)
        {
            _service = service;
        }

        // GET: /GameField/{gameId}
        [HttpGet]
        [Route("{gameId}")]
        public async Task<ActionResult<GameFieldDto[]>> FindAll(int gameId)
        {
            var gameFields = await _service.GetAll(gameId);
            var gameFieldDtos = gameFields.Select(x =>GameFieldDto.From(x)).ToArray();
            return Ok(gameFields);
        }

        // PUT: /GameField/{id}
        [HttpPut]
        public async Task<ActionResult<string>> Selected(GameFieldInputDto input)
        {
            var result = await _service.Selected(input);
            return Ok(result);
        }
    }
}

