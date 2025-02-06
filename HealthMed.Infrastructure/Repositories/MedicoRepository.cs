using HealthMed.Api.Entities;
using HealthMed.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Infrastructure.Repositories;

public class MedicoRepository : IMedicoRepository
{
    private readonly ApplicationDbContext _context;

    public MedicoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Medico?> ObterPorId(int id)
    {
        return await _context.Medicos.FindAsync(id);
    }

    public async Task<Medico?> ObterPorUsuarioId(int usuarioId)
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);
    }

    public async Task<IEnumerable<Medico>> ListarTodos()
    {
        return await _context.Medicos.Include(m => m.Usuario).ToListAsync();
    }

    public async Task<bool> ExisteCRM(string crm)
    {
        return await _context.Medicos.AnyAsync(m => m.CRM == crm);
    }

    public async Task Adicionar(Medico medico)
    {
        _context.Medicos.Add(medico);
        await SalvarAlteracoes();
    }

    public async Task Atualizar(Medico medico)
    {
        _context.Medicos.Update(medico);
        await SalvarAlteracoes();
    }

    public async Task Remover(Medico medico)
    {
        _context.Medicos.Remove(medico);
        await SalvarAlteracoes();
    }

    public async Task SalvarAlteracoes()
    {
        await _context.SaveChangesAsync();
    }
}
