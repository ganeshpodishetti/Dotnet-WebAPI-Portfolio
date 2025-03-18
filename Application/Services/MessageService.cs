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
    public async Task<IEnumerable<MessageResponseDto>> GetMessagesByUserIdAsync(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var messages = await unitOfWork.MessageRepository.GetAllByUserIdAsync(userId);
        var response = mapper.Map<IEnumerable<MessageResponseDto>>(messages);
        return response;
    }

    public async Task<bool> AddMessageAsync(MessageRequestDto messageRequestDto, string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add message");

        var message = mapper.Map<Message>(messageRequestDto);
        message.UserId = existingUser.Id;
        message.CreatedAt = DateTime.UtcNow;
        message.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.MessageRepository.AddAsync(message);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<bool> UpdateMessageAsync(MessageRequestDto messageRequestDto, string accessToken)
    {
        var existingMessage = await GetMessageByUserId(accessToken);
        if (existingMessage is null)
            throw new Exception("User does not exist to update message.");

        // Map DTO to existing entity to preserve Id
        mapper.Map(messageRequestDto, existingMessage);
        existingMessage.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.MessageRepository.UpdateAsync(existingMessage);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<bool> DeleteMessageAsync(string accessToken)
    {
        var existingMessage = await GetMessageByUserId(accessToken);
        if (existingMessage is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.MessageRepository.DeleteAsync(existingMessage);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<int> GetNumberOfUnread(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var messages = await unitOfWork.MessageRepository.GetNumberOfUnread(userId);
        return messages;
    }

    private async Task<Message?> GetMessageByUserId(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        return await unitOfWork.MessageRepository.GetByUserIdAsync(userId);
    }
}