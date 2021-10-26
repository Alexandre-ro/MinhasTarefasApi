using MinhasTarefasApi.Models;

namespace MinhasTarefasApi.Repositories.Contracts
{
    public interface IUsuarioRepository
    {
        void Cadastrar(ApplicationUser Usuario, string senha);
        ApplicationUser Obter(string email, string senha);
    }
}
