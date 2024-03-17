using System;
using Microsoft.EntityFrameworkCore;
using BattleGame.Models;

namespace BattleGame.Data
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
		}

		public DbSet<Game> Games { get; set; } = null!;
        public DbSet<GameField> GameFields { get; set; } = null!;
    }
}

