using HealthMed.Api;
using HealthMed.Api.Entities;

namespace HealthMed.Domain.Repositories;

public interface IAgendamentoRepository
{
    Task<IEnumerable<Agendamento>> Listar(int medicoId, StatusAgendamento status);
    Task<Agendamento?> ObterPorId(int id);
    Task<IEnumerable<DateTime>> ObterHorariosDisponiveis(int medicoId, DateTime data);
    Task Criar(Agendamento agendamento);
    Task Remover(Agendamento agendamento);
    Task Cancelar(Agendamento agendamento);
    Task SalvarAlteracoes();
}
