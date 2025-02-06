using HealthMed.Api.Entities;
using HealthMed.Api.Services;
using HealthMed.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HealthMed.Tests.Application;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IPasswordHasher<Usuario>> _passwordHasherMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<Usuario>>();
        _configMock = new Mock<IConfiguration>();

        _configMock.Setup(cfg => cfg["Jwt:Key"]).Returns("chaveSuperSecreta12345678901234567890");
        _configMock.Setup(cfg => cfg["Jwt:ExpireHours"]).Returns("1");
        _configMock.Setup(cfg => cfg["Jwt:Issuer"]).Returns("testIssuer");
        _configMock.Setup(cfg => cfg["Jwt:Audience"]).Returns("testAudience");

        _authService = new AuthService(_authRepositoryMock.Object, _passwordHasherMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task AutenticarMedico_DeveRetornarNull_QuandoMedicoNaoExiste()
    {
        _authRepositoryMock.Setup(repo => repo.ObterUsuarioPorCRM("12345")).ReturnsAsync((Usuario)null);

        var result = await _authService.AutenticarMedico("12345", "senha");

        Assert.Null(result);
    }

    [Fact]
    public async Task AutenticarMedico_DeveRetornarToken_QuandoCredenciaisSaoValidas()
    {
        var medico = new Usuario { Id = 1, Nome = "Dr. Teste", Role = "Medico", SenhaHash = "hashedPassword" };

        _authRepositoryMock.Setup(repo => repo.ObterUsuarioPorCRM("12345")).ReturnsAsync(medico);
        _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(medico, medico.SenhaHash, "senha"))
                           .Returns(PasswordVerificationResult.Success);

        var result = await _authService.AutenticarMedico("12345", "senha");
        Assert.NotNull(result);
    }
}


