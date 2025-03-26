using Application.DTOs.Message;
using Domain.Common;

namespace Application.Interfaces;

public interface IMessageService
{
    Task<Result<IEnumerable<MessageResponseDto>>> GetMessagesByUserIdAsync(string accessToken);
    Task<Result<bool>> AddMessageAsync(MessageRequestDto messageRequestDto, string accessToken);
    Task<Result<bool>> UpdateMessageAsync(UpdateMessageDto messageRequestDto, Guid id, string accessToken);
    Task<Result<bool>> DeleteMessageAsync(Guid id, string accessToken);
    Task<Result<int>> GetNumberOfUnread(string accessToken);
}