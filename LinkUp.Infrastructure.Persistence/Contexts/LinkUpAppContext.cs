using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Entities.Social;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LinkUp.Infrastructure.Persistence.Contexts
{
    public class LinkUpAppContext : DbContext
    {
        public LinkUpAppContext(DbContextOptions<LinkUpAppContext> options) : base(options) { }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<BattleshipAttack> BattleshipAttacks { get; set; }
        public DbSet<BattleshipGame> BattleshipGames { get; set; }
        public DbSet<BattleshipShip> BattleshipShips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //Liskov-substitution

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
