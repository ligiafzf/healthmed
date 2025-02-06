using HealthMed.Api.Entities;
using HealthMed.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterUsuarioPorEmail(string email)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> ObterUsuarioPorId(int id)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> ObterUsuarioPorCRM(string crm)
    {
        var medico = await _context.Medicos
            .Include(m => m.Usuario)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.CRM == crm);

        return medico?.Usuario;
    }

    public async Task<Usuario?> ObterUsuarioPorCPF(string cpf)
    {
        var paciente = await _context.Pacientes
            .Include(p => p.Usuario)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CPF == cpf);

        return paciente?.Usuario;
    }

    public async Task AdicionarUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await SalvarAlteracoes();
    }

    public async Task SalvarAlteracoes()
    {
        await _context.SaveChangesAsync();
    }
}
