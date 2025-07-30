//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace AcademiaDoZe.Domain.Enums;
public enum TipoUsuario
{
    [Display(Name = "Administrador")]
    Administrador = 0,
    [Display(Name = "Atendente")]
    Atendente = 1,
    [Display(Name = "Instrutor")]
    Instrutor = 2,
    [Display(Name = "Aluno")]
    Aluno = 3
}
