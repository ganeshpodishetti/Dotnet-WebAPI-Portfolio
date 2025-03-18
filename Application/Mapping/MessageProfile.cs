using Application.DTOs.Message;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<MessageRequestDto, Message>();
        CreateMap<Message, MessageResponseDto>();
    }
}