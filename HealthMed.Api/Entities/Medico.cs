using HealthMed.Api.Entities;

public class Medico
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string CRM { get; set; }
    public TimeSpan HorarioInicio { get; set; }
    public TimeSpan HorarioFim { get; set; }
    public Usuario Usuario { get; set; }

    public List<DateTime> ObterHorariosDisponiveis(List<Agendamento> agendamentos, DateTime data)
    {
        var horariosDisponiveis = new List<DateTime>();
        var agora = DateTime.Now;
        var horarioInicio = data.Date.Add(HorarioInicio);
        var horarioFim = data.Date.Add(HorarioFim);

        if (data.Date == agora.Date && horarioInicio < agora)
            horarioInicio = agora.AddMinutes(15 - (agora.Minute % 15));

        for (var horarioAtual = horarioInicio; horarioAtual < horarioFim; horarioAtual = horarioAtual.AddMinutes(15))
        {
            if (horarioAtual.Hour == 12)
            {
                horarioAtual = horarioAtual.AddHours(1);
                continue;
            }

            if (!agendamentos.Any(a => a.DataHora == horarioAtual))
                horariosDisponiveis.Add(horarioAtual);
        }

        return horariosDisponiveis;
    }
}
