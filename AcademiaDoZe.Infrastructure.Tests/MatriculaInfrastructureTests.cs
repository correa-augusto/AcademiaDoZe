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
using AcedemiaDoZe.Infrastructure.Repositories;

namespace AcademiaDoZe.Infrastructure.Tests
{
    public class MatriculaInfrastructureTests : TestBase
    {

        [Fact]
        public async Task Matricula_AdicionarMatricula()
        {
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
             var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType, repoAluno);
            var logradouroId = 4;
            var repoLogradouroObterPorId = new LogradouroRepository(ConnectionString, DatabaseType);
            Logradouro? logradouro = await repoLogradouroObterPorId.ObterPorId(logradouroId);
            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });

            var _cpf = "12345678904";
            // verifica se cpf já existe


            var cpfExistente = await repoAluno.CpfJaExiste(_cpf);
            Assert.False(cpfExistente, "CPF já existe no banco de dados.");

            var alunoMock = Aluno.Criar(0, "Nome teste agora vai", _cpf, new DateOnly(1990, 1, 1), "49999047812", "email@teste.com", logradouro, "123", "complemento casa", "senha_teste", arquivo);

            // Salva o aluno no banco e obtém o objeto com ID preenchido
            var alunoInserido = await repoAluno.Adicionar(alunoMock);
            Assert.NotNull(alunoInserido);
            Assert.True(alunoInserido.Id > 0);



            var matricula = Matricula.Criar(
                0,
                 alunoMock,
                 TipoPlano.Mensal,
                 new DateOnly(2025, 8, 15),
                 new DateOnly(2025, 9, 15),
                 "Ganhar massa muscular",
                 Restricoes.None,
                 "nenhuma",
                 arquivo
               );

            var repoMatriculaAdicionar = new MatriculaRepository(ConnectionString, DatabaseType, repoAluno);
            var matriculaInserido = await repoMatriculaAdicionar.Adicionar(matricula);
            Assert.NotNull(matriculaInserido);
            Assert.True(matriculaInserido.Id > 0);
        }

        [Fact]
        public async Task Matricula_AtualizarMatricula_ComAlunoExistente()
        {
            // Passa o repoAluno para o construtor do MatriculaRepository
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType, repoAluno);
            var repoLogradouro = new LogradouroRepository(ConnectionString, DatabaseType);

            var logradouroId = 4;
            Logradouro? logradouro = await repoLogradouro.ObterPorId(logradouroId);
            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });

            var alunoId = 17;
            Aluno? aluno = await repoAluno.ObterPorId(alunoId);

            var matriculaId = 24;
            Matricula? matricula = await repoMatricula.ObterPorId(matriculaId);
            Assert.NotNull(matricula);
            Assert.True(matricula.Id > 0);

            // 3. Modifica a Matrícula com os novos dados
            var novoObjetivo = "Perder peso e aumentar resistência";
            var novaDataFim = new DateOnly(2025, 10, 30);
            var matriculaAtualizada = Matricula.Criar(
                0,
                matricula.AlunoMatricula,
                matricula.Plano,
                matricula.DataInicio,
                novaDataFim,
                novoObjetivo,
                matricula.RestricoesMedicas,
                matricula.ObservacoesRestricoes,
                matricula.LaudoMedico
            );
            // Atribui o ID da matrícula que será atualizada
            typeof(Entity).GetProperty("Id")?.SetValue(matriculaAtualizada, matricula.Id);

            // Act
            var matriculaModificada = await repoMatricula.Atualizar(matriculaAtualizada);

            // 4. Verifica se o método de atualização retornou a matrícula corretamente
            Assert.NotNull(matriculaModificada);
            Assert.Equal(matricula.Id, matriculaModificada.Id);

            // 5. Busca a matrícula novamente no banco de dados para verificar os dados
            var matriculaDoBanco = await repoMatricula.ObterPorId(matriculaModificada.Id);

            // 6. Confere se as propriedades foram de fato atualizadas no banco
            Assert.NotNull(matriculaDoBanco);
            Assert.Equal(novoObjetivo, matriculaDoBanco.Objetivo);
            Assert.Equal(novaDataFim, matriculaDoBanco.DataFim);
        }

        [Fact]
        public async Task Matricula_ObterVencendoEmDias()
        {
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType, repoAluno);

            var logradouroId = 4;
            var repoLogradouroObterPorId = new LogradouroRepository(ConnectionString, DatabaseType);
            Logradouro? logradouro = await repoLogradouroObterPorId.ObterPorId(logradouroId);
            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
            var alunoId = 17;
            Aluno? aluno = await repoAluno.ObterPorId(alunoId);

            var matricula = Matricula.Criar(
                0,
                 aluno,
                 TipoPlano.Mensal,
                 new DateOnly(2025, 8, 15),
                 new DateOnly(2025, 8, 25), 
                 "Objetivo teste",
                 Restricoes.None,
                 "nenhuma",
                 arquivo
            );

            var matriculaInserido = await repoMatricula.Adicionar(matricula);
            Assert.NotNull(matriculaInserido);
            Assert.True(matriculaInserido.Id > 0);

            // Teste da busca de matrículas vencendo em 10 dias
            var matriculasVencendo = await repoMatricula.ObterVencendoEmDias(10);
            Assert.NotEmpty(matriculasVencendo);
            Assert.Contains(matriculasVencendo, m => m.Id == matriculaInserido.Id);
        }

        [Fact]
        public async Task Matricula_ObterAtivas()
        {
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType, repoAluno);

            var logradouroId = 4;
            var repoLogradouroObterPorId = new LogradouroRepository(ConnectionString, DatabaseType);
            Logradouro? logradouro = await repoLogradouroObterPorId.ObterPorId(logradouroId);
            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });

            var alunoId = 17;
            Aluno? aluno = await repoAluno.ObterPorId(alunoId);

            Assert.NotNull(aluno);
            Assert.True(aluno.Id > 0);

            var matricula = Matricula.Criar(
                 0,
                 aluno,
                 TipoPlano.Mensal,
                 new DateOnly(2025, 8, 15),
                 new DateOnly(2025, 9, 15),
                 "Objetivo teste",
                 Restricoes.None,
                 "nenhuma",
                 arquivo
            );

            var matriculaInserido = await repoMatricula.Adicionar(matricula);
            Assert.NotNull(matriculaInserido);
            Assert.True(matriculaInserido.Id > 0);

            // Teste da busca de matrículas ativas
            var matriculasAtivas = await repoMatricula.ObterAtivas();
            Assert.NotEmpty(matriculasAtivas);
            Assert.Contains(matriculasAtivas, m => m.Id == matriculaInserido.Id);
        }

        [Fact]
        public async Task Matricula_ObterPorAluno()
        {
            var repoAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType, repoAluno);

            var logradouroId = 4;
            var repoLogradouroObterPorId = new LogradouroRepository(ConnectionString, DatabaseType);
            Logradouro? logradouro = await repoLogradouroObterPorId.ObterPorId(logradouroId);
            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
            var alunoId = 17;
            Aluno? aluno = await repoAluno.ObterPorId(alunoId);

            Assert.NotNull(aluno);
            Assert.True(aluno.Id > 0);

            var matricula = Matricula.Criar(
                 0,
                 aluno,
                 TipoPlano.Mensal,
                 new DateOnly(2025, 8, 15),
                 new DateOnly(2025, 9, 15),
                 "Objetivo teste",
                 Restricoes.None,
                 "nenhuma",
                 arquivo
            );

            // Salva a matrícula no banco
            var matriculaInserido = await repoMatricula.Adicionar(matricula);
            Assert.NotNull(matriculaInserido);
            Assert.True(matriculaInserido.Id > 0);

            // Teste da busca por aluno
            var matriculas = await repoMatricula.ObterPorAluno(aluno.Id);
            Assert.NotEmpty(matriculas);
            Assert.Contains(matriculas, m => m.AlunoMatricula.Id == aluno.Id);
        }
    }
}