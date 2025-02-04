using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Xml;

public class MedicoService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public MedicoService(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    // 📌 **Cadastrar Médico**
    public async Task<bool> CadastrarMedico(CadastroMedicoDto dto)
    {
        if (_context.Usuarios.Any(u => u.Email == dto.Email))
            return false; // E-mail já cadastrado

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

        return true;
    }

    // 📌 **Obter Médico pelo ID do Usuário**
    public async Task<Medico> ObterMedicoPorUsuarioId(int usuarioId)
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);
    }

    // 📌 **Atualizar Dados do Médico**
    public async Task<bool> AtualizarMedico(int usuarioId, AtualizarMedicoDto model)
    {
        var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);
        if (medico == null) return false;

        medico.HorarioInicio = model.HorarioInicio;
        medico.HorarioFim = model.HorarioFim;

        await _context.SaveChangesAsync();
        return true;
    }

    // 📌 **Deletar Médico**
    public async Task<bool> DeletarMedico(int usuarioId)
    {
        var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);
        if (medico == null) return false;

        _context.Medicos.Remove(medico);

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
        if (usuario != null) _context.Usuarios.Remove(usuario);

        await _context.SaveChangesAsync();
        return true;
    }
}
