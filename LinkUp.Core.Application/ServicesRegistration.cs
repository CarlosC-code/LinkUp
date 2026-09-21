using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LinkUp.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Configurations
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion

            #region Services IOC
            services.AddScoped<IReactionService, ReactionService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IFriendRequestService, FriendRequestService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IBattleshipService, BattleshipService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
            #endregion
        }
    }
}
