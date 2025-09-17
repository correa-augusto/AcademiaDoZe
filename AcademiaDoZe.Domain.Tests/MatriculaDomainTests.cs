// AUGUSTO DOS SANTOS CORREA
using System;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObject;
using Xunit;

namespace AcademiaDoZe.Domain.Tests
{
    public class MatriculaDomainTests
    {
        private Arquivo GetArquivoValido() => Arquivo.Criar(new byte[1]);

        private Aluno GetAlunoValido(DateOnly? nascimento = null)
        {
            return Aluno.Criar(
                id: 0,
                nome: "João Silva",
                cpf: "12345678901",
                dataNascimento: nascimento ?? DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
                telefone: "11999999999",
                email: "joao@email.com",
                endereco: Logradouro.Criar(0, "12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil"),
                numero: "100",
                complemento: "Apto 1",
                senha: "123456",
                foto : GetArquivoValido()
            );
        }

        [Fact]
        public void CriarMatricula_ComDadosValidos_DeveCriarObjeto()
        {
            var id = 0;
            var aluno = GetAlunoValido();
            var plano = TipoPlano.Mensal;
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-10));
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(20));
            var objetivo = "Condicionamento físico";
            var restricoes = Restricoes.None;
            var observacoesRestricoes = "Nnehuma";
            var laudo = GetArquivoValido();

            var matricula = Matricula.Criar(id, aluno, plano, dataInicio, dataFim, objetivo, restricoes, observacoesRestricoes, laudo);

            Assert.NotNull(matricula);
            Assert.True(matricula.Ativa);
            Assert.Equal(20 - 0, matricula.DiasRestantesDoPlano());
        }

        [Fact]
        public void CriarMatricula_ComAlunoNulo_DeveLancarExcecao()
        {
            var id = 0;
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-5));
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(25));
            var plano = TipoPlano.Mensal;
            var laudo = GetArquivoValido();
            var observacoesRestricoes = "nenhuma";

            var ex = Assert.Throws<ArgumentNullException>(() =>
                Matricula.Criar(id, null, plano, dataInicio, dataFim, "Força", Restricoes.None, observacoesRestricoes, laudo)
            );

            Assert.Equal("alunoMatricula", ex.ParamName);
        }

        [Fact]
        public void CriarMatricula_ComPlanoInvalido_DeveLancarExcecao()
        {
            var id = 0;
            var aluno = GetAlunoValido();
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-5));
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(10));
            var laudo = GetArquivoValido();
            var observacoesRestricoes = "nenhuma";


            var ex = Assert.Throws<ArgumentException>(() =>
                Matricula.Criar(id, aluno, (TipoPlano)999, dataInicio, dataFim, "Definição muscular", Restricoes.None, observacoesRestricoes, laudo)
            );

            Assert.Equal("Tipo de plano inválido.", ex.Message);
        }

        [Fact]
        public void CriarMatricula_ComDataInicioFutura_DeveLancarExcecao()
        {
            var id = 0;
            var aluno = GetAlunoValido();
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(2)); // futuro
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(10));
            var laudo = GetArquivoValido();
            var observacoesRestricoes = "nenhuma";

            var ex = Assert.Throws<ArgumentException>(() =>
                Matricula.Criar(id, aluno, TipoPlano.Mensal, dataInicio, dataFim, "Emagrecimento", Restricoes.None, observacoesRestricoes, laudo)
            );

            Assert.Equal("Data de inicio não pode ser no futuro.", ex.Message);
        }

        [Fact]
        public void CriarMatricula_ComDataFimAntesDeInicio_DeveLancarExcecao()
        {
            var id = 0;
            var aluno = GetAlunoValido();
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(-2));
            var laudo = GetArquivoValido();
            var observacoesRestricoes = "nenhuma";

            var ex = Assert.Throws<ArgumentException>(() =>
                Matricula.Criar(id, aluno, TipoPlano.Mensal, dataInicio, dataFim, "Saúde", Restricoes.None, observacoesRestricoes, laudo)
            );

            Assert.Equal("Data final não pode ser anterior à data de início.", ex.Message);
        }

        [Fact]
        public void CriarMatricula_ComObjetivoVazio_DeveLancarExcecao()
        {
            var id = 0;
            var aluno = GetAlunoValido();
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(10));
            var laudo = GetArquivoValido();
            var observacoesRestricoes = "nenhuma";

            var ex = Assert.Throws<ArgumentException>(() =>
                Matricula.Criar(id, aluno, TipoPlano.Mensal, dataInicio, dataFim, "", Restricoes.None, observacoesRestricoes, laudo)
            );

            Assert.Equal("objetivo deve ser preenchido", ex.Message);
        }

        [Fact]
        public void CriarMatricula_ComRestricoesSemLaudo_DeveLancarExcecao()
        {
            var id = 0;
            var aluno = GetAlunoValido();
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(10));
            var observacoesRestricoes = "nenhuma";

            var ex = Assert.Throws<ArgumentException>(() =>
                Matricula.Criar(id, aluno, TipoPlano.Mensal, dataInicio, dataFim, "Alongamento", Restricoes.Diabetes, observacoesRestricoes, null)
            );

            Assert.Equal("Alunos com restrições de saúde precisam apresentar laudo médico.", ex.Message);
        }

        [Fact]
        public void CriarMatricula_AlunoEntre12e16SemLaudo_DeveLancarExcecao()
        {
            var id = 0;
            var aluno = GetAlunoValido(DateOnly.FromDateTime(DateTime.Today.AddYears(-13))); 
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(30));
            var observacoesRestricoes = "nenhuma";

            var ex = Assert.Throws<ArgumentException>(() =>
                Matricula.Criar(id, aluno, TipoPlano.Mensal, dataInicio, dataFim, "Reabilitação", Restricoes.None, observacoesRestricoes, null)
            );

            Assert.Equal("Alunos de 12 a 16 anos precisam apresentar laudo médico.", ex.Message);
        }

        [Fact]
        public void CriarMatricula_ComLaudoNulo_DeveLancarExcecao()
        {
            var id = 0;
            var aluno = GetAlunoValido();
            var dataInicio = DateOnly.FromDateTime(DateTime.Today.AddDays(-3));
            var dataFim = DateOnly.FromDateTime(DateTime.Today.AddDays(30));
            var observacoesRestricoes = "nenhuma";

            var ex = Assert.Throws<ArgumentNullException>(() =>
                Matricula.Criar(id, aluno, TipoPlano.Mensal, dataInicio, dataFim, "Flexibilidade", Restricoes.None, observacoesRestricoes, null)
            );

            Assert.Equal("laudoMedico", ex.ParamName);
        }
    }
}