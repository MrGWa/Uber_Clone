using UberClone.Application.DTOs.Admin;
using UberClone.Application.Interfaces;
using UberClone.Application.Interfaces.UseCases;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases.Admin;

public class ManagePromoCodeUseCase(
    IRepository<PromoCode> promoCodeRepository,
    IRepository<AuditLog> auditLogRepository) : IManagePromoCodeUseCase
{
    private readonly IRepository<PromoCode> _promoCodeRepository = promoCodeRepository;
    private readonly IRepository<AuditLog> _auditLogRepository = auditLogRepository;

    public async Task<PromoCodeDto> CreatePromoCodeAsync(CreatePromoCodeDto dto, string adminUserId)
    {
        // Business logic: Validate promo code
        await ValidatePromoCode(dto);

        var promoCode = new PromoCode
        {
            Code = dto.Code.ToUpper(),
            DiscountPercent = dto.DiscountPercent,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            UsageLimit = dto.UsageLimit,
            IsActive = true
        };

        await _promoCodeRepository.AddAsync(promoCode);

        // Create audit log
        var auditLog = new AuditLog
        {
            ActionType = "Create Promo Code",
            TargetEntity = $"PromoCode:{dto.Code}",
            PerformedBy = adminUserId,
            Details = $"Code: {dto.Code}, Discount: {dto.DiscountPercent}%",
            Timestamp = DateTime.UtcNow
        };

        await _auditLogRepository.AddAsync(auditLog);

        return MapToDto(promoCode);
    }

    public async Task<bool> DeactivatePromoCodeAsync(int promoCodeId, string adminUserId)
    {
        var promoCode = await _promoCodeRepository.GetByIdAsync(promoCodeId);
        if (promoCode == null)
            throw new Exception("Promo code not found.");

        promoCode.IsActive = false;
        await _promoCodeRepository.UpdateAsync(promoCode);

        // Create audit log
        var auditLog = new AuditLog
        {
            ActionType = "Deactivate Promo Code",
            TargetEntity = $"PromoCode:{promoCode.Code}",
            PerformedBy = adminUserId,
            Details = $"Promo code {promoCode.Code} deactivated",
            Timestamp = DateTime.UtcNow
        };

        await _auditLogRepository.AddAsync(auditLog);
        return true;
    }

    public async Task<List<PromoCodeDto>> GetAllPromoCodesAsync()
    {
        var promoCodes = await _promoCodeRepository.GetAllAsync();
        return promoCodes.Select(MapToDto).ToList();
    }

    public async Task<bool> ValidatePromoCodeForUseAsync(string code)
    {
        var promoCode = await _promoCodeRepository.GetByConditionAsync(p => p.Code == code.ToUpper());
        
        if (promoCode == null || !promoCode.IsActive)
            return false;

        if (promoCode.EndDate < DateTime.UtcNow)
            return false;

        if (promoCode.StartDate > DateTime.UtcNow)
            return false;

        return true;
    }

    private async Task ValidatePromoCode(CreatePromoCodeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Code))
            throw new ArgumentException("Promo code cannot be empty.");

        if (dto.Code.Length < 3)
            throw new ArgumentException("Promo code must be at least 3 characters long.");

        // Check if code already exists
        var existingCode = await _promoCodeRepository.GetByConditionAsync(p => p.Code == dto.Code.ToUpper());
        if (existingCode != null)
            throw new ArgumentException("Promo code already exists.");

        if (dto.DiscountPercent <= 0 || dto.DiscountPercent > 100)
            throw new ArgumentException("Discount percentage must be between 1 and 100.");

        if (dto.StartDate >= dto.EndDate)
            throw new ArgumentException("Start date must be before end date.");

        if (dto.EndDate <= DateTime.UtcNow)
            throw new ArgumentException("End date must be in the future.");
    }

    private PromoCodeDto MapToDto(PromoCode promoCode)
    {
        return new PromoCodeDto
        {
            Id = promoCode.Id,
            Code = promoCode.Code,
            DiscountPercent = promoCode.DiscountPercent,
            StartDate = promoCode.StartDate,
            EndDate = promoCode.EndDate,
            UsageLimit = promoCode.UsageLimit,
            IsActive = promoCode.IsActive
        };
    }
}
