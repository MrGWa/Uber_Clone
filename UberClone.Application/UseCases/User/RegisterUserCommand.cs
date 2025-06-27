using BCrypt.Net;
using UberClone.Application.DTOs;
using UberClone.Application.Interfaces;
using UberClone.Domain.Entities;

namespace UberClone.Application.UseCases.User;

public class RegisterUserCommand
{
    private readonly IUserRepository _repository;

    public RegisterUserCommand(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(RegisterUserDto dto)
    {
        if (await _repository.IsEmailTakenAsync(dto.Email))
            throw new Exception("Email already registered.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new Domain.Entities.User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        await _repository.AddAsync(user);
    }
}