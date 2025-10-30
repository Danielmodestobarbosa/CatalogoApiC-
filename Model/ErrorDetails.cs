using System.Text.Json;

namespace CatalogoApiNovo.Model
{
    public class ErrorDetails
    {
        //Representa o código de status do erro 
        public int StatusCode { get; set; }

        //Mensagem descritiva relacionada ao ero
        public string? Message { get; set; }

        //Rastreamento da pila
        public string? Trace { get; set; }


        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
