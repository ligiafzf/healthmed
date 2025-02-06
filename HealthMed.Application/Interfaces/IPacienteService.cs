using HealthMed.Api.Dtos;
using HealthMed.Api.Entities;

namespace HealthMed.Api.Interfaces;

public interface IPacienteService
{
    Task<bool> Cadastrar(CadastroPacienteDto model);
    Task<Paciente?> ObterPorUsuarioId(int usuarioId);
    Task<bool> Atualizar(int usuarioId, AtualizarPacienteDto model);
    Task<bool> Deletar(int usuarioId);
}
