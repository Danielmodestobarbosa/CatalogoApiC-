using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogoApiNovo.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        //Context contém informações sobre a exceção
        public void OnException(ExceptionContext context)
        {
            //Logando a exceção a nível de erro
            _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada: StatusCode : 500");

            //Definindo o resultado da exceção 
            context.Result = new ObjectResult("Ocorreu um problema ao tratar a sua solicitação: StatusCode : 500")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
