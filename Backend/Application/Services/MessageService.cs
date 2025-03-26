using Application.DTOs.Message;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class MessageService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : IMessageService
{
    // get messages
    public async Task<Result<IEnumerable<MessageResponseDto>>> GetMessagesByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var messages = await unitOfWork.MessageRepository.GetAllByUserIdAsync(userId);
        if (messages is null)
            return Result<IEnumerable<MessageResponseDto>>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to get messages", StatusCode.NotFound));

        var response = mapper.Map<IEnumerable<MessageResponseDto>>(messages);
        return Result<IEnumerable<MessageResponseDto>>.Success(response);
    }

    // sent messages
    public async Task<Result<bool>> AddMessageAsync(MessageRequestDto messageRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists", "User does not exist to add message",
                StatusCode.NotFound));

        var message = mapper.Map<Message>(messageRequestDto);
        message.UserId = existingUser.Id;
        message.SentAt = DateTime.UtcNow;

        var result = await unitOfWork.MessageRepository.AddAsync(message);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // edit messages
    public async Task<Result<bool>> UpdateMessageAsync(UpdateMessageDto messageRequestDto, Guid messageId,
        string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingMessage = await unitOfWork.MessageRepository.GetByUserIdAsync(userId, messageId);
        if (existingMessage is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists", "User does not exist to update message",
                StatusCode.NotFound));

        // Map DTO to existing entity to preserve id
        mapper.Map(messageRequestDto, existingMessage);
        existingMessage.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.MessageRepository.UpdateAsync(existingMessage);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // delete messages
    public async Task<Result<bool>> DeleteMessageAsync(Guid messageId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingMessage = await unitOfWork.MessageRepository.GetByUserIdAsync(userId, messageId);
        if (existingMessage is null)
            //throw new Exception("User does not exist to delete experience.");
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists", "User does not exist to delete message",
                StatusCode.NotFound));

        var result = await unitOfWork.MessageRepository.DeleteAsync(existingMessage);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // get unread messages
    public async Task<Result<int>> GetNumberOfUnread(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var messages = await unitOfWork.MessageRepository.GetNumberOfUnread(userId);
        return Result<int>.Success(messages);
    }
}