using System;

namespace itb_api.Models.Api
{
    [Serializable]
    public class UpdateChatRequestBody
    {
        public string? Username { get; set; } = null;

        public string? Path { get; set; } = null;

        public string? State { get; set; } = null;
    }
}