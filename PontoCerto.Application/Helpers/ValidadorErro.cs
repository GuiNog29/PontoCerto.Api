using Microsoft.AspNetCore.Mvc;
using PontoCerto.Application.DTOs;

namespace PontoCerto.Application.Helpers
{
    public class ValidadorErro : IValidadorErro
    {
        public ObjectResult TratarErro(string acao, Exception ex)
        {
            return new ObjectResult(new ErroDto
            {
                Mensagem = $"Ocorreu um erro ao tentar {acao}: {ex.Message}",
                Detalhes = ex.StackTrace
            })
            {
                StatusCode = 500
            };
        }
    }

    public interface IValidadorErro
    {
        ObjectResult TratarErro(string acao, Exception ex);
    }
}
