﻿namespace UberClone.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = "Passenger";

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}