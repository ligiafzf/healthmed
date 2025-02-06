namespace HealthMed.Api.Entities;
public class Paciente
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string CPF { get; set; }
    public string Telefone { get; set; }
    public Usuario Usuario { get; set; }
}
