using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

internal class UserRepository(PortfolioDbContext context)
    : GenericRepository<User>(context), IUserRepository
{
}