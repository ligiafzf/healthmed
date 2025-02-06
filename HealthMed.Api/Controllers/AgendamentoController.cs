using HealthMed.Api.Dtos;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController : ControllerBase
{
    private readonly IAgendamentoService _agendamentoService;

    public AgendamentoController(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    [Authorize(Roles = "Paciente")]
    [HttpGet("medicos")]
    public async Task<IActionResult> ListarMedicos()
    {
        var medicos = await _agendamentoService.ListarMedicos();
        return Ok(medicos);
    }

    [Authorize(Roles = "Medico")]
    [HttpGet("{medicoId}/agendamentos")]
    public async Task<IActionResult> ListarAgendamentos(int medicoId, [FromQuery] StatusAgendamento status = StatusAgendamento.Todos)
    {

        var agendamentos = await _agendamentoService.Listar(medicoId, status);
        return Ok(agendamentos);

    }

    [Authorize(Roles = "Paciente")]
    [HttpGet("{medicoId}/horarios-disponiveis")]
    public async Task<IActionResult> ObterHorariosDisponiveis(int medicoId, [FromQuery] DateTime data)
    {
        var horarios = await _agendamentoService.ObterHorariosDisponiveis(medicoId, data);
        return Ok(horarios);
    }

    [Authorize(Roles = "Paciente")]
    [HttpPost]
    public async Task<IActionResult> CriarAgendamento([FromBody] CadastroAgendamentoDto dto)
    {
        await _agendamentoService.Criar(dto);
        return Ok("Agendamento realizado com sucesso.");
    }

    [Authorize(Roles = "Medico")]
    [HttpPut("{id}/aceitar")]
    public async Task<IActionResult> Aceitar(int id)
    {
        await _agendamentoService.Aceitar(id);
        return Ok("Agendamento aceito.");
    }

    [Authorize(Roles = "Medico")]
    [HttpPut("{id}/rejeitar")]
    public async Task<IActionResult> Rejeitar(int id)
    {
        await _agendamentoService.Rejeitar(id);
        return Ok("Agendamento rejeitado.");
    }
}
