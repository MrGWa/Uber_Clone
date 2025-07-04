using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases.Admin;

public class ManageTariffUseCase(
    IRepository<Tariff> tariffRepository,
    IRepository<AuditLog> auditLogRepository) : IManageTariffUseCase
{
    private readonly IRepository<Tariff> _tariffRepository = tariffRepository;
    private readonly IRepository<AuditLog> _auditLogRepository = auditLogRepository;

    public async Task<TariffDto> CreateOrUpdateAsync(CreateOrUpdateTariffDto dto, string adminUserId)
    {
        // Business logic: Validate tariff data
        ValidateTariffData(dto);

        var existingTariff = await _tariffRepository.GetByConditionAsync(t => t.Region == dto.Region);
        
        Tariff tariff;
        string action;

        if (existingTariff != null)
        {
            // Update existing tariff
            existingTariff.BaseFare = dto.BaseFare;
            existingTariff.PerMinute = dto.PerMinute;
            existingTariff.PerKilometer = dto.PerKilometer;
            existingTariff.SurgeMultiplier = dto.SurgeMultiplier;
            existingTariff.UpdatedAt = DateTime.UtcNow;
            
            await _tariffRepository.UpdateAsync(existingTariff);
            tariff = existingTariff;
            action = "Update";
        }
        else
        {
            // Create new tariff
            tariff = new Tariff
            {
                Region = dto.Region,
                BaseFare = dto.BaseFare,
                PerMinute = dto.PerMinute,
                PerKilometer = dto.PerKilometer,
                SurgeMultiplier = dto.SurgeMultiplier,
                UpdatedAt = DateTime.UtcNow
            };
            
            await _tariffRepository.AddAsync(tariff);
            action = "Create";
        }

        // Create audit log
        var auditLog = new AuditLog
        {
            ActionType = $"{action} Tariff",
            TargetEntity = $"Tariff:{dto.Region}",
            PerformedBy = adminUserId,
            Details = $"Base: {dto.BaseFare}, Min: {dto.PerMinute}, Km: {dto.PerKilometer}, Surge: {dto.SurgeMultiplier}",
            Timestamp = DateTime.UtcNow
        };

        await _auditLogRepository.AddAsync(auditLog);

        return new TariffDto
        {
            Id = tariff.Id,
            Region = tariff.Region,
            BaseFare = tariff.BaseFare,
            PerMinute = tariff.PerMinute,
            PerKilometer = tariff.PerKilometer,
            SurgeMultiplier = tariff.SurgeMultiplier
        };
    }

    public async Task<List<TariffDto>> GetAllTariffsAsync()
    {
        var tariffs = await _tariffRepository.GetAllAsync();
        return tariffs.Select(t => new TariffDto
        {
            Id = t.Id,
            Region = t.Region,
            BaseFare = t.BaseFare,
            PerMinute = t.PerMinute,
            PerKilometer = t.PerKilometer,
            SurgeMultiplier = t.SurgeMultiplier
        }).ToList();
    }

    private void ValidateTariffData(CreateOrUpdateTariffDto dto)
    {
        if (dto.BaseFare < 0)
            throw new ArgumentException("Base fare cannot be negative.");
            
        if (dto.PerMinute < 0)
            throw new ArgumentException("Per minute rate cannot be negative.");
            
        if (dto.PerKilometer < 0)
            throw new ArgumentException("Per kilometer rate cannot be negative.");
            
        if (dto.SurgeMultiplier < 1)
            throw new ArgumentException("Surge multiplier must be at least 1.0.");
    }
}
