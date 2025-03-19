using Application.DTOs.Message;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class MessageService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : IMessageService
{
    // get messages
    public async Task<IEnumerable<MessageResponseDto>> GetMessagesByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var messages = await unitOfWork.MessageRepository.GetAllByUserIdAsync(userId);
        var response = mapper.Map<IEnumerable<MessageResponseDto>>(messages);
        return response;
    }

    // sent messages
    public async Task<bool> AddMessageAsync(MessageRequestDto messageRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add message");

        var message = mapper.Map<Message>(messageRequestDto);
        message.UserId = existingUser.Id;
        message.UpdatedAt = DateTime.UtcNow;
        message.SentAt = DateTime.UtcNow;

        var result = await unitOfWork.MessageRepository.AddAsync(message);
        await unitOfWork.CommitAsync();
        return result;
    }

    // edit messages
    public async Task<bool> UpdateMessageAsync(UpdateMessageDto messageRequestDto, Guid messageId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingMessage = await unitOfWork.MessageRepository.GetByUserIdAsync(userId, messageId);
        if (existingMessage is null)
            throw new Exception("User does not exist to update message.");

        // Map DTO to existing entity to preserve id
        mapper.Map(messageRequestDto, existingMessage);
        existingMessage.UpdatedAt = DateTime.UtcNow;
        existingMessage.IsRead = true;

        var result = await unitOfWork.MessageRepository.UpdateAsync(existingMessage);
        await unitOfWork.CommitAsync();
        return result;
    }

    // delete messages
    public async Task<bool> DeleteMessageAsync(Guid messageId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingMessage = await unitOfWork.MessageRepository.GetByUserIdAsync(userId, messageId);
        if (existingMessage is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.MessageRepository.DeleteAsync(existingMessage);
        await unitOfWork.CommitAsync();
        return result;
    }

    // get unread messages
    public async Task<int> GetNumberOfUnread(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var messages = await unitOfWork.MessageRepository.GetNumberOfUnread(userId);
        return messages;
    }
}