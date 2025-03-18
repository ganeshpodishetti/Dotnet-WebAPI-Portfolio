using Domain.Entities;

namespace Domain.Interfaces;

public interface ISocialLinkRepository :
    IGenericRepository<SocialLink>
{
    //Task<SocialLink?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<SocialLink>?> GetAllByUserIdAsync(Guid userId);
}