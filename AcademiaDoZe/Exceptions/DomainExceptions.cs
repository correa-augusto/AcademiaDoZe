//AUGUSTO DOS SANTOS CORREA
namespace AcademiaDoZe.Domain.Exceptions
{
    // classe base para exce��es de dom�nio
   // permitindo exce��es espec�ficas de regras de neg�cio
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}