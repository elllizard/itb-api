using System.Collections.Generic;
using System.Threading.Tasks;
using itb_api.Models.Database;

namespace itb_api.Services.Statistic
{
    public interface IStatisticService
    {
        public Task<List<StatisticsCollection>> ReadBulkAsync(string username1, string? username2 = "");
        public Task<List<StatisticsCollection>> CreateAsync(
            string username,
            string avatarUrl,
            int postsCount,
            int followedBy,
            int follows,
            List<StatisticsPostsCollection> posts
        );

        public Task<List<StatisticsCollection>> ReadAsync(string username);

        public Task<List<StatisticsCollection>> UpdateAsync(
            string username,
            string avatarUrl,
            int postsCount,
            int followedBy,
            int follows,
            List<StatisticsPostsCollection> posts
        );

        public Task DeleteAsync(string username);
    }
}