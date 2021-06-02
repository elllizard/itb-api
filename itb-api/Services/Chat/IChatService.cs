using System.Collections.Generic;
using System.Threading.Tasks;
using itb_api.Models.Database;

namespace itb_api.Services.Chat
{
    public interface IChatService
    {
        public Task<List<ChatsCollection>> CreateAsync(
            long chatId,
            string? username = null,
            string? path = null,
            string? state = null
        );
        public Task<List<ChatsCollection>> ReadAsync(long chatId);
        public Task<List<ChatsCollection>> UpdateAsync(
            long chatId,
            string? username = null,
            string? path = null,
            string? state = null
        );
        public Task DeleteAsync(long chatId);
    }
}