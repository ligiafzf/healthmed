using HealthMed.Api.Entities;

namespace HealthMed.Domain.Repositories;

public interface IAuthRepository
{
    Task<Usuario?> ObterUsuarioPorEmail(string email);
    Task<Usuario?> ObterUsuarioPorId(int id);
    Task<Usuario?> ObterUsuarioPorCRM(string crm);
    Task<Usuario?> ObterUsuarioPorCPF(string cpf);
    Task AdicionarUsuario(Usuario usuario);
    Task SalvarAlteracoes();
}
