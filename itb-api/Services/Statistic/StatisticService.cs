using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itb_api.Models.Database;
using itb_api.Models.Instagram;
using itb_api.Services.Database;
using itb_api.Services.Instagram;
using MongoDB.Driver;

namespace itb_api.Services.Statistic
{
    public class StatisticService : IStatisticService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMongoCollection<StatisticsCollection> _collection;
        private readonly IInstagramService _instagramService;

        public StatisticService(
            IDatabaseService databaseService,
            IInstagramService instagramService
        )
        {
            _databaseService = databaseService;
            _instagramService = instagramService;
            _collection = _databaseService.Database.GetCollection<StatisticsCollection>("statistics");
        }

        public async Task<List<StatisticsCollection>> ReadBulkAsync(string username1, string? username2 = "")
        {
            List<StatisticsCollection> _result = new List<StatisticsCollection>();
            _result.Add(await ReadOrDownloadStatistic(username1));
            if (username2 != null && !username2.Equals(""))
            {
                _result.Add(await ReadOrDownloadStatistic(username2));
            }

            return _result;
        }

        public async Task<List<StatisticsCollection>> CreateAsync(
            string username,
            string avatarUrl,
            int postsCount,
            int followedBy,
            int follows,
            List<StatisticsPostsCollection> posts
        )
        {
            StatisticsCollection _statistic = new StatisticsCollection()
            {
                Username = username,
                AvatarUrl = avatarUrl,
                PostsCount = postsCount,
                FollowedBy = followedBy,
                Follows = follows,
                Posts = posts,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _collection.InsertOneAsync(_statistic);
            return await ReadAsync(username);
        }

        public async Task<List<StatisticsCollection>> ReadAsync(string username)
        {
            FilterDefinition<StatisticsCollection> _filter =
                Builders<StatisticsCollection>.Filter.Eq("Username", username);

            return await ((await _collection.FindAsync(_filter)).ToListAsync());
        }

        public async Task<List<StatisticsCollection>> UpdateAsync(
            string username,
            string avatarUrl,
            int postsCount,
            int followedBy,
            int follows,
            List<StatisticsPostsCollection> posts
        )
        {
            FilterDefinition<StatisticsCollection> _filter =
                Builders<StatisticsCollection>.Filter.Eq("Username", username);

            UpdateDefinition<StatisticsCollection> _update = Builders<StatisticsCollection>.Update
                .Set("AvatarUrl", avatarUrl)
                .Set("PostsCount", postsCount)
                .Set("FollowedBy", followedBy)
                .Set("Follows", follows)
                .Set("Posts", posts)
                .Set("UpdatedAt", DateTime.Now);

            await _collection.UpdateOneAsync(_filter, _update);
            return await ReadAsync(username);
        }

        public async Task DeleteAsync(string username)
        {
            FilterDefinition<StatisticsCollection> _filter =
                Builders<StatisticsCollection>.Filter.Eq("Username", username);
            await _collection.FindOneAndDeleteAsync(_filter);
        }

        private async Task<StatisticsCollection> ReadOrDownloadStatistic(string username)
        {
            List<StatisticsCollection> statistics = await ReadAsync(username);
            if (statistics.Count > 0)
            {
                StatisticsCollection statistic = statistics.First();

                if (DateTime.Now - statistic.UpdatedAt > TimeSpan.FromDays(1))
                {
                    StatisticsCollection _downloaded = await DownloadStatistic(username);
                    return (await UpdateAsync(
                        _downloaded.Username, 
                        _downloaded.AvatarUrl, 
                        _downloaded.PostsCount,
                        _downloaded.FollowedBy, 
                        _downloaded.Follows, 
                        _downloaded.Posts
                        )).First();
                }
                else
                {
                    return statistic;
                }
            }
            else
            {
                StatisticsCollection _downloaded = await DownloadStatistic(username);
                return (await CreateAsync(
                    _downloaded.Username, 
                    _downloaded.AvatarUrl, 
                    _downloaded.PostsCount,
                    _downloaded.FollowedBy, 
                    _downloaded.Follows, 
                    _downloaded.Posts
                )).First();
            }
        }

        private async Task<StatisticsCollection> DownloadStatistic(string username)
        {
            GetUserProfileByUsernameResponse _profile = await _instagramService.GetUserProfileByUsernameAsync(username);

            List<StatisticsPostsCollection> _posts = new List<StatisticsPostsCollection>();
            if (_profile.graphql.user.edge_owner_to_timeline_media.edges.Count > 0)
            {
                foreach (Edge edge in _profile.graphql.user.edge_owner_to_timeline_media.edges)
                {
                    StatisticsPostsCollection _post = new StatisticsPostsCollection()
                    {
                        LikesCount = edge.node.edge_liked_by.count,
                        CommentsCount = edge.node.edge_media_to_comment.count
                    };
                    _posts.Add(_post);
                }
            }
            
            StatisticsCollection _statistic = new StatisticsCollection()
            {
                Username = _profile.graphql.user.username,
                AvatarUrl = _profile.graphql.user.profile_pic_url_hd,
                PostsCount = _profile.graphql.user.edge_owner_to_timeline_media.count,
                FollowedBy = _profile.graphql.user.edge_followed_by.count,
                Follows = _profile.graphql.user.edge_follow.count,
                Posts = _posts
            };

            return _statistic;
        }
    }
}