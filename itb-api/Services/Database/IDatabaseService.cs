using System;
using MongoDB.Driver;

namespace itb_api.Services.Database
{
    public interface IDatabaseService
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
    }
}