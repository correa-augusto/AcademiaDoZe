//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe.Domain.ValueObject;
public record Arquivo
{
    public byte[] Conteudo { get; }
    public Arquivo(byte[] conteudo)
    {
        Conteudo = conteudo;
    }
}