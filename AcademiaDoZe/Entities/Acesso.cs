//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.AcademiaDoZe;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Entities;
public sealed class Acesso : Entity
{
    public Pessoa AlunoColaborador { get; }
    public DateTime DataHoraEntrada { get; }

    public DateTime DataHoraSaida { get; }

    private Acesso(Pessoa pessoa, DateTime dataHoraEntrada, DateTime dataHoraSaida)
    {
        AlunoColaborador = pessoa;
        DataHoraEntrada = dataHoraEntrada;
        DataHoraSaida = dataHoraSaida;
    }

    public static Acesso Criar(Pessoa pessoa,  DateTime dataHoraSaida, DateTime dataHoraEntrada )
    {
        if (pessoa == null) throw new ArgumentNullException(nameof(pessoa));

        if (dataHoraEntrada > DateTime.Now) throw new ArgumentException("Data e hora do acesso não podem estar no futuro.");

        if (dataHoraSaida  < dataHoraEntrada) throw new DomainException("Data de saída não pode ser anterior à entrada.");

        if (dataHoraEntrada == DateTime.MinValue)  throw new DomainException("Data e hora de entrada inválida.");

        if (dataHoraSaida == DateTime.MinValue) throw new DomainException("Data e hora de saída inválida.");

        return new Acesso(pessoa, dataHoraEntrada, dataHoraSaida);
    }
}