using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace itb_api.Models.Database
{
    public class StatisticsCollection
    {
        public ObjectId Id { get; set; }

        public string Username { get; set; }

        public string AvatarUrl { get; set; }

        public int PostsCount { get; set; }

        public int FollowedBy { get; set; }

        public int Follows { get; set; }

        public List<StatisticsPostsCollection> Posts { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class StatisticsPostsCollection
    {
        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}