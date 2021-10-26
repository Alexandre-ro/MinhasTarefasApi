using MinhasTarefasApi.Models;
using System;
using System.Collections.Generic;

namespace MinhasTarefasApi.Repositories.Contracts
{
    public interface ITarefaRepository
    {
        List<Tarefa> Sincronizacao(List<Tarefa> tarefas);
        List<Tarefa> Restauracao(ApplicationUser usuario, DateTime dataUltimaSincronizacao);
    }
}
