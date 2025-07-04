using UberClone.Domain.Entities;

namespace UberClone.Application.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetTransactionByIdAsync(int transactionId);
    Task<List<Transaction>> GetTransactionsByRideIdAsync(Guid rideId);
    Task<Transaction> CreateTransactionAsync(Transaction transaction);
    Task UpdateTransactionAsync(Transaction transaction);
    Task SaveAsync(Transaction transaction);
}
