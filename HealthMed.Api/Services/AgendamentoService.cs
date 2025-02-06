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
            .Include(m => m.Usuario)
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

    public async Task<IEnumerable<ListarAgendamentoDto>> Listar(int medicoId, StatusAgendamento status)
    {
        var query = _context.Agendamentos
            .Include(a => a.Paciente.Usuario)
            .Where(a => a.MedicoId == medicoId)
            .AsQueryable();

        if (status == StatusAgendamento.Aceitos) query = query.Where(a => a.Aprovado);
        else if (status == StatusAgendamento.NaoAceitos) query = query.Where(a => !a.Aprovado);

        return await query
            .OrderBy(a => a.DataHora)
            .Select(a => new ListarAgendamentoDto
            {
                Id = a.Id,
                Paciente = a.Paciente.Usuario.Nome,
                DataHora = a.DataHora,
                Aprovado = a.Aprovado
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<DateTime>> ObterHorariosDisponiveis(int medicoId, DateTime data)
    {
        var medico = await _context.Medicos.FindAsync(medicoId) ?? throw new Exception("Médico não encontrado.");
        var agendamentos = await _context.Agendamentos
            .Where(a => a.MedicoId == medicoId && a.DataHora.Date == data.Date)
            .ToListAsync();

        return medico.ObterHorariosDisponiveis(agendamentos, data);
    }

    public async Task Criar(CadastroAgendamentoDto dto)
    {
        var medico = await _context.Medicos.FindAsync(dto.MedicoId) ?? throw new Exception("Médico não encontrado.");
        var paciente = await _context.Pacientes.FindAsync(dto.PacienteId) ?? throw new Exception("Paciente não encontrado.");

        if (dto.DataHora.TimeOfDay < medico.HorarioInicio || dto.DataHora.TimeOfDay >= medico.HorarioFim)
            throw new Exception("O horário de agendamento está fora do expediente do médico.");

        if (dto.DataHora.Hour == 12)
            throw new Exception("Médico não atende no horário de almoço.");

        if (await _context.Agendamentos.AnyAsync(a => a.MedicoId == dto.MedicoId && a.DataHora == dto.DataHora))
            throw new Exception("O horário solicitado já está ocupado.");

        _context.Agendamentos.Add(new Agendamento(dto.MedicoId, dto.PacienteId, dto.DataHora));
        await _context.SaveChangesAsync();
    }

    public async Task Aceitar(int agendamentoId)
    {
        var agendamento = await _context.Agendamentos.FindAsync(agendamentoId) ?? throw new Exception("Agendamento não encontrado.");
        agendamento.AceitarAgendamento();
        await _context.SaveChangesAsync();
    }

    public async Task Rejeitar(int agendamentoId)
    {
        var agendamento = await _context.Agendamentos.FindAsync(agendamentoId) ?? throw new Exception("Agendamento não encontrado.");
        _context.Agendamentos.Remove(agendamento);
        await _context.SaveChangesAsync();
    }
}
