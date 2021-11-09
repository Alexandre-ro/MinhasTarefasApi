using MinhasTarefasApi.Database;
using MinhasTarefasApi.Models;
using MinhasTarefasApi.Repositories.Contracts;
using System;
using System.Linq;

namespace MinhasTarefasApi.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        public readonly MinhasTarefasContext _banco; 
        
        public TokenRepository(MinhasTarefasContext banco)
        {
            _banco = banco;
        }

        public Token Obter(string refreshToken)
        {
            return _banco.Token.FirstOrDefault(a => a.RefreshToken == refreshToken && a.Utilizado == false);
        }

        public void Cadastrar(Token token)
        {
            _banco.Token.Add(token);
            _banco.SaveChanges();
        }


        public void Atualizar(Token token)
        {
            _banco.Token.Update(token);
            _banco.SaveChanges();
        }
    }
}
