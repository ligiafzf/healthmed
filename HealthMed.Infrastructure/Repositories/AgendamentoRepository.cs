using HealthMed.Api;
using HealthMed.Api.Entities;
using HealthMed.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infrastructure.Repositories;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly ApplicationDbContext _context;

    public AgendamentoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Agendamento>> Listar(int medicoId, StatusAgendamento status)
    {
        var query = _context.Agendamentos
            .Include(a => a.Paciente.Usuario)
            .Where(a => a.MedicoId == medicoId && !a.Cancelado)
            .AsQueryable();

        if (status == StatusAgendamento.Aceitos) query = query.Where(a => a.Aprovado);
        else if (status == StatusAgendamento.NaoAceitos) query = query.Where(a => !a.Aprovado);

        return await query.OrderBy(a => a.DataHora).ToListAsync();
    }

    public async Task<Agendamento?> ObterPorId(int id)
    {
        return await _context.Agendamentos.FindAsync(id);
    }

    public async Task<IEnumerable<DateTime>> ObterHorariosDisponiveis(int medicoId, DateTime data)
    {
        var agendamentos = await _context.Agendamentos
            .Where(a => a.MedicoId == medicoId && a.DataHora.Date == data.Date)
            .Select(a => a.DataHora)
            .ToListAsync();

        return agendamentos;
    }

    public async Task Criar(Agendamento agendamento)
    {
        _context.Agendamentos.Add(agendamento);
        await SalvarAlteracoes();
    }

        public async Task Remover(Agendamento agendamento)
    {
        _context.Agendamentos.Remove(agendamento);
        await SalvarAlteracoes();
    }

    public async Task Cancelar(Agendamento agendamento)
    {
        _context.Agendamentos.Update(agendamento);
        await SalvarAlteracoes();
    }

    public async Task SalvarAlteracoes()
    {
        await _context.SaveChangesAsync();
    }
}
