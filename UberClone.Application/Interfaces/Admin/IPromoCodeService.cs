using UberClone.Application.DTOs.Admin;

namespace UberClone.Application.Interfaces.Admin;

public interface IPromoCodeService
{
    Task<List<PromoCodeDto>> GetAllPromoCodesAsync();
    Task<PromoCodeDto?> GetPromoCodeByIdAsync(int id);
    Task<PromoCodeDto?> GetPromoCodeByCodeAsync(string code);
    Task<PromoCodeDto> CreatePromoCodeAsync(CreatePromoCodeDto dto);
    Task<PromoCodeDto> UpdatePromoCodeAsync(int id, CreatePromoCodeDto dto);
    Task DeletePromoCodeAsync(int id);
    Task DeactivatePromoCodeAsync(int id);
    Task<bool> ValidatePromoCodeAsync(string code);
}
