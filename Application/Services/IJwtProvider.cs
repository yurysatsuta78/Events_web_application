﻿using Domain.Models;
using System.Security.Claims;

namespace Application.Services
{
    public interface IJwtProvider
    {
        string GenerateRefreshToken(Guid participantId, DateTime current);
        string GenerateToken(Participant participant, DateTime current);
        ClaimsPrincipal ValidateRefreshToken(string refreshToken);
    }
}