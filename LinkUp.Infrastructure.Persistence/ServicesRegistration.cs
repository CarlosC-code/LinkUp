using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using LinkUp.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkUp.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region Contexts
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<LinkUpAppContext>(opt =>
                                              opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<LinkUpAppContext>(opt =>
                opt.UseSqlServer(connectionString,
                m => m.MigrationsAssembly(typeof(LinkUpAppContext).Assembly.FullName))
                , ServiceLifetime.Transient);
            }
            #endregion

            #region Repostories IOC
            services.AddScoped(typeof(Core.Domain.Interface.IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBattleshipAttackRepository, BattleshipAttackRepository>();
            services.AddScoped<IBattleshipGameRepository, BattleshipGameRepository>();
            services.AddScoped<IBattleshipShipRepository, BattleshipShipRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            services.AddScoped<IFriendshipRepository, FriendshipRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IReactionRepository, ReactionRepository>();
            #endregion
        }
    }
}

