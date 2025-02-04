using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher, IConfiguration config)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _config = config;
    }

    public async Task<string> AutenticarMedico(string crm, string senha)
    {
        var medico = await _context.Medicos
            .Include(m => m.Usuario)
            .SingleOrDefaultAsync(m => m.CRM == crm);

        if (medico == null || 
            _passwordHasher.VerifyHashedPassword(medico.Usuario, medico.Usuario.SenhaHash, senha) != PasswordVerificationResult.Success)
        {
            return null;
        }

        return GerarTokenJWT(medico.Usuario, medico.CRM);
    }

    public async Task<string> AutenticarPaciente(string cpf, string senha)
    {
        var paciente = await _context.Pacientes
            .Include(p => p.Usuario)
            .SingleOrDefaultAsync(p => p.CPF == cpf);

        if (paciente == null || 
            _passwordHasher.VerifyHashedPassword(paciente.Usuario, paciente.Usuario.SenhaHash, senha) != PasswordVerificationResult.Success)
        {
            return null;
        }

        return GerarTokenJWT(paciente.Usuario, paciente.CPF);
    }

    private string GerarTokenJWT(Usuario usuario, string identificador)
    {
        // Valida se a chave JWT foi configurada
        var chaveJwt = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(chaveJwt))
            throw new InvalidOperationException("A chave JWT não foi configurada corretamente.");

        // Converte a chave para bytes e cria a chave simétrica
        var key = Encoding.UTF8.GetBytes(chaveJwt);
        var securityKey = new SymmetricSecurityKey(key);

        // Define as credenciais de assinatura
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Cria a lista de claims com as informações do usuário
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.Role, usuario.Role), // Ex: "Medico" ou "Paciente"
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim("Identificador", identificador) // Ex: CRM para médicos, CPF para pacientes
    };

        // Define as propriedades do token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_config["Jwt:ExpireHours"])),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = credentials
        };

        // Cria e retorna o token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
