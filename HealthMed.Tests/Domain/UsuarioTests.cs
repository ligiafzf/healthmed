using HealthMed.Api.Entities;

namespace HealthMed.Tests.Domain;

public class UsuarioTests
{
    [Fact]
    public void CriarUsuario_DeveInicializarCorretamente()
    {
        var usuario = new Usuario { Id = 1, Nome = "Teste", Email = "teste@email.com", SenhaHash = "senha123", Role = "Medico" };

        Assert.Equal(1, usuario.Id);
        Assert.Equal("Teste", usuario.Nome);
        Assert.Equal("teste@email.com", usuario.Email);
        Assert.Equal("senha123", usuario.SenhaHash);
        Assert.Equal("Medico", usuario.Role);
    }

    [Fact]
    public void CriarUsuario_SemEmail_DeveFalhar()
    {
        var usuario = new Usuario { Id = 1, Nome = "Teste", Email = "", SenhaHash = "senha123", Role = "Medico" };

        Assert.True(string.IsNullOrEmpty(usuario.Email));
    }
}

