using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinhasTarefasApi.Models;
using MinhasTarefasApi.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinhasTarefasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITokenRepository  tokenRepository)
        {
            _usuarioRepository = usuarioRepository;
            _signInManager     = signInManager;
            _userManager       = userManager;
            _tokenRepository   = tokenRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UsuarioDTO usuarioDTO)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("ConfirmacaoSenha");

            if (ModelState.IsValid)
            {
                ApplicationUser usuario = _usuarioRepository.Obter(usuarioDTO.Email, usuarioDTO.Senha);

                if (usuario != null)
                {
                    var token = BuildToken(usuario);

                    //Salvar o Token no Banco
                    var tokenModel   = new Token { 
                        RefreshToken = token.RefreshToken,
                        ExpirationRefreshToken = token.Expiration,
                        Usuario = usuario,
                        Criado = DateTime.Now,
                        Utilizado = false,
                    };

                    _tokenRepository.Cadastrar(tokenModel);
                    //retornar token jwt
                    
                    return Ok (token);
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

        public TokenDTO BuildToken(ApplicationUser usuario) 
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-api-jwt-private"));

            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var exp = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: exp,
                signingCredentials: sign
           );

            var refereshToken = Guid.NewGuid().ToString();
            var tokenString   = new JwtSecurityTokenHandler().WriteToken(token);
            var expRefresh    = DateTime.UtcNow.AddHours(2);
            var tokenDto      = new TokenDTO { Token = tokenString, 
                                               Expiration = exp, 
                                               RefreshToken = refereshToken, 
                                               ExpirationRefreshToken = expRefresh };

            return tokenDto;
        }

        [HttpPost("renovar")]
        public IActionResult Renovar([FromBody] TokenDTO tokenDTO) 
        {
            var refreshTokenDb = _tokenRepository.Obter(tokenDTO.RefreshToken);

            if (refreshTokenDb ==  null) 
            {
                return NotFound();
            }

            refreshTokenDb.Atualizado = DateTime.Now;
            refreshTokenDb.Utilizado = true;
            _tokenRepository.Atualizar(refreshTokenDb);

            var usuario = _usuarioRepository.Obter(refreshTokenDb.UsuarioId);
            var token   = BuildToken(usuario);

            //Salvar o Token no Banco
            var tokenModel = new Token
            {
                RefreshToken = token.RefreshToken,
                ExpirationRefreshToken = token.Expiration,
                Usuario = usuario,
                Criado = DateTime.Now,
                Utilizado = false,
            };

            _tokenRepository.Cadastrar(tokenModel);

            return Ok(token);
        }

        [HttpPost("")]
        public ActionResult Cadastrar([FromBody] UsuarioDTO usuarioDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usuario = new ApplicationUser();
                usuario.FullName        = usuarioDTO.Nome;
                usuario.UserName        = usuarioDTO.Email;
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
