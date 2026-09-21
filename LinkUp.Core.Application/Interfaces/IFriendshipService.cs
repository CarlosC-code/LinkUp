using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IFriendshipService
    {
        Task<List<string>> GetFriendIdsAsync(string userId);
        Task<bool> AreFriendsAsync(string userA, string userB);
        Task DeleteFriendshipAsync(string userA, string userB);
        Task<int> GetMutualFriendsCountAsync(string userA, string userB);

    }
}
