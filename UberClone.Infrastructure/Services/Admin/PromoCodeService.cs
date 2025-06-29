//Added by tamar
using UberClone.Application.DTOs.Admin;
using UberClone.Domain.Entities;
using UberClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace UberClone.Infrastructure.Services.Admin;

public class PromoCodeService
{
    private readonly AppDbContext _context;

    public PromoCodeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PromoCodeDto> CreatePromoCodeAsync(CreatePromoCodeDto dto)
    {
        var promo = new PromoCode
        {
            Code = dto.Code,
            DiscountPercent = dto.DiscountPercent,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            UsageLimit = dto.UsageLimit,
            IsActive = true
        };

        _context.PromoCodes.Add(promo);
        await _context.SaveChangesAsync();

        return MapToDto(promo);
    }

    public async Task<List<PromoCodeDto>> GetAllPromoCodesAsync()
    {
        var promos = await _context.PromoCodes.ToListAsync();
        return promos.Select(MapToDto).ToList();
    }

    public async Task DeactivatePromoCodeAsync(int promoId)
    {
        var promo = await _context.PromoCodes.FindAsync(promoId);
        if (promo == null) throw new Exception("Promo code not found.");

        promo.IsActive = false;
        await _context.SaveChangesAsync();
    }

    private static PromoCodeDto MapToDto(PromoCode promo)
    {
        return new PromoCodeDto
        {
            Id = promo.Id,
            Code = promo.Code,
            DiscountPercent = promo.DiscountPercent,
            StartDate = promo.StartDate,
            EndDate = promo.EndDate,
            UsageLimit = promo.UsageLimit,
            IsActive = promo.IsActive
        };
    }
}
