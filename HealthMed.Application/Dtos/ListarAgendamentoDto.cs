namespace HealthMed.Api.Dtos
{
    public class ListarAgendamentoDto
    {
        public int Id { get; set; }
        public string Paciente { get; set; }
        public DateTime DataHora { get; set; }
        public bool Aprovado { get; set; }
    }

}
