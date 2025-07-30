//AUGUSTO DOS SANTOS CORREA
using System.Text.RegularExpressions;
namespace AcademiaDoZe.Domain.Services
{
    public static partial class TextoNormalizadoService
    {
        // Remove espa�os repetidos e espa�os no in�cio e no final do texto
        public static string LimparEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : EspacosRegex().Replace(texto, " ").Trim();
        // Limpa todos os espa�os
        public static string LimparTodosEspacos(string? texto) => string.IsNullOrWhiteSpace(texto) ? string.Empty : texto.Replace(" ", string.Empty);
        // Converte o texto para mai�sculo
        public static string ParaMaiusculo(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : texto.ToUpperInvariant();
        // Manter somente digitos numericos
        public static string LimparEDigitos(string? texto) => string.IsNullOrEmpty(texto) ? string.Empty : new string([.. texto.Where(char.IsDigit)]);
        [GeneratedRegex(@"\s+")]
        private static partial Regex EspacosRegex();
    }
}