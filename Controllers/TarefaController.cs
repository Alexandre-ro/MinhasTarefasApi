using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinhasTarefasApi.Models;
using MinhasTarefasApi.Repositories;
using MinhasTarefasApi.Repositories.Contracts;
using System;
using System.Collections.Generic;

namespace MinhasTarefasApi.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TarefaController(TarefaRepository tarefaRepository, UserManager<ApplicationUser> userManager)
        {
            _tarefaRepository = tarefaRepository;
            _userManager      = userManager;
        }

        public ActionResult Sincronizar([FromBody] List<Tarefa> tarefas) 
        {
            return Ok(_tarefaRepository.Sincronizacao(tarefas));
        }

        public ActionResult Restaurar(DateTime data) 
        {
            var usuario = _userManager.GetUserAsync(HttpContext.User).Result;

            return Ok(usuario);
        }
       
    }
}
