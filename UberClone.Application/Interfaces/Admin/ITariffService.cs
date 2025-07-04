using UberClone.Application.DTOs.Admin;

namespace UberClone.Application.Interfaces.Admin;

public interface ITariffService
{
    Task<List<TariffDto>> GetAllTariffsAsync();
    Task<TariffDto?> GetTariffByIdAsync(int id);
    Task<TariffDto> CreateTariffAsync(CreateOrUpdateTariffDto dto);
    Task<TariffDto> CreateOrUpdateAsync(CreateOrUpdateTariffDto dto);
    Task<TariffDto> UpdateTariffAsync(int id, CreateOrUpdateTariffDto dto);
    Task DeleteTariffAsync(int id);
}
