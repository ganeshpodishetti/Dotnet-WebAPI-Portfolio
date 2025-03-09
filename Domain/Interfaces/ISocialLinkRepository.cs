using Domain.Entities;

namespace Domain.Interfaces;

public interface ISocialLinkRepository
{
    Task<IEnumerable<SocialLink>> GetAll();
    Task<SocialLink?> GetById(Guid id);
    Task<SocialLink> Add(SocialLink socialLink);
    Task Update(SocialLink socialLink);
    Task Delete(SocialLink socialLink);
}
