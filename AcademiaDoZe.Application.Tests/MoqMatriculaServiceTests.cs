//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Domain.Entities;
using Microsoft.Identity.Client;
using Moq;

namespace AcademiaDoZe.Application.Tests
{
    public class MoqMatriculaServiceTests
    {
        private readonly Mock<IMatriculaService> _matriculaServiceMock;
        private readonly IMatriculaService _matriculaService;
        public MoqMatriculaServiceTests()
        {
            _matriculaServiceMock = new Mock<IMatriculaService>();
            _matriculaService = _matriculaServiceMock.Object;
        }
        private LogradouroDTO CriarEnderecoPadrao(int id = 1)
        {
            return new LogradouroDTO
            {
                Id = id,
                Cep = "88500000",
                Nome = "Rua teste",
                Bairro = "Bairro teste",
                Cidade = "Lages",
                Estado = "sc",
                Pais = "Brasil"
            };
        }

        private AlunoDTO CriarAlunoPadrao(int id = 1)
        {
            var enderecoId = 1;

            return new AlunoDTO
            {
                Id = id,
                Nome = "AlunoTeste",
                Cpf = "12345678901",
                DataNascimento = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
                Telefone = "11999999999",
                Email = "aluno@teste.com",
                Endereco = CriarEnderecoPadrao(enderecoId),
                Numero = "100",
                Complemento = "Apto 101",
                Senha = "Senha@123",
            };
        }

