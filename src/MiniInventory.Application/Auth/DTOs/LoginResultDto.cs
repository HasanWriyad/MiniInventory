namespace MiniInventory.Application.Auth.DTOs;

public record LoginResultDto(string Token, string Username, string Role);
