using UberClone.Domain.Entities;

namespace UberClone.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task<bool> IsEmailTakenAsync(string email);
}