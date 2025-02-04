using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/medicos")]
public class MedicoController : ControllerBase
{
    private readonly MedicoService _medicoService;
    private int _userId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    public MedicoController(MedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    [AllowAnonymous]
    [HttpPost("cadastrar")]
    public async Task<IActionResult> CadastrarMedico([FromBody] CadastroMedicoDto model)
    {
        var sucesso = await _medicoService.CadastrarMedico(model);
        if (!sucesso) return BadRequest("E-mail já cadastrado.");
        return Ok("Médico cadastrado com sucesso.");
    }

    [Authorize(Roles = "Medico")]
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        var medico = await _medicoService.ObterMedicoPorUsuarioId(_userId);
        if (medico == null) return NotFound("Médico não encontrado.");
        return Ok(medico);
    }

    [Authorize(Roles = "Medico")]
    [HttpPut("atualizar")]
    public async Task<IActionResult> AtualizarMedico([FromBody] AtualizarMedicoDto model)
    {
        var atualizado = await _medicoService.AtualizarMedico(_userId, model);
        if (!atualizado) return BadRequest("Erro ao atualizar.");
        return Ok("Dados atualizados com sucesso.");
    }

    [Authorize(Roles = "Medico")]
    [HttpDelete("deletar")]
    public async Task<IActionResult> DeletarMedico()
    {
        var deletado = await _medicoService.DeletarMedico(_userId);
        if (!deletado) return BadRequest("Erro ao deletar conta.");
        return Ok("Médico deletado com sucesso.");
    }
}
