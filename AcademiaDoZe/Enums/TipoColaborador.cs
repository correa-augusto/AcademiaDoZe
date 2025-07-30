//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe
{
    public enum TipoColaborador
    {
        [Display(Name = "CLT")]
        CLT = 0,
        [Display(Name = "Estagiário")]
        Estagio = 1
    }
}
