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

    [Authorize(Roles = "Paciente")]
    [HttpGet("medico/{medicoId}/horarios-disponiveis")]
    public async Task<IActionResult> ObterHorariosDisponiveis(int medicoId, [FromQuery] DateTime data)
    {
        var horarios = await _agendamentoService.ObterHorariosDisponiveis(medicoId, data);
        return Ok(horarios);
    }

    [Authorize(Roles = "Paciente")]
    [HttpPost]
    public async Task<IActionResult> CriarAgendamento([FromBody] CadastroAgendamentoDto dto)
    {
        await _agendamentoService.CriarAgendamento(dto.MedicoId, dto.PacienteId, dto.DataHora);
        return Ok("Agendamento realizado com sucesso.");
    }

    [Authorize(Roles = "Medico")]
    [HttpPut("{id}/aceitar")]
    public async Task<IActionResult> AceitarAgendamento(int id)
    {
        await _agendamentoService.AceitarAgendamento(id);
        return Ok("Agendamento aceito.");
    }
}
