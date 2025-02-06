using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;
using HealthMed.Api.Services;
using HealthMed.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;


namespace HealthMed.Tests.Application;

public class PacienteServiceTests
{
    private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IPasswordHasher<Usuario>> _passwordHasherMock;
    private readonly PacienteService _pacienteService;

    public PacienteServiceTests()
    {
        _pacienteRepositoryMock = new Mock<IPacienteRepository>();
        _authRepositoryMock = new Mock<IAuthRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<Usuario>>();
        _pacienteService = new PacienteService(_pacienteRepositoryMock.Object, _authRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Cadastrar_DeveRetornarFalse_QuandoEmailJaExiste()
    {
        _authRepositoryMock.Setup(repo => repo.ObterUsuarioPorEmail("paciente@teste.com")).ReturnsAsync(new Usuario());
        var dto = new CadastroPacienteDto { Email = "paciente@teste.com", Nome = "Paciente Teste", CPF = "11111111111" };

        var result = await _pacienteService.Cadastrar(dto);

        Assert.False(result);
    }
}

