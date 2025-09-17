//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObject;
using AcademiaDoZe.Infrastructure.Repositories;

namespace AcademiaDoZe.Infrastructure.Tests
{
    public class AlunoInfrasrucutureTests : TestBase
    {
        [Fact]
        public async Task Aluno_LogradouroPorId_CpfJaExiste_Adicionar()
        {
            // com base em logradouroID, acessar logradourorepository e obter o logradouro

            var logradouroId = 4;
            var repoLogradouroObterPorId = new LogradouroRepository(ConnectionString, DatabaseType);
            Logradouro? logradouro = await repoLogradouroObterPorId.ObterPorId(logradouroId);
            // cria um arquivo de exemplo

            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });

            var _cpf = "12345678900";
            // verifica se cpf já existe

            var repoAlunoCpf = new AlunoRepository(ConnectionString, DatabaseType);

            var cpfExistente = await repoAlunoCpf.CpfJaExiste(_cpf);
            Assert.False(cpfExistente, "CPF já existe no banco de dados.");
            var aluno = Aluno.Criar(
            0,
            "zé",
            _cpf,

            new DateOnly(2010, 10, 09),
            "49999999999",
            "ze@com.br",
            logradouro!,
            "123",
            "complemento casa",
            "abcBolinhas",
            arquivo


            );
            // Adicionar

            var repoAlunoAdicionar = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoInserido = await repoAlunoAdicionar.Adicionar(aluno);
            Assert.NotNull(alunoInserido);
            Assert.True(alunoInserido.Id > 0);

        }


        [Fact]
        public async Task Aluno_ObterPorCpf_Atualizar()
        {
            var _cpf = "12345678900";
            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
            var repoAlunoObterPorCpf = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoExistente = await repoAlunoObterPorCpf.ObterPorCpf(_cpf);
            Assert.NotNull(alunoExistente);

            // criar novo colaborador com os mesmos dados, editando o que quiser
            var colaboradorAtualizado = Aluno.Criar(
            0,
            "zé dos testes 123",
            alunoExistente.Cpf,
            alunoExistente.DataNascimento,
            alunoExistente.Telefone,
            alunoExistente.Email,
            alunoExistente.Endereco,
            alunoExistente.Numero,
            alunoExistente.Complemento,
            alunoExistente.Senha,
            arquivo
            );

            // Teste de Atualização

            var repoAlunoAtualizar = new AlunoRepository(ConnectionString, DatabaseType);
            var resultadoAtualizacao = await repoAlunoAtualizar.Atualizar(colaboradorAtualizado);
            Assert.NotNull(resultadoAtualizacao);

            Assert.Equal("zé dos testes 123", resultadoAtualizacao.Nome);

        }
        [Fact]
        public async Task Aluno_ObterPorCpf_TrocarSenha()
        {
            var _cpf = "12345678900";
            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
            var repoAlunoPorCpf = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoExistente = await repoAlunoPorCpf.ObterPorCpf(_cpf);
            Assert.NotNull(alunoExistente);
            var novaSenha = "novaSenha123";
            var repoColaboradorTrocarSenha = new AlunoRepository(ConnectionString, DatabaseType);

            var resultadoTrocaSenha = await repoColaboradorTrocarSenha.TrocarSenha(alunoExistente.Id, novaSenha);
            Assert.True(resultadoTrocaSenha);

            var repoAlunoObterPorId = new AlunoRepository(ConnectionString, DatabaseType);
            var colaboradorAtualizado = await repoAlunoObterPorId.ObterPorId(alunoExistente.Id);
            Assert.NotNull(colaboradorAtualizado);
            Assert.Equal(novaSenha, colaboradorAtualizado.Senha);
        }

        [Fact]
        public async Task Colaborador_ObterTodos()
        {
            var repoAlunoRepository = new AlunoRepository(ConnectionString, DatabaseType);
            var resultado = await repoAlunoRepository.ObterTodos();
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task Colaborador_ObterPorCpf_Remover_ObterPorId()
        {
            var _cpf = "12345678900";
            var repoAlunoObterPorCpf = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoExistente = await repoAlunoObterPorCpf.ObterPorCpf(_cpf);
            Assert.NotNull(alunoExistente);

            // Remover
            var repoAlunoRemover = new AlunoRepository(ConnectionString, DatabaseType);
            var resultadoRemover = await repoAlunoRemover.Remover(alunoExistente.Id);
            Assert.True(resultadoRemover);

            var repoAlunoObterPorId = new AlunoRepository(ConnectionString, DatabaseType);
            var resultadoRemovido = await repoAlunoObterPorId.ObterPorId(alunoExistente.Id);
            Assert.Null(resultadoRemovido);
        }
    }
}

