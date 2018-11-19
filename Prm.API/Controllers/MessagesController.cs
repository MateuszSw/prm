using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Prm.API.Data;
using Prm.API.Dtos;
using Prm.API.Helpers;
using Prm.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prm.API.Controllers
{
    [ServiceFilter(typeof(LogUser))]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IPrmRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IPrmRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageRepo = await _repo.GetMessage(id);

            if (messageRepo == null)
                return NotFound();

            return Ok(messageRepo);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMessagesUser (int userId, 
            [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            messageParams.UserId = userId;
            var messagesRepo = await _repo.GetMessagesUser (messageParams);
            var messages = _mapper.Map<IEnumerable<MessageDto>>(messagesRepo);
            Response.AddPagination(messagesRepo.CurrentPage, messagesRepo.SizePage, 
                messagesRepo.TotalCount, messagesRepo.Total);
            return Ok(messages);
        }

        [HttpGet("conversation/{recipientId}")]
        public async Task<IActionResult> GetConversation (int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var messagesFromRepo = await _repo.GetConversation (userId, recipientId);
            var messageThread = _mapper.Map<IEnumerable<MessageDto>>(messagesFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageCreationDto messageCreationDto)
        {
            var sender = await _repo.GetUser(userId, false);
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            messageCreationDto.IdSender = userId;
            var recipient = await _repo.GetUser(messageCreationDto.RecipientId, false);
            if (recipient == null)
                return BadRequest("nie znaleziono użytkownika");
            var message = _mapper.Map<Message>(messageCreationDto);
            _repo.Add(message);
            if (await _repo.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageDto>(message);
                return CreatedAtRoute("GetMessage", new {id = message.Id}, messageToReturn);
            }
            throw new Exception("wiadomość nie została zapisana");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();  

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.IdSender == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.Delete(messageFromRepo);
            
            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Error deleting the message");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();  

            var message = await _repo.GetMessage(id);

            if (message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _repo.SaveAll();

            return NoContent();
        }
    }
}