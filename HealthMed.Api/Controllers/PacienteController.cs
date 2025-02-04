using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/pacientes")]
public class PacienteController : ControllerBase
{
    private readonly PacienteService _pacienteService;
    private int _userId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    public PacienteController(PacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    [AllowAnonymous]
    [HttpPost("cadastrar")]
    public async Task<IActionResult> CadastrarPaciente([FromBody] CadastroPacienteDto model)
    {
        var sucesso = await _pacienteService.CadastrarPaciente(model.Nome, model.Email, model.Senha, model.CPF, model.Telefone);
        if (!sucesso) return BadRequest("E-mail já cadastrado.");
        return Ok("Paciente cadastrado com sucesso.");
    }

    [Authorize(Roles = "Paciente")]
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil()
    {
        var paciente = await _pacienteService.ObterPacientePorUsuarioId(_userId);
        if (paciente == null) return NotFound("Paciente não encontrado.");
        return Ok(paciente);
    }
    [Authorize(Roles = "Paciente")]
    [HttpPut("atualizar")]
    public async Task<IActionResult> AtualizarPaciente([FromBody] AtualizarPacienteDto model)
    {
        var atualizado = await _pacienteService.AtualizarPaciente(_userId, model);
        if (!atualizado) return BadRequest("Erro ao atualizar.");
        return Ok("Dados atualizados com sucesso.");
    }

    [Authorize(Roles = "Paciente")]
    [HttpDelete("deletar")]
    public async Task<IActionResult> DeletarPaciente()
    {
        var deletado = await _pacienteService.DeletarPaciente(_userId);
        if (!deletado) return BadRequest("Erro ao deletar conta.");
        return Ok("Paciente deletado com sucesso.");
    }
}
