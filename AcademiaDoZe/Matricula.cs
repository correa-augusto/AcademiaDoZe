using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe
{
    public class Matricula
    {
        public List<Matricula> Matriculas { get; set; } = new();
        public int Id { get; set; }
        public Aluno Aluno { get; set; }
        public TipoPlano Plano { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFinal { get; set; }
        public string Objetivo { get; set; }
        public  Restricoes Restricoes { get; set; }
        public string? Observacoes { get; set; }
        public string? LaudoMedicoCaminho { get; set; } 

        public bool Ativa => DateOnly.FromDateTime(DateTime.Today) <= DataFinal;

        public int DiasRestantesDoPlano()
        {
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            return (DataFinal.DayNumber - hoje.DayNumber);
        }

        public void ValidarMatricula()
        {
            if (Aluno == null) throw new ArgumentException("Matrícula deve estar vinculada a um aluno.");
            if (DataInicio > DataFinal) throw new ArgumentException("Data final não pode ser anterior à data de início.");

            int idade = DateOnly.FromDateTime(DateTime.Today).Year - Aluno.DataNascimento.Year;
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);
            if (Aluno.DataNascimento > hoje.AddYears(-idade))
                idade--;

            if (idade >= 12 && idade <= 16 && string.IsNullOrEmpty(LaudoMedicoCaminho))
                throw new ArgumentException("Alunos de 12 a 16 anos precisam apresentar laudo médico.");

            if (Restricoes != Restricoes.Nenhuma && string.IsNullOrEmpty(LaudoMedicoCaminho))
                throw new ArgumentException("Alunos com restrições de saúde precisam apresentar laudo médico.");
        }

        public Matricula(
        int id,
        Aluno aluno,
        TipoPlano plano,
        DateOnly dataInicio,
        DateOnly dataFinal,
        string objetivo,
        Restricoes restricoes,
        string? observacoes = null,
        string? laudoMedicoCaminho = null
    )
            {
                Id = id;
                Aluno = aluno ?? throw new ArgumentNullException(nameof(aluno), "Aluno não pode ser nulo.");
                Plano = plano;
                DataInicio = dataInicio;
                DataFinal = dataFinal;
                Objetivo = string.IsNullOrWhiteSpace(objetivo)
                    ? throw new ArgumentException("Objetivo não pode ser nulo ou vazio.")
                    : objetivo;
                Restricoes = restricoes;
                Observacoes = observacoes;
                LaudoMedicoCaminho = laudoMedicoCaminho;

                ValidarMatricula();
            }


        } 
    }
