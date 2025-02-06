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
        List<DateTime> horariosDisponiveis = new List<DateTime>();
        DateTime agora = DateTime.Now;

        DateTime horarioAtual = data.Date.Add(HorarioInicio);
        if (horarioAtual < agora)
        {
            int minutosAjuste = 15 - (agora.Minute % 15);
            horarioAtual = agora.AddMinutes(minutosAjuste);
        }

        DateTime horarioFim = data.Date.Add(HorarioFim);

        while (horarioAtual < horarioFim)
        {
            if (horarioAtual.Hour == 12)
            {
                horarioAtual = horarioAtual.AddMinutes(60);
                continue;
            }

            bool horarioJaAgendado = agendamentos.Any(a => a.DataHora == horarioAtual && a.Aprovado);
            if (!horarioJaAgendado)
            {
                horariosDisponiveis.Add(horarioAtual);
            }

            horarioAtual = horarioAtual.AddMinutes(15);
        }

        return horariosDisponiveis;
    }

}
