﻿namespace Contracts.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateTokenAsync(string Email, String Role);

        public Task<string> GenerateRefreshTokenAsync();

        public Task<bool> ValidateRefreshToken(string token);


    }
}
