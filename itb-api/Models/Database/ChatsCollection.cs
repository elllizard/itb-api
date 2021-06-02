using System;
using MongoDB.Bson;

namespace itb_api.Models.Database
{
    public class ChatsCollection
    {
        public ObjectId Id { get; set; }

        public long ChatId { get; set; }

        public string? Username { get; set; } = null;

        public string? Path { get; set; } = null;

        public string? State { get; set; } = null;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}