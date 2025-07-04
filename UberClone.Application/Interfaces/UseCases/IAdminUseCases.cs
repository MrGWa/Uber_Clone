using UberClone.Application.DTOs.Admin;

namespace UberClone.Application.Interfaces.UseCases;

public interface ICreateSupportTicketUseCase
{
    Task<int> ExecuteAsync(CreateSupportTicketDto dto);
}

public interface IResolveSupportTicketUseCase
{
    Task<bool> ExecuteAsync(int ticketId, UpdateSupportTicketDto dto, string adminUserId);
}

public interface IManageTariffUseCase
{
    Task<TariffDto> CreateOrUpdateAsync(CreateOrUpdateTariffDto dto, string adminUserId);
    Task<List<TariffDto>> GetAllTariffsAsync();
}

public interface IManagePromoCodeUseCase
{
    Task<PromoCodeDto> CreatePromoCodeAsync(CreatePromoCodeDto dto, string adminUserId);
    Task<bool> DeactivatePromoCodeAsync(int promoCodeId, string adminUserId);
    Task<bool> ValidatePromoCodeForUseAsync(string code);
    Task<List<PromoCodeDto>> GetAllPromoCodesAsync();
}
