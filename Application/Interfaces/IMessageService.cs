using Application.DTOs.Message;

namespace Application.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<MessageResponseDto>> GetMessagesByUserIdAsync(string accessToken);
    Task<bool> AddMessageAsync(MessageRequestDto messageRequestDto, string accessToken);
    Task<bool> UpdateMessageAsync(MessageRequestDto messageRequestDto, string accessToken);
    Task<bool> DeleteMessageAsync(string accessToken);
    Task<int> GetNumberOfUnread(string accessToken);
}