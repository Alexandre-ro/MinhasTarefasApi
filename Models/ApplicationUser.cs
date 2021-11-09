using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhasTarefasApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string  FullName{ get; set; }
       
        [ForeignKey("UsuarioId")]
        public virtual ICollection<Tarefa> Tarefas { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }
    }
}
