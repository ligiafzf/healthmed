using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;
using HealthMed.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Api.Services;

public class AgendamentoService : IAgendamentoService
{
    private readonly ApplicationDbContext _context;

    public AgendamentoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ListarMedicoDto>> ListarMedicos()
    {
        return await _context.Medicos
            .Include(x => x.Usuario)
            .Select(m => new ListarMedicoDto
            {
                UsuarioId = m.Id,
                Nome = m.Usuario.Nome,
                CRM = m.CRM,
                HorarioInicio = m.HorarioInicio,
                HorarioFim = m.HorarioFim
            })
            .ToListAsync();
    }


    public async Task<IEnumerable<DateTime>> ObterHorariosDisponiveis(int medicoId, DateTime data)
    {
        var medico = await _context.Medicos.FindAsync(medicoId);
        if (medico == null) throw new Exception("Médico não encontrado.");

        var agendamentos = await _context.Agendamentos
            .Where(a => a.MedicoId == medicoId && a.DataHora.Date == data.Date)
            .ToListAsync();

        return medico.ObterHorariosDisponiveis(agendamentos, data);
    }

    public async Task CriarAgendamento(int medicoId, int pacienteId, DateTime dataHora)
    {
        var medico = await _context.Medicos.FindAsync(medicoId);
        if (medico == null) throw new Exception("Médico não encontrado.");

        if (dataHora.Hour == 12) throw new Exception("Médico não atende no horário de almoço.");

        var horariosDisponiveis = await ObterHorariosDisponiveis(medicoId, dataHora.Date);
        if (!horariosDisponiveis.Contains(dataHora))
        {
            throw new Exception("Horário não disponível.");
        }

        var agendamento = new Agendamento(medicoId, pacienteId, dataHora);
        _context.Agendamentos.Add(agendamento);
        await _context.SaveChangesAsync();
    }

    public async Task AceitarAgendamento(int agendamentoId)
    {
        var agendamento = await _context.Agendamentos.FindAsync(agendamentoId);
        if (agendamento == null) throw new Exception("Agendamento não encontrado.");

        agendamento.AceitarAgendamento();
        await _context.SaveChangesAsync();
    }

}

