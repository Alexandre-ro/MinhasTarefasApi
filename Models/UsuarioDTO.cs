using System.ComponentModel.DataAnnotations;

namespace MinhasTarefasApi.Models
{
    public class UsuarioDTO
    {
        [Required]
        public string Nome { get; set; }
        
        [Required]
        [EmailAddress]
        public string  Email { get; set; }
        
        [Required]       
        public string Senha { get; set; }
        
        [Required]
        [Compare("Senha")]
        public string ConfirmacaoSenha { get; set; }
    }
}
