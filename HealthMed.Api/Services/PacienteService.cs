using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Api.Services;

public class PacienteService : IPacienteService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public PacienteService(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Cadastrar(CadastroPacienteDto model)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == model.Email))
            return false;

        using var transaction = await _context.Database.BeginTransactionAsync();

        var usuario = new Usuario
        {
            Nome = model.Nome,
            Email = model.Email,
            Role = "Paciente",
            SenhaHash = _passwordHasher.HashPassword(null, model.Senha)
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var paciente = new Paciente
        {
            UsuarioId = usuario.Id,
            CPF = model.CPF,
            Telefone = model.Telefone
        };

        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
        return true;
    }

    public async Task<Paciente?> ObterPorUsuarioId(int usuarioId)
    {
        return await _context.Pacientes
            .AsNoTracking()
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
    }

    public async Task<bool> Atualizar(int usuarioId, AtualizarPacienteDto model)
    {
        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        if (paciente == null) return false;

        paciente.Telefone = model.Telefone ?? paciente.Telefone;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Deletar(int usuarioId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        if (paciente == null) return false;

        _context.Pacientes.Remove(paciente);

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
        if (usuario != null) _context.Usuarios.Remove(usuario);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }
}
