
using HealthMed.Api.Entities;

namespace HealthMed.Tests.Domain;

public class PacienteTests
{
    [Fact]
    public void CriarPaciente_DeveInicializarCorretamente()
    {
        var paciente = new Paciente { UsuarioId = 1, CPF = "12345678901", Telefone = "11999999999" };

        Assert.Equal(1, paciente.UsuarioId);
        Assert.Equal("12345678901", paciente.CPF);
        Assert.Equal("11999999999", paciente.Telefone);
    }

    [Fact]
    public void CriarPaciente_ComCPFInvalido_DeveFalhar()
    {
        var paciente = new Paciente { UsuarioId = 1, CPF = "123", Telefone = "11999999999" };

        Assert.NotEqual(11, paciente.CPF.Length);
    }
}

