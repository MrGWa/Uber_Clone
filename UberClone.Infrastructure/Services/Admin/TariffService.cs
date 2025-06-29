//Added by tamar
using UberClone.Application.DTOs.Admin;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class TariffService
{
    private readonly AppDbContext _context;

    public TariffService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TariffDto> CreateOrUpdateAsync(CreateOrUpdateTariffDto dto)
    {
        var tariff = await _context.Tariffs.FirstOrDefaultAsync(t => t.Region == dto.Region);

        if (tariff == null)
        {
            tariff = new Tariff
            {
                Region = dto.Region,
                BaseFare = dto.BaseFare,
                PerMinute = dto.PerMinute,
                PerKilometer = dto.PerKilometer,
                SurgeMultiplier = dto.SurgeMultiplier
            };
            _context.Tariffs.Add(tariff);
        }
        else
        {
            tariff.BaseFare = dto.BaseFare;
            tariff.PerMinute = dto.PerMinute;
            tariff.PerKilometer = dto.PerKilometer;
            tariff.SurgeMultiplier = dto.SurgeMultiplier;
            tariff.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return new TariffDto
        {
            Id = tariff.Id,
            Region = tariff.Region,
            BaseFare = tariff.BaseFare,
            PerMinute = tariff.PerMinute,
            PerKilometer = tariff.PerKilometer,
            SurgeMultiplier = tariff.SurgeMultiplier,
            UpdatedAt = tariff.UpdatedAt
        };
    }

    public async Task<List<TariffDto>> GetAllTariffsAsync()
    {
        return await _context.Tariffs
            .Select(t => new TariffDto
            {
                Id = t.Id,
                Region = t.Region,
                BaseFare = t.BaseFare,
                PerMinute = t.PerMinute,
                PerKilometer = t.PerKilometer,
                SurgeMultiplier = t.SurgeMultiplier,
                UpdatedAt = t.UpdatedAt
            }).ToListAsync();
    }
}
