﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppUser = RO.DevTest.Domain.Entities.User;

namespace RO.DevTest.Application.Features.Auth.Commands.LoginCommand;
public class LoginCommandHandler(UserManager<AppUser> userManager, IConfiguration configuration) : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new LoginResult
        {
            AccessToken = tokenHandler.WriteToken(token),
            IssuedAt = DateTime.UtcNow,
            ExpirationDate = tokenDescriptor.Expires!.Value,
            Roles = [.. roles]
        };
    }
}
