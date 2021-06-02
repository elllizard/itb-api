using System;
using System.Text;
using itb_api.Models.Configurtations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace itb_api.Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        private readonly DatabaseConfiguration _databaseConfig;

        public DatabaseService(IOptions<DatabaseConfiguration> databaseConfig)
        {
            _databaseConfig = databaseConfig.Value;

            StringBuilder _url = new StringBuilder();
            _url.Append(_databaseConfig.Url);
            _url.Replace("[user]", _databaseConfig.User);
            _url.Replace("[password]", _databaseConfig.Password);
            _url.Replace("[name]", _databaseConfig.Name);

            Client = new MongoClient(_url.ToString());

            Database = Client.GetDatabase(_databaseConfig.Name);
        }
    }
}