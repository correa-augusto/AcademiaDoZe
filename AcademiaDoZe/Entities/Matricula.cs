//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObject;
namespace AcademiaDoZe.Domain.Entities;
public sealed class Matricula : Entity
{
    public Aluno AlunoMatricula { get; }
    public TipoPlano Plano { get; }
    public DateOnly DataInicio { get; }
    public DateOnly DataFim { get; }
    public string Objetivo { get; }
    public Restricoes RestricoesMedicas { get; }
    public string ObservacoesRestricoes { get; }
    public Arquivo LaudoMedico { get; }
    private Matricula(Aluno alunoMatricula,
    TipoPlano plano,
    DateOnly dataInicio,
    DateOnly dataFim,
    string objetivo,
    Restricoes restricoesMedicas,
    Arquivo laudoMedico)

    : base()
    {
        AlunoMatricula = alunoMatricula;
        Plano = plano;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Objetivo = objetivo;
        RestricoesMedicas = restricoesMedicas;
        LaudoMedico = laudoMedico;
    }

    public bool Ativa => DateOnly.FromDateTime(DateTime.Today) <= DataFim;
    public int DiasRestantesDoPlano()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        return (DataFim.DayNumber - hoje.DayNumber);
    }
    public static Matricula Criar(Aluno alunoMatricula, TipoPlano plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, Restricoes restricoesMedicas, Arquivo laudoMedico)
    {

        int idade = DateOnly.FromDateTime(DateTime.Today).Year - alunoMatricula.DataNascimento.Year;

        DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);

        if (alunoMatricula.DataNascimento > hoje.AddYears(-idade)) idade--;

        if (idade >= 12 && idade <= 16 &&laudoMedico == null) throw new ArgumentException("Alunos de 12 a 16 anos precisam apresentar laudo médico.");

        if (restricoesMedicas != Restricoes.None && laudoMedico == null) throw new ArgumentException("Alunos com restrições de saúde precisam apresentar laudo médico.");

        if (alunoMatricula == null) throw new ArgumentNullException(nameof(alunoMatricula));

        if (!Enum.IsDefined(typeof(TipoPlano), plano)) throw new ArgumentException("Tipo de plano inválido.");

        if (dataInicio > DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("Data de inicio não pode ser no futuro.");

        if (dataInicio > dataFim) throw new ArgumentException("Data final não pode ser anterior à data de início.");

        if (string.IsNullOrEmpty(objetivo)) throw new ArgumentException("objetivo deve ser preenchido");

        if (!Enum.IsDefined(typeof(Restricoes), restricoesMedicas)) throw new ArgumentException("Tipo de restrição inválido.");

        if (laudoMedico == null) throw new ArgumentNullException(nameof(laudoMedico));

        return new Matricula(alunoMatricula, plano, dataInicio, dataFim, objetivo, restricoesMedicas, laudoMedico);
    }
}