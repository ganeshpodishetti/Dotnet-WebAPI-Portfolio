using Application.DTOs.Message;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        // Dtos to Domain
        CreateMap<MessageRequestDto, Message>();
        CreateMap<UpdateMessageDto, Message>();

        // Domain to Dtos
        CreateMap<Message, MessageResponseDto>()
            .ConstructUsing(src => new MessageResponseDto(
                src.Id.ToString(),
                src.Name,
                src.Email,
                src.Content,
                src.SentAt.ToString(),
                src.IsRead)
            );
    }
}