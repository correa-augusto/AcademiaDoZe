//AUGUSTO DOS SANTOS CORREA
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
namespace AcademiaDoZe.Application.Tests
{
    public class MatriculaApplicationTests
    {
        // Configurações de conexão
        const string connectionString = "Server=localhost;Port=3307;Database=db_academia_do_ze;Uid=correa;Pwd=123;AllowPublicKeyRetrieval=True;SslMode=None;";
        const EAppDatabaseType databaseType = EAppDatabaseType.MySql;

        [Fact(Timeout = 60000)]
        public async Task MatriculaService_Integracao_Adicionar_Obter_Atualizar_Remover()
        {
            // Arrange: DI usando repositório real (Infra)
            var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
            var provider = DependencyInjection.BuildServiceProvider(services);
            var matriculaService = provider.GetRequiredService<IMatriculaService>();
            var alunoRepoFactory = provider.GetRequiredService<Func<IAlunoRepository>>();
            var alunoRepository = alunoRepoFactory();
            var matriculaRepoFactory = provider.GetRequiredService<Func<IMatriculaRepository>>();
            var matriculaRepository = matriculaRepoFactory();

            var _cpf = GerarCpfFake();
            var caminhoFoto = Path.Combine("..", "..", "..", "foto_teste.png");
            ArquivoDTO foto = new();

            if (File.Exists(caminhoFoto)) { foto.Conteudo = await File.ReadAllBytesAsync(caminhoFoto); }
            else { foto.Conteudo = null; Assert.Fail("Foto de teste não encontrada."); }

            var enderecoDto = new LogradouroDTO
            {
                Id = 1,
                Cep = "88500000",
                Nome = "Rua teste",
                Bairro = "Bairro teste",
                Cidade = "Lages",
                Estado = "sc",
                Pais = "Brasil"
            };

            var alunoDto = new AlunoDTO
            {
                Nome = "AlunoTeste",
                Cpf = _cpf,
                DataNascimento = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
                Telefone = "11999999999",
                Email = "aluno@teste.com",
                Endereco = enderecoDto,
                Numero = "100",
                Complemento = "Apto 101",
                Senha = "Senha@123",
            };

            // LIMPEZA DAS TABELAS ANTES DO TESTE
            await matriculaRepository.LimparTabela();
            await alunoRepository.LimparTabela();

            // Adiciona o aluno no banco UMA ÚNICA VEZ
            var alunoEntity = alunoDto.ToEntityMatricula();
            var alunoAdicionado = await alunoRepository.Adicionar(alunoEntity);

            // ATUALIZA O DTO DO ALUNO COM O ID GERADO PELO BANCO DE DADOS
            alunoDto.Id = alunoAdicionado.Id;

            // Cria o DTO da matrícula usando o DTO do aluno já atualizado
            var dto = new MatriculaDTO
            {
                AlunoMatricula = alunoDto,
                Plano = EAppMatriculaPlano.Anual,
                DataInicio = DateOnly.FromDateTime(DateTime.Now),
                DataFim = DateOnly.FromDateTime(DateTime.Now.AddYears(+1)),
                Objetivo = "ganhar massa",
                RestricoesMedicas = EAppMatriculaRestricoes.None,
                ObservacoesRestricoes = "Nenhuma",
            };

            MatriculaDTO? criado = null;

            try
            {
                // Act - Adicionar
                criado = await matriculaService.AdicionarAsync(dto);
            }
            finally
            {
                // Clean-up
                if (criado is not null)
                {
                    try { await matriculaService.RemoverAsync(criado.Id); } catch { }
                }
                await matriculaRepository.LimparTabela();
                await alunoRepository.LimparTabela();
            }
        }

        // Helper
        private static string GerarCpfFake()
        {
            var rnd = new Random();
            return string.Concat(Enumerable.Range(0, 11).Select(_ => rnd.Next(0, 10).ToString()));
        }
    }
}