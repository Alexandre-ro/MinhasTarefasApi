using MinhasTarefasApi.Database;
using MinhasTarefasApi.Models;
using MinhasTarefasApi.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinhasTarefasApi.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private MinhasTarefasContext _banco;

        public TarefaRepository(MinhasTarefasContext banco)
        {
            _banco = banco;
        }

        public List<Tarefa> Restauracao(ApplicationUser usuario, DateTime dataUltimaSincronizacao)
        {
            var query = _banco.Tarefas.Where(a => a.UsuarioId == usuario.Id).AsQueryable();

            if (dataUltimaSincronizacao.ToString() != null)
            {
                query.Where(a => a.Criado >= dataUltimaSincronizacao || a.Atualizado >= dataUltimaSincronizacao);
            }

            return query.ToList<Tarefa>();
        }

        public List<Tarefa> Sincronizacao(List<Tarefa> tarefas)
        {
            var novasTarefas                  = tarefas.Where(a => a.IdTarefaApi == 0);
            var tarefasAtualizadasOuExcluidas = tarefas.Where(a => a.IdTarefaApi != 0).ToList();

            //Cadastrar novo registro
            if (novasTarefas.Count() > 0)
            {
                foreach (var tarefa in novasTarefas)
                {
                    _banco.Tarefas.Add(tarefa);
                }
            }

            //Atualização
            
            if (tarefasAtualizadasOuExcluidas.Count() > 0)
            {
                foreach (var tarefaAtualizada in tarefasAtualizadasOuExcluidas)
                {
                    _banco.Update(tarefaAtualizada);
                }

            }

            _banco.SaveChanges();

            return novasTarefas.ToList();
        }
    }
}
