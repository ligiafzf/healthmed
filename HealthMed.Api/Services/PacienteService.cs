using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class PacienteService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public PacienteService(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> CadastrarPaciente(string nome, string email, string senha, string cpf, string telefone)
    {
        if (_context.Usuarios.Any(u => u.Email == email))
            return false; // E-mail já cadastrado

        var usuario = new Usuario
        {
            Nome = nome,
            Email = email,
            Role = "Paciente",
            SenhaHash = _passwordHasher.HashPassword(null, senha)
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var paciente = new Paciente
        {
            UsuarioId = usuario.Id,
            CPF = cpf,
            Telefone = telefone
        };

        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();

        return true;
    }

    // 📌 **Obter Paciente pelo ID do Usuário**
    public async Task<Paciente> ObterPacientePorUsuarioId(int usuarioId)
    {
        return await _context.Pacientes
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
    }

    // 📌 **Atualizar Dados do Paciente**
    public async Task<bool> AtualizarPaciente(int usuarioId, AtualizarPacienteDto model)
    {
        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        if (paciente == null) return false;

        paciente.Telefone = model.Telefone ?? paciente.Telefone;

        await _context.SaveChangesAsync();
        return true;
    }

    // 📌 **Deletar Paciente**
    public async Task<bool> DeletarPaciente(int usuarioId)
    {
        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        if (paciente == null) return false;

        _context.Pacientes.Remove(paciente);

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
        if (usuario != null) _context.Usuarios.Remove(usuario);

        await _context.SaveChangesAsync();
        return true;
    }
}
