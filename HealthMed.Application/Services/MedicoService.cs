using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;
using HealthMed.Api.Interfaces;
using HealthMed.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HealthMed.Api.Services;

public class MedicoService : IMedicoService
{
    private readonly IMedicoRepository _medicoRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public MedicoService(IMedicoRepository medicoRepository, IAuthRepository authRepository, IPasswordHasher<Usuario> passwordHasher)
    {
        _medicoRepository = medicoRepository;
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Cadastrar(CadastroMedicoDto dto)
    {
        if (await _authRepository.ObterUsuarioPorEmail(dto.Email) != null)
            return false;

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Role = "Medico",
            SenhaHash = _passwordHasher.HashPassword(null, dto.Senha)
        };

        await _authRepository.AdicionarUsuario(usuario);

        var medico = new Medico
        {
            UsuarioId = usuario.Id,
            CRM = dto.CRM,
            HorarioInicio = dto.HorarioInicio,
            HorarioFim = dto.HorarioFim
        };

        await _medicoRepository.Adicionar(medico);

        return true;
    }

    public async Task<Medico?> ObterPorUsuarioId(int usuarioId)
    {
        return await _medicoRepository.ObterPorUsuarioId(usuarioId);
    }

    public async Task<bool> Atualizar(int usuarioId, AtualizarMedicoDto model)
    {
        var medico = await _medicoRepository.ObterPorUsuarioId(usuarioId);
        if (medico == null) return false;

        medico.HorarioInicio = model.HorarioInicio;
        medico.HorarioFim = model.HorarioFim;

        await _medicoRepository.Atualizar(medico);
        return true;
    }

    public async Task<bool> Deletar(int usuarioId)
    {
        var medico = await _medicoRepository.ObterPorUsuarioId(usuarioId);
        if (medico == null) return false;

        await _medicoRepository.Remover(medico);

        var usuario = await _authRepository.ObterUsuarioPorId(usuarioId);
        if (usuario != null) await _authRepository.SalvarAlteracoes();

        return true;
    }
}
