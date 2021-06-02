using System;
using System.Threading.Tasks;
using itb_api.Models.Instagram;

namespace itb_api.Services.Instagram
{
    public interface IInstagramService
    {
        public Task<GetUserProfileByUsernameResponse> GetUserProfileByUsernameAsync(string username);
    }
}