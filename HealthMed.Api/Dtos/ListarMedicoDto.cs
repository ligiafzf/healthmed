namespace HealthMed.Api.Dtos
{
    public class ListarMedicoDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string CRM { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
    }
}
