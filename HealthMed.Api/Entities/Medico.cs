public class Medico
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string CRM { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan HorarioFim { get; set; }
    public Usuario Usuario { get; set; }
}
