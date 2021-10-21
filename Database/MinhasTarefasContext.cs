using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinhasTarefasApi.Models;

namespace MinhasTarefasApi.Database
{
    public class MinhasTarefasContext : IdentityDbContext<ApplicationUser>
    {
        public MinhasTarefasContext(DbContextOptions<MinhasTarefasContext> options) : base(options)
        {

        }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}
