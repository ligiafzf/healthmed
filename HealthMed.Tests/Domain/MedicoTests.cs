using HealthMed.Api.Entities;

namespace HealthMed.Tests.Domain;

public class MedicoTests
{
    [Fact]
    public void ObterHorariosDisponiveis_DeveRetornarHorariosValidos()
    {
        var medico = new Medico { HorarioInicio = new TimeSpan(8, 0, 0), HorarioFim = new TimeSpan(17, 0, 0) };
        var data = DateTime.Today.AddDays(1);
        var agendamentos = new List<Agendamento>
        {
            new Agendamento(1, 1, data.AddHours(9)),
            new Agendamento(1, 2, data.AddHours(10))
        };

        var horariosDisponiveis = medico.ObterHorariosDisponiveis(agendamentos, data);

        Assert.DoesNotContain(data.AddHours(9), horariosDisponiveis);
        Assert.DoesNotContain(data.AddHours(10), horariosDisponiveis);
        Assert.Contains(data.AddHours(8), horariosDisponiveis);
    }

    [Fact]
    public void ObterHorariosDisponiveis_DevePularHorarioAlmoco()
    {
        var medico = new Medico { HorarioInicio = new TimeSpan(8, 0, 0), HorarioFim = new TimeSpan(17, 0, 0) };
        var data = DateTime.Today.AddDays(1);
        var horariosDisponiveis = medico.ObterHorariosDisponiveis(new List<Agendamento>(), data);

        Assert.DoesNotContain(data.AddHours(12), horariosDisponiveis);
    }
}

