using Microsoft.AspNetCore.Identity;
using MinhasTarefasApi.Models;
using MinhasTarefasApi.Repositories.Contracts;
using System;
using System.Text;

namespace MinhasTarefasApi.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioRepository(UserManager<ApplicationUser> userManager)
        {

            _userManager = userManager;
        }

        public ApplicationUser Obter(string email, string senha)
        {
            var usuario = _userManager.FindByEmailAsync(email).Result;

            if (_userManager.CheckPasswordAsync(usuario, senha).Result)
            {
                return usuario;
            }

            else
            {
                throw new Exception("Usuário não encontrado!");
            }
        }

        public void Cadastrar(ApplicationUser usuario, string senha)
        {
            var result = _userManager.CreateAsync(usuario, senha).Result;

            StringBuilder sb = new StringBuilder();

            foreach (var erro in result.Errors)
            {
                sb.Append(erro.Description);
            }

            if (!result.Succeeded)
            {
                throw new Exception($"Usuário não cadastrado! {sb.ToString()}");
            }
        }
    }
}
