using Domain.Entities;

namespace Domain.Interfaces;

public interface ISocialLinkRepository :
    IGenericRepository<SocialLink>
{
    Task<IEnumerable<SocialLink>?> GetAllByUserIdAsync(Guid userId);
}