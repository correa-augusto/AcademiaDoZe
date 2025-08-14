using
System.ComponentModel.DataAnnotations;
namespace AcademiaDoZe.Domain.Enums
{
    public enum PessoaTipo
    {
        [Display(Name = "Colaborador")]
        Colaborador = 0,
        [Display(Name = "Aluno")]
        Aluno = 1
    }
}