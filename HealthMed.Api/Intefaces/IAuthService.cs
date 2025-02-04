using System.Threading.Tasks;

public interface IAuthService
{
    Task<string> AutenticarMedico(string crm, string senha);
    Task<string> AutenticarPaciente(string cpf, string senha);
}
