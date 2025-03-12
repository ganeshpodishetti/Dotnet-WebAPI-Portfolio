using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

internal class UserRepository(PortfolioDbContext context)
    : Repository<User>(context), IUserRepository
{
}