using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;
using HealthMed.Api.Interfaces;
using HealthMed.Domain.Repositories;

namespace HealthMed.Api.Services;

public class AgendamentoService : IAgendamentoService
{
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IMedicoRepository _medicoRepository;
    private readonly IPacienteRepository _pacienteRepository;

    public AgendamentoService(IAgendamentoRepository agendamentoRepository, IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository)
    {
        _agendamentoRepository = agendamentoRepository;
        _medicoRepository = medicoRepository;
        _pacienteRepository = pacienteRepository;
    }

    public async Task<IEnumerable<ListarMedicoDto>> ListarMedicos()
    {
        var medicos = await _medicoRepository.ListarTodos();

        return medicos.Select(m => new ListarMedicoDto
        {
            UsuarioId = m.UsuarioId,
            Nome = m.Usuario.Nome,
            CRM = m.CRM,
            HorarioInicio = m.HorarioInicio,
            HorarioFim = m.HorarioFim
        }).ToList();
    }

    public async Task<IEnumerable<ListarAgendamentoDto>> Listar(int medicoId, StatusAgendamento status)
    {
        var agendamentos = await _agendamentoRepository.Listar(medicoId, status);

        return agendamentos.Select(a => new ListarAgendamentoDto
        {
            Id = a.Id,
            Paciente = a.Paciente.Usuario.Nome,
            DataHora = a.DataHora,
            Aprovado = a.Aprovado
        }).ToList();
    }

    public async Task<IEnumerable<DateTime>> ObterHorariosDisponiveis(int medicoId, DateTime data)
    {
        var medico = await _medicoRepository.ObterPorId(medicoId);
        if (medico == null)
            throw new Exception("Médico não encontrado.");

        var agendamentos = await _agendamentoRepository.Listar(medicoId, StatusAgendamento.Todos);

        return medico.ObterHorariosDisponiveis(agendamentos.ToList(), data);
    }

    public async Task Criar(CadastroAgendamentoDto dto)
    {
        var medico = await _medicoRepository.ObterPorId(dto.MedicoId);
        if (medico == null)
            throw new Exception("Médico não encontrado.");

        var paciente = await _pacienteRepository.ObterPorId(dto.PacienteId);
        if (paciente == null)
            throw new Exception("Paciente não encontrado.");

        if (dto.DataHora.TimeOfDay < medico.HorarioInicio || dto.DataHora.TimeOfDay >= medico.HorarioFim)
            throw new Exception("O horário de agendamento está fora do expediente do médico.");

        if (dto.DataHora.Hour == 12)
            throw new Exception("Médico não atende no horário de almoço.");

        var conflito = await _agendamentoRepository.Listar(dto.MedicoId, StatusAgendamento.Todos);
        if (conflito.Any(a => a.DataHora == dto.DataHora))
            throw new Exception("O horário solicitado já está ocupado.");

        var agendamento = new Agendamento(dto.MedicoId, dto.PacienteId, dto.DataHora);
        await _agendamentoRepository.Criar(agendamento);
    }

    public async Task Aceitar(int agendamentoId)
    {
        var agendamento = await _agendamentoRepository.ObterPorId(agendamentoId);
        if (agendamento == null)
            throw new Exception("Agendamento não encontrado.");

        agendamento.AceitarAgendamento();
        await _agendamentoRepository.SalvarAlteracoes();
    }

    public async Task Rejeitar(int agendamentoId)
    {
        var agendamento = await _agendamentoRepository.ObterPorId(agendamentoId);
        if (agendamento == null)
            throw new Exception("Agendamento não encontrado.");

        await _agendamentoRepository.Remover(agendamento);
    }
}
