using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Domain.Enums;
namespace AcademiaDoZe.Application.Mappings
{
    public static class MatriculaEnumMappings
    {
        public static TipoPlano ToDomain(this EAppMatriculaPlano appPlano)
        {
            return (TipoPlano)appPlano;
        }
        public static EAppMatriculaPlano ToApp(this TipoPlano domainPlano)
        {
            return (EAppMatriculaPlano)domainPlano;
        }
        public static Restricoes ToDomain(this EAppMatriculaRestricoes? appRestricoes)
        {
            return (Restricoes)appRestricoes;
        }
        public static EAppMatriculaRestricoes ToApp(this Restricoes domainRestricoes)
        {
            return (EAppMatriculaRestricoes)domainRestricoes;
        }
    }
}