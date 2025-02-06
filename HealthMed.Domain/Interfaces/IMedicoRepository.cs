using HealthMed.Api.Entities;

namespace HealthMed.Domain.Repositories;

public interface IMedicoRepository
{
    Task<Medico?> ObterPorId(int id);
    Task<Medico?> ObterPorUsuarioId(int usuarioId);
    Task<IEnumerable<Medico>> ListarTodos();
    Task<bool> ExisteCRM(string crm);
    Task Adicionar(Medico medico);
    Task Atualizar(Medico medico);
    Task Remover(Medico medico);
    Task SalvarAlteracoes();
}
