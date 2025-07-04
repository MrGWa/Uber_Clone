using UberClone.Application.DTOs;

namespace UberClone.Application.Interfaces.UseCases;

public interface IRegisterUserCommand
{
    Task ExecuteAsync(RegisterUserDto dto);
}
