using System;

namespace itb_api.Models.Api
{
    [Serializable]
    public class CreateChatRequestBody
    {
        public long ChatId { get; set; }

        public string? Username { get; set; } = null;

        public string? Path { get; set; } = null;

        public string? State { get; set; } = null;
    }
}