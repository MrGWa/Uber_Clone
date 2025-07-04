using Microsoft.EntityFrameworkCore;
using UberClone.Application.Interfaces;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;

namespace UberClone.Infrastructure.Repositories
{
    public class TransactionRepository(AppDbContext context) : ITransactionRepository
    {
        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            return await context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
        }

        public async Task<List<Transaction>> GetTransactionsByRideIdAsync(Guid rideId)
        {
            return await context.Transactions
                .Where(t => t.RideId == rideId)
                .ToListAsync();
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();
            return transaction;
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();
        }

        public async Task SaveAsync(Transaction transaction)
        {
            if (transaction.Id == 0)
            {
                context.Transactions.Add(transaction);
            }
            else
            {
                context.Transactions.Update(transaction);
            }
            await context.SaveChangesAsync();
        }
    }
}
