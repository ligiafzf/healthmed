using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;
using HealthMed.Api.Interfaces;
using HealthMed.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HealthMed.Api.Services;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public PacienteService(IPacienteRepository pacienteRepository, IAuthRepository authRepository, IPasswordHasher<Usuario> passwordHasher)
    {
        _pacienteRepository = pacienteRepository;
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Cadastrar(CadastroPacienteDto model)
    {
        if (await _authRepository.ObterUsuarioPorEmail(model.Email) != null)
            return false;

        var usuario = new Usuario
        {
            Nome = model.Nome,
            Email = model.Email,
            Role = "Paciente",
            SenhaHash = _passwordHasher.HashPassword(null, model.Senha)
        };

        await _authRepository.AdicionarUsuario(usuario);

        var paciente = new Paciente
        {
            UsuarioId = usuario.Id,
            CPF = model.CPF,
            Telefone = model.Telefone
        };

        await _pacienteRepository.Adicionar(paciente);

        return true;
    }

    public async Task<Paciente?> ObterPorUsuarioId(int usuarioId)
    {
        return await _pacienteRepository.ObterPorUsuarioId(usuarioId);
    }

    public async Task<bool> Atualizar(int usuarioId, AtualizarPacienteDto model)
    {
        var paciente = await _pacienteRepository.ObterPorUsuarioId(usuarioId);
        if (paciente == null) return false;

        paciente.Telefone = model.Telefone ?? paciente.Telefone;

        await _pacienteRepository.Atualizar(paciente);
        return true;
    }

    public async Task<bool> Deletar(int usuarioId)
    {
        var paciente = await _pacienteRepository.ObterPorUsuarioId(usuarioId);
        if (paciente == null) return false;

        await _pacienteRepository.Remover(paciente);

        var usuario = await _authRepository.ObterUsuarioPorId(usuarioId);
        if (usuario != null) await _authRepository.SalvarAlteracoes();

        return true;
    }
}
