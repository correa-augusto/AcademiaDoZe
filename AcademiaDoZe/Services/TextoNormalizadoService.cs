//AUGUSTO DOS SANTOS CORREA
using System.Text.RegularExpressions;
namespace AcademiaDoZe.Domain.Services
{
    public static partial class TextoNormalizadoService
    {
        // verifica se o texto � nulo ou vazio
        public static bool TextoVazioOuNulo(string? texto) => string.IsNullOrWhiteSpace(texto);
        // remove espa�os repetidos e espa�os no in�cio e no final do texto
        public static string LimparEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : EspacosRegex().Replace(texto, " ").Trim();
        // limpa todos os espa�os
        public static string LimparTodosEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : texto.Replace(" ", string.Empty);
        // converte o texto para mai�sculo
        public static string ParaMaiusculo(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : texto.ToUpperInvariant();
        // manter somente digitos numericos
        public static string LimparEDigitos(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : new string([.. texto.Where(char.IsDigit)]);
        // validar se email cont�m @ e ponto
        public static bool ValidarFormatoEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return !email.Contains('@') || !email.Contains('.');
        }
        // validar formato da senha - m�nimo 6 caracteres, pelo menos uma letra mai�scula
        public static bool ValidarFormatoSenha(string? senha)
        {
            if (string.IsNullOrWhiteSpace(senha)) return true;
            return senha.Length < 6 || !senha.Any(char.IsUpper);
        }
        [GeneratedRegex(@"\s+")]
        private static partial Regex EspacosRegex();
    }
}