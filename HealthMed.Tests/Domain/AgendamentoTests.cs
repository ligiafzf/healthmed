using HealthMed.Api.Entities;

namespace HealthMed.Tests.Domain;

public class AgendamentoTests
{
    [Fact]
    public void CriarAgendamento_DeveInicializarCorretamente()
    {
        var agendamento = new Agendamento(1, 2, DateTime.Now);

        Assert.Equal(1, agendamento.MedicoId);
        Assert.Equal(2, agendamento.PacienteId);
        Assert.False(agendamento.Aprovado);
    }

    [Fact]
    public void AceitarAgendamento_DeveMarcarComoAprovado()
    {
        var agendamento = new Agendamento(1, 2, DateTime.Now);

        agendamento.AceitarAgendamento();

        Assert.True(agendamento.Aprovado);
    }
}

