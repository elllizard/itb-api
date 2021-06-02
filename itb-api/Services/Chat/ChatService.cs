using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using itb_api.Models.Database;
using itb_api.Services.Database;
using MongoDB.Driver;

namespace itb_api.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMongoCollection<ChatsCollection> _collection;

        public ChatService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _collection = _databaseService.Database.GetCollection<ChatsCollection>("chats");
        }

        public async Task<List<ChatsCollection>> CreateAsync(
            long chatId,
            string? username = null,
            string? path = null,
            string? state = null
        )
        {
            ChatsCollection _chat = new ChatsCollection()
            {
                ChatId = chatId,
                Username = username,
                Path = path,
                State = state,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _collection.InsertOneAsync(_chat);
            return await ReadAsync(chatId);
        }

        public async Task<List<ChatsCollection>> ReadAsync(long chatId)
        {
            FilterDefinition<ChatsCollection> _filter = Builders<ChatsCollection>.Filter.Eq("ChatId", chatId);

            return await ((await _collection.FindAsync(_filter)).ToListAsync());
        }

        public async Task<List<ChatsCollection>> UpdateAsync(
            long chatId,
            string? username = null,
            string? path = null,
            string? state = null
        )
        {
            FilterDefinition<ChatsCollection> _filter = Builders<ChatsCollection>.Filter.Eq("ChatId", chatId);

            UpdateDefinition<ChatsCollection> _update = Builders<ChatsCollection>.Update
                .Set("Username", username)
                .Set("Path", path)
                .Set("State", state)
                .Set("UpdatedAt", DateTime.Now);

            await _collection.UpdateOneAsync(_filter, _update);
            return await ReadAsync(chatId);
        }

        public async Task DeleteAsync(long chatId)
        {
            FilterDefinition<ChatsCollection> _filter = Builders<ChatsCollection>.Filter.Eq("ChatId", chatId);
            await _collection.FindOneAndDeleteAsync(_filter);
        }
    }
}