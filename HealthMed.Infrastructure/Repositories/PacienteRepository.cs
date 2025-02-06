using HealthMed.Api.Entities;
using HealthMed.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly ApplicationDbContext _context;

    public PacienteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Paciente?> ObterPorId(int id)
    {
        return await _context.Pacientes.FindAsync(id);
    }

    public async Task<Paciente?> ObterPorUsuarioId(int usuarioId)
    {
        return await _context.Pacientes
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
    }

    public async Task<bool> ExisteCPF(string cpf)
    {
        return await _context.Pacientes.AnyAsync(p => p.CPF == cpf);
    }

    public async Task Adicionar(Paciente paciente)
    {
        _context.Pacientes.Add(paciente);
        await SalvarAlteracoes();
    }

    public async Task Atualizar(Paciente paciente)
    {
        _context.Pacientes.Update(paciente);
        await SalvarAlteracoes();
    }

    public async Task Remover(Paciente paciente)
    {
        _context.Pacientes.Remove(paciente);
        await SalvarAlteracoes();
    }

    public async Task SalvarAlteracoes()
    {
        await _context.SaveChangesAsync();
    }
}
