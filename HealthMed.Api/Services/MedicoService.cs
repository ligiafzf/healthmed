using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;
using HealthMed.Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Api.Services;

public class MedicoService : IMedicoService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public MedicoService(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> CadastrarMedico(CadastroMedicoDto dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return false;

        using var transaction = await _context.Database.BeginTransactionAsync();

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Role = "Medico",
            SenhaHash = _passwordHasher.HashPassword(null, dto.Senha)
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var medico = new Medico
        {
            UsuarioId = usuario.Id,
            CRM = dto.CRM,
            HorarioInicio = dto.HorarioInicio,
            HorarioFim = dto.HorarioFim
        };

        _context.Medicos.Add(medico);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
        return true;
    }

    public async Task<Medico?> ObterMedicoPorUsuarioId(int usuarioId)
    {
        return await _context.Medicos
            .AsNoTracking()
            .Include(m => m.Usuario)
            .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);
    }

    public async Task<bool> AtualizarMedico(int usuarioId, AtualizarMedicoDto model)
    {
        var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);
        if (medico == null) return false;

        medico.HorarioInicio = model.HorarioInicio;
        medico.HorarioFim = model.HorarioFim;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletarMedico(int usuarioId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);
        if (medico == null) return false;

        _context.Medicos.Remove(medico);

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
        if (usuario != null) _context.Usuarios.Remove(usuario);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return true;
    }
}
