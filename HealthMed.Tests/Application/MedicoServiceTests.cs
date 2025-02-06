using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;
using HealthMed.Api.Services;
using HealthMed.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace HealthMed.Tests.Application;

public class MedicoServiceTests
{
    private readonly Mock<IMedicoRepository> _medicoRepositoryMock;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IPasswordHasher<Usuario>> _passwordHasherMock;
    private readonly MedicoService _medicoService;

    public MedicoServiceTests()
    {
        _medicoRepositoryMock = new Mock<IMedicoRepository>();
        _authRepositoryMock = new Mock<IAuthRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<Usuario>>();
        _medicoService = new MedicoService(_medicoRepositoryMock.Object, _authRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Cadastrar_DeveRetornarFalse_QuandoEmailJaExiste()
    {
        _authRepositoryMock.Setup(repo => repo.ObterUsuarioPorEmail("email@teste.com")).ReturnsAsync(new Usuario());
        var dto = new CadastroMedicoDto { Email = "email@teste.com", Nome = "Dr. Teste", CRM = "12345" };

        var result = await _medicoService.Cadastrar(dto);

        Assert.False(result);
    }
}