        private MatriculaDTO CriarMatriculaPadrao(int id = 1)
        {
            var alunoId = 1;

            return new MatriculaDTO
            {
                Id = id,
                AlunoMatricula = CriarAlunoPadrao(alunoId),
                Plano = EAppMatriculaPlano.Anual,
                DataInicio = DateOnly.FromDateTime(DateTime.Now),
                DataFim = DateOnly.FromDateTime(DateTime.Now.AddYears(+1)),
                Objetivo = "ganhar massa",
                RestricoesMedicas = EAppMatriculaRestricoes.None,
                ObservacoesRestricoes = "Nenhuma",
            };
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarMatricula_QuandoDadosValidos()
        {
            // Arrange
            var matriculaDto = CriarMatriculaPadrao(0); // ID 0 para novo registro
            var matriculaCriada = CriarMatriculaPadrao(1); // ID 1 após criação
                                                           // It.IsAny faz com que o Moq aceite qualquer objeto do tipo ColaboradorDTO
            _matriculaServiceMock.Setup(s => s.AdicionarAsync(It.IsAny<MatriculaDTO>())).ReturnsAsync(matriculaCriada);
            // Act
            var result = await _matriculaService.AdicionarAsync(matriculaDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _matriculaServiceMock.Verify(s => s.AdicionarAsync(It.IsAny<MatriculaDTO>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarMatricula_QuandoDadosValidos()
        {
            // Arrange
            var matriculaId = 1;
            var matriculaAtualizada = CriarMatriculaPadrao(matriculaId);
            matriculaAtualizada.Objetivo = "Perder peso";
            _matriculaServiceMock.Setup(s => s.AtualizarAsync(It.IsAny<MatriculaDTO>())).ReturnsAsync(matriculaAtualizada);
            // Act
            var result = await _matriculaService.AtualizarAsync(matriculaAtualizada);
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Perder peso", result.Objetivo);
            _matriculaServiceMock.Verify(s => s.AtualizarAsync(It.IsAny<MatriculaDTO>()), Times.Once);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarNatricula_QuandoExistir()
        {
            // Arrange
            var matriculaId = 1;
            var matriculaDto = CriarMatriculaPadrao(matriculaId);
            _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(matriculaId)).ReturnsAsync(matriculaDto);
            // Act
            var result = await _matriculaService.ObterPorIdAsync(matriculaId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(matriculaDto.Id, result.Id);
            _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(matriculaId), Times.Once);
        }
        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExistir()
        {
            // Arrange
            var matriculaId = 999;
            _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(matriculaId)).ReturnsAsync((MatriculaDTO)null!);
            // Act
            var result = await _matriculaService.ObterPorIdAsync(matriculaId);
            // Assert
            Assert.Null(result);
            _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(matriculaId), Times.Once);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarMatriculas_QuandoExistirem()
        {
            // Arrange
            var matriculasDto = new List<MatriculaDTO>
            {
                CriarMatriculaPadrao(1),
                CriarMatriculaPadrao(2)
            };

            _matriculaServiceMock.Setup(s => s.ObterTodasAsync()).ReturnsAsync(matriculasDto);
            // Act
            var result = await _matriculaService.ObterTodasAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _matriculaServiceMock.Verify(s => s.ObterTodasAsync(), Times.Once);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarListaVazia_QuandoNaoHouverMatriculas()
        {
            // Arrange
            _matriculaServiceMock.Setup(s => s.ObterTodasAsync()).ReturnsAsync(new List<MatriculaDTO>());
            // Act
            var result = await _matriculaService.ObterTodasAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _matriculaServiceMock.Verify(s => s.ObterTodasAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRemoverMatricula_QuandoExistir()
        {
            // Arrange

            var matriculad = 1;

            _matriculaServiceMock.Setup(s => s.RemoverAsync(matriculad)).ReturnsAsync(true);
            // Act

            var result = await _matriculaService.RemoverAsync(matriculad);

            // Assert

            Assert.True(result);

            _matriculaServiceMock.Verify(s => s.RemoverAsync(matriculad), Times.Once);
        }
        [Fact]
        public async Task RemoverAsync_DeveRetornarFalse_QuandoNaoExistir()
        {
            // Arrange

            var matriculad = 999;

            _matriculaServiceMock.Setup(s => s.RemoverAsync(matriculad)).ReturnsAsync(false);
            // Act

            var result = await _matriculaService.RemoverAsync(matriculad);

            // Assert

            Assert.False(result);

            _matriculaServiceMock.Verify(s => s.RemoverAsync(matriculad), Times.Once);
        }

        [Fact]
        public async Task ObterPorAlunoIdAsync_DeveRetornarMatricula_QuandoHouverAlunoVinculadoAMatricula()
        {
            // Arrange
            var matriculaId = 1;
            var alunoId = 1;
            var alunoDto = CriarAlunoPadrao(alunoId);
            var matriculaDto = CriarMatriculaPadrao(matriculaId);

            _matriculaServiceMock.Setup(s => s.ObterPorAlunoIdAsync(alunoId)).ReturnsAsync(new List<MatriculaDTO> { matriculaDto });

            // Act
            var result = await _matriculaService.ObterPorAlunoIdAsync(alunoId);

            // Assert
            Assert.NotNull(result);
            var matricula = result.First();
            Assert.Equal(matriculaDto.Id, matricula.Id);

            _matriculaServiceMock.Verify(s => s.ObterPorAlunoIdAsync(alunoId), Times.Once);
        }

        [Fact]
        public async Task ObterPorAlunoIdAsync_DeveRetornarListaVazia_QuandoNaoHouverAlunoVinculadoAMatricula()
        {
            // Arrange
            var alunoId = 99; 
            _matriculaServiceMock.Setup(s => s.ObterPorAlunoIdAsync(alunoId)).ReturnsAsync(new List<MatriculaDTO>()); // lista vazia

            // Act
            var result = await _matriculaService.ObterPorAlunoIdAsync(alunoId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // garante que não veio nada
            _matriculaServiceMock.Verify(s => s.ObterPorAlunoIdAsync(alunoId), Times.Once);
        }

        [Fact]
        public async Task ObterAtivasAsync_DeveRetornarMatricula_QuandoHouverAlunoComMatriculaAtiva()
        {
            // Arrange
            var alunoId = 10;
            var matriculaId = 1;
            var matriculaDto = CriarMatriculaPadrao(matriculaId);

            _matriculaServiceMock.Setup(s => s.ObterAtivasAsync(alunoId)).ReturnsAsync(new List<MatriculaDTO> { matriculaDto });

            // Act
            var result = await _matriculaService.ObterAtivasAsync(alunoId);

            // Assert
            Assert.NotNull(result);
            var matricula = result.First();
            Assert.Equal(matriculaDto.Id, matricula.Id);
        }

        [Fact]
        public async Task ObterAtivasAsync_DeveRetornarVazio_QuandoNaoHouverAlunoComMatriculaAtiva()
        {
            // Arrange
            var alunoId = 10;

            _matriculaServiceMock.Setup(s => s.ObterAtivasAsync(alunoId)).ReturnsAsync(new List<MatriculaDTO>());

            // Act
            var result = await _matriculaService.ObterAtivasAsync(alunoId);

            // Assert
            Assert.NotNull(result); 
            Assert.Empty(result); 
        }

        [Fact]

        public async Task ObterVencendoEmDiasAsync_DeveRetornarMatriculaAtivaComBaseNaData()
        {
            //Arrange
            var matriculaId = 1;
            var matriculaDto = CriarMatriculaPadrao(matriculaId);

            _matriculaServiceMock.Setup(s => s.ObterVencendoEmDiasAsync(matriculaId)).ReturnsAsync(new List<MatriculaDTO> { matriculaDto });

            // Act
            var result = await _matriculaService.ObterVencendoEmDiasAsync(matriculaId);

            // Assert
            Assert.NotNull(result);
            var matricula = result.First();
            Assert.Equal(matriculaDto.Id, matricula.Id);
        }

        [Fact]
        public async Task ObterVencendoEmDiasAsync_DeveRetornarVazio_QuandoNaoHouverMatriculaVencendoEmDias()
        {
            // Arrange
            var matriculaId = 1;

            _matriculaServiceMock.Setup(s => s.ObterVencendoEmDiasAsync(matriculaId)).ReturnsAsync(new List<MatriculaDTO>());

            // Act
            var result = await _matriculaService.ObterVencendoEmDiasAsync(matriculaId);

            // Assert
            Assert.NotNull(result); 
            Assert.Empty(result);    
        }
    }
    }

