using HealthMed.Api.Dtos;

namespace HealthMed.Api.Interfaces;

public interface IMedicoService
{
    Task<bool> Cadastrar(CadastroMedicoDto dto);
    Task<Medico?> ObterPorUsuarioId(int usuarioId);
    Task<bool> Atualizar(int usuarioId, AtualizarMedicoDto model);
    Task<bool> Deletar(int usuarioId);
}
