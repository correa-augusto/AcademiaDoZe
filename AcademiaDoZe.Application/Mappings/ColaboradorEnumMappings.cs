using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Domain.Enums;
namespace AcademiaDoZe.Application.Mappings
{
    public static class ColaboradorEnumMappings
    {
        public static TipoUsuario ToDomain(this EAppColaboradorTipo appTipo)
        {
            return (TipoUsuario)appTipo;
        }
        public static EAppColaboradorTipo ToApp(this TipoUsuario domainTipo)
        {
            return (EAppColaboradorTipo)domainTipo;
        }
        public static TipoColaborador ToDomain(this EAppColaboradorVinculo appVinculo)
        {
            return (TipoColaborador)appVinculo;
        }
        public static EAppColaboradorVinculo ToApp(this TipoColaborador domainVinculo)
        {
            return (EAppColaboradorVinculo)domainVinculo;
        }
    }
}