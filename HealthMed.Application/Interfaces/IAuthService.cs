namespace HealthMed.Api.Interfaces;

public interface IAuthService
{
    Task<string?> AutenticarMedico(string crm, string senha);
    Task<string?> AutenticarPaciente(string cpf, string senha);
}
