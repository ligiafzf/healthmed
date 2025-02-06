using System.Security.Claims;
using System.Text;
using HealthMed.Api.Interfaces;
using HealthMed.Api.Entities;
using HealthMed.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace HealthMed.Api.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IConfiguration _config;

    public AuthService(IAuthRepository authRepository, IPasswordHasher<Usuario> passwordHasher, IConfiguration config)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _config = config;
    }

    public async Task<string?> AutenticarMedico(string crm, string senha)
    {
        var medico = await _authRepository.ObterUsuarioPorCRM(crm);

        if (medico == null ||
            _passwordHasher.VerifyHashedPassword(medico, medico.SenhaHash, senha) != PasswordVerificationResult.Success)
        {
            return null;
        }

        return GerarTokenJWT(medico, crm);
    }

    public async Task<string?> AutenticarPaciente(string cpf, string senha)
    {
        var paciente = await _authRepository.ObterUsuarioPorCPF(cpf);

        if (paciente == null ||
            _passwordHasher.VerifyHashedPassword(paciente, paciente.SenhaHash, senha) != PasswordVerificationResult.Success)
        {
            return null;
        }

        return GerarTokenJWT(paciente, cpf);
    }

    private string GerarTokenJWT(Usuario usuario, string identificador)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Role),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim("Identificador", identificador)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_config["Jwt:ExpireHours"])),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = credentials
        };

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
    }
}
