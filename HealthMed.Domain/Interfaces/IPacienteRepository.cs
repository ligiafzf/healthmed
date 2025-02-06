using HealthMed.Api.Entities;

namespace HealthMed.Domain.Repositories;

public interface IPacienteRepository
{
    Task<Paciente?> ObterPorId(int id);
    Task<Paciente?> ObterPorUsuarioId(int usuarioId);
    Task<bool> ExisteCPF(string cpf);
    Task Adicionar(Paciente paciente);
    Task Atualizar(Paciente paciente);
    Task Remover(Paciente paciente);
    Task SalvarAlteracoes();
}
