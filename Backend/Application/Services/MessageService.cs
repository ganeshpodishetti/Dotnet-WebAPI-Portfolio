using Application.DTOs.Message;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class MessageService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService,
    ILogger<MessageService> logger) : IMessageService
{
    // get messages
    public async Task<Result<IEnumerable<MessageResponseDto>>> GetMessagesByUserIdAsync(string accessToken)
    {
        logger.LogInformation("Getting messages for user");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var messages = await unitOfWork.MessageRepository.GetAllByUserIdAsync(userId);

        var response = mapper.Map<IEnumerable<MessageResponseDto>>(messages);
        logger.LogInformation("Successfully retrieved messages for user {UserId}", userId);
        return Result<IEnumerable<MessageResponseDto>>.Success(response);
    }

    // get unread messages
    public async Task<Result<int>> GetNumberOfUnread(string accessToken)
    {
        logger.LogInformation("Getting number of unread messages");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var messages = await unitOfWork.MessageRepository.GetNumberOfUnread(userId);
        logger.LogInformation("User {UserId} has {Count} unread messages", userId, messages);
        return Result<int>.Success(messages);
    }

    // sent messages
    public async Task<Result<bool>> AddMessageAsync(MessageRequestDto messageRequestDto, string accessToken)
    {
        logger.LogInformation("Adding new message");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var message = mapper.Map<Message>(messageRequestDto);
        message.UserId = userId;
        message.SentAt = DateTime.UtcNow;

        var result = await unitOfWork.MessageRepository.AddAsync(message);
        if (!result)
        {
            logger.LogError("Failed to add message for user {UserId}", userId);
            return Result<bool>.Failure(MessageErrors.FailedToAddMessage(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully added message for user {UserId}", userId);
        return Result<bool>.Success(result);
    }

    // edit messages
    public async Task<Result<bool>> UpdateMessageAsync(UpdateMessageDto messageRequestDto, Guid messageId,
        string accessToken)
    {
        logger.LogInformation("Updating message {MessageId}", messageId);
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingMessage = await unitOfWork.MessageRepository.GetByUserIdAsync(userId, messageId);
        if (existingMessage is null)
        {
            logger.LogWarning("Message {MessageId} not found for user {UserId}", messageId, userId);
            return Result<bool>.Failure(MessageErrors.MessageNotBelongToUser(nameof(userId)));
        }

        // Map DTO to existing entity to preserve id
        mapper.Map(messageRequestDto, existingMessage);
        existingMessage.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.MessageRepository.UpdateAsync(existingMessage);
        if (!result)
        {
            logger.LogError("Failed to update message for user {UserId}", userId);
            return Result<bool>.Failure(MessageErrors.FailedToUpdateMessage(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully updated message {MessageId}", messageId);
        return Result<bool>.Success(result);
    }

    // delete messages
    public async Task<Result<bool>> DeleteMessageAsync(Guid messageId, string accessToken)
    {
        logger.LogInformation("Deleting message {MessageId}", messageId);
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingMessage = await unitOfWork.MessageRepository.GetByUserIdAsync(userId, messageId);
        if (existingMessage is null)
        {
            logger.LogWarning("Message {MessageId} not found for user {UserId}", messageId, userId);
            return Result<bool>.Failure(MessageErrors.MessageNotBelongToUser(nameof(userId)));
        }

        var result = await unitOfWork.MessageRepository.DeleteAsync(existingMessage);
        if (!result)
        {
            logger.LogError("Failed to delete message for user {UserId}", userId);
            return Result<bool>.Failure(MessageErrors.FailedToDeleteMessage(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully deleted message {MessageId}", messageId);
        return Result<bool>.Success(result);
    }
}