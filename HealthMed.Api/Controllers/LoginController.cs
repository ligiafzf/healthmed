using HealthMed.Api.Dtos;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Api.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("medico")]
    public async Task<IActionResult> LoginMedico([FromBody] LoginMedicoDto model)
    {
        if (string.IsNullOrEmpty(model.CRM) || string.IsNullOrEmpty(model.Senha))
            return BadRequest("CRM e Senha são obrigatórios.");

        var token = await _authService.AutenticarMedico(model.CRM, model.Senha);

        if (token == null)
            return Unauthorized("CRM ou senha inválidos.");

        return Ok(new { Token = token });
    }

    [HttpPost("paciente")]
    public async Task<IActionResult> LoginPaciente([FromBody] LoginPacienteDto model)
    {
        if (string.IsNullOrEmpty(model.CPF) || string.IsNullOrEmpty(model.Senha))
            return BadRequest("CPF e Senha são obrigatórios.");

        var token = await _authService.AutenticarPaciente(model.CPF, model.Senha);

        if (token == null)
            return Unauthorized("CPF ou senha inválidos.");

        return Ok(new { Token = token });
    }
}
