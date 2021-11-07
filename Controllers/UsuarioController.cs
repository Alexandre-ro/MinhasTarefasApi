using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhasTarefasApi.Models;
using MinhasTarefasApi.Repositories.Contracts;
using System.Collections.Generic;

namespace MinhasTarefasApi.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioController(IUsuarioRepository usuarioRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _usuarioRepository = usuarioRepository;
            _signInManager     = signInManager;
            _userManager       = userManager;
        }

        public IActionResult Login([FromBody] UsuarioDTO usuarioDTO)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("ConfirmacaoSenha");

            if (ModelState.IsValid)
            {
                ApplicationUser usuario = _usuarioRepository.Obter(usuarioDTO.Email, usuarioDTO.Senha);

                if (usuario != null)
                {
                    //Login no Identity
                    _signInManager.SignInAsync(usuario, false);

                    //retornar token jwt
                    return Ok();
                }
                else
                {
                    return NotFound("Usuário não localizado!");
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        public ActionResult Cadastrar([FromBody] UsuarioDTO usuarioDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usuario = new ApplicationUser();
                usuario.FullName        = usuarioDTO.Nome;
                usuario.Email           = usuarioDTO.Email;

                var resultado = _userManager.CreateAsync(usuario, usuarioDTO.Senha).Result;

                if (!resultado.Succeeded)
                {
                    List<string> erros = new List<string>();

                    foreach (var erro in resultado.Errors)
                    {
                        erros.Add(erro.Description);
                    }

                    return UnprocessableEntity(erros);
                }

                else
                {
                    return Ok(usuario);
                }
            }

            else
            {
                return UnprocessableEntity(ModelState);
            }
        }
    }
}
