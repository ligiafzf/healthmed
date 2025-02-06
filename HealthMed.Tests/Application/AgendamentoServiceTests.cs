using HealthMed.Api.Dtos;
using HealthMed.Api.Services;
using HealthMed.Domain.Repositories;
using Moq;

namespace HealthMed.Tests.Application;

public class AgendamentoServiceTests
{
    private readonly Mock<IAgendamentoRepository> _agendamentoRepositoryMock;
    private readonly Mock<IMedicoRepository> _medicoRepositoryMock;
    private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
    private readonly AgendamentoService _agendamentoService;

    public AgendamentoServiceTests()
    {
        _agendamentoRepositoryMock = new Mock<IAgendamentoRepository>();
        _medicoRepositoryMock = new Mock<IMedicoRepository>();
        _pacienteRepositoryMock = new Mock<IPacienteRepository>();

        _agendamentoService = new AgendamentoService(
            _agendamentoRepositoryMock.Object,
            _medicoRepositoryMock.Object,
            _pacienteRepositoryMock.Object);
    }

    [Fact]
    public async Task Criar_DeveLancarExcecao_QuandoMedicoNaoExiste()
    {
        _medicoRepositoryMock.Setup(repo => repo.ObterPorId(1)).ReturnsAsync((Medico)null);
        var dto = new CadastroAgendamentoDto { MedicoId = 1, PacienteId = 1, DataHora = DateTime.Now };

        await Assert.ThrowsAsync<Exception>(() => _agendamentoService.Criar(dto));
    }
}

