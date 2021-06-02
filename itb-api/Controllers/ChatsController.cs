using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itb_api.Models.Api;
using itb_api.Models.Database;
using itb_api.Services.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace itb_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly ILogger<ChatsController> _logger;
        private readonly IChatService _chatService;

        public ChatsController(
            ILogger<ChatsController> logger,
            IChatService chatService
        )
        {
            _logger = logger;
            _chatService = chatService;
        }

        [HttpPost]
        [Route("")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequestBody update)
        {
            _logger.LogInformation($"Received create request for chat with id '{update.ChatId}'.");
            try
            {
                List<ChatsCollection> chats =
                    await _chatService.CreateAsync(update.ChatId, update.Username, update.Path, update.State);
                if (chats.Count > 0)
                {
                    ChatsCollection chat = chats.First();

                    return Ok(chat);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id:long:required}")]
        [Produces("application/json")]
        public async Task<IActionResult> ReadChat(long id)
        {
            _logger.LogInformation($"Received read request for chat with id '{id}'.");
            try
            {
                List<ChatsCollection> chats = await _chatService.ReadAsync(id);
                if (chats.Count > 0)
                {
                    ChatsCollection chat = chats.First();

                    return Ok(chat);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("{id:long:required}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateChat(long id, [FromBody] UpdateChatRequestBody update)
        {
            _logger.LogInformation($"Received update request for chat with id '{id}'.");
            try
            {
                List<ChatsCollection> chats =
                    await _chatService.UpdateAsync(id, update.Username, update.Path, update.State);
                if (chats.Count > 0)
                {
                    ChatsCollection chat = chats.First();

                    return Ok(chat);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{id:long:required}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteChat(long id)
        {
            _logger.LogInformation($"Received delete request for chat with id '{id}'.");
            try
            {
                await _chatService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}