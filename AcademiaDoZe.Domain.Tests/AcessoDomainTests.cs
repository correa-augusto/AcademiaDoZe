// AUGUSTO DOS SANTOS CORREA
using System;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.ValueObject;
using AcademiaDoZe.Domain.Exceptions;
using Xunit;
using AcademiaDoZe.AcademiaDoZe;

namespace AcademiaDoZe.Domain.Tests
{
    public class AcessoDomainTests
    {

        private Arquivo GetArquivoValido() => Arquivo.Criar(new byte[1]);
        private Logradouro GetValidarLogradouro() => Logradouro.Criar(0, "12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Pessoa CriarPessoaValida()
        {
            return Aluno.Criar(
                0,
                nome: "Maria Souza",
                cpf: "98765432100",
                dataNascimento: DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                telefone: "11988888888",
                email: "maria@email.com",
                endereco: GetValidarLogradouro(),
                numero: "200",
                complemento: "Casa",
                senha: "senha123",
                foto: GetArquivoValido()
            );
        }

        [Fact]
        public void CriarAcesso_ComDadosValidos_DeveCriarObjeto()
        {
            var pessoa = CriarPessoaValida();
            var entrada = DateTime.Now.AddHours(-1);
            var saida = DateTime.Now;

            var acesso = Acesso.Criar(pessoa, saida, entrada);

            Assert.NotNull(acesso);
            Assert.Equal(pessoa, acesso.AlunoColaborador);
            Assert.Equal(entrada, acesso.DataHoraEntrada);
            Assert.Equal(saida, acesso.DataHoraSaida);
        }

        [Fact]
        public void CriarAcesso_ComPessoaNula_DeveLancarExcecao()
        {
            var entrada = DateTime.Now.AddHours(-1);
            var saida = DateTime.Now;

            var ex = Assert.Throws<ArgumentNullException>(() =>
                Acesso.Criar(null, saida, entrada)
            );

            Assert.Equal("pessoa", ex.ParamName);
        }

        [Fact]
        public void CriarAcesso_ComEntradaNoFuturo_DeveLancarExcecao()
        {
            var pessoa = CriarPessoaValida();
            var entrada = DateTime.Now.AddMinutes(10); // futuro
            var saida = DateTime.Now.AddMinutes(20);

            var ex = Assert.Throws<ArgumentException>(() =>
                Acesso.Criar(pessoa, saida, entrada)
            );

            Assert.Equal("Data e hora do acesso não podem estar no futuro.", ex.Message);
        }

        [Fact]
        public void CriarAcesso_ComSaidaAntesDaEntrada_DeveLancarExcecao()
        {
            var pessoa = CriarPessoaValida();
            var entrada = DateTime.Now;
            var saida = DateTime.Now.AddMinutes(-30);

            var ex = Assert.Throws<DomainException>(() =>
                Acesso.Criar(pessoa, saida, entrada)
            );

            Assert.Equal("Data de saída não pode ser anterior à entrada.", ex.Message);
        }

        [Fact]
        public void CriarAcesso_ComEntradaMinValue_DeveLancarExcecao()
        {
            var pessoa = CriarPessoaValida();
            var entrada = DateTime.MinValue;
            var saida = DateTime.Now;

            var ex = Assert.Throws<DomainException>(() =>
                Acesso.Criar(pessoa, saida, entrada)
            );

            Assert.Equal("Data e hora de entrada inválida.", ex.Message);
        }

    }
}