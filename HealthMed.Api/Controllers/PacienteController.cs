using HealthMed.Api.Dtos;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthMed.Api.Controllers;

[ApiController]
[Route("api/pacientes")]
public class PacienteController : ControllerBase
{
    private readonly IPacienteService _pacienteService;
    private int _userId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    public PacienteController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    [AllowAnonymous]
    [HttpPost("cadastrar")]
    public async Task<IActionResult> CadastrarPaciente([FromBody] CadastroPacienteDto model)
    {
        var sucesso = await _pacienteService.Cadastrar(model);
        if (!sucesso) return BadRequest("E-mail já cadastrado.");
        return Ok("Paciente cadastrado com sucesso.");
    }

    [Authorize(Roles = "Paciente")]
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        var paciente = await _pacienteService.ObterPorUsuarioId(_userId);
        if (paciente == null) return NotFound("Paciente não encontrado.");
        return Ok(paciente);
    }
    [Authorize(Roles = "Paciente")]
    [HttpPut("atualizar")]
    public async Task<IActionResult> AtualizarPaciente([FromBody] AtualizarPacienteDto model)
    {
        var atualizado = await _pacienteService.Atualizar(_userId, model);
        if (!atualizado) return BadRequest("Erro ao atualizar.");
        return Ok("Dados atualizados com sucesso.");
    }

    [Authorize(Roles = "Paciente")]
    [HttpDelete("deletar")]
    public async Task<IActionResult> DeletarPaciente()
    {
        var deletado = await _pacienteService.Deletar(_userId);
        if (!deletado) return BadRequest("Erro ao deletar conta.");
        return Ok("Paciente deletado com sucesso.");
    }
}
