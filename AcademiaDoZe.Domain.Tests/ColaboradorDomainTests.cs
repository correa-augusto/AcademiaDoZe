//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObject;


namespace AcademiaDoZe.Domain.Tests
{
    public class ColaboradorDomainTests
    {
        private Logradouro GetValidarLogradouro() => Logradouro.Criar(0, "12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Arquivo GetValidarArquivo() => Arquivo.Criar(new byte[1]);

        [Fact]
        public void CriarColaborador_ComDadosValidos_DeveCriarObjeto()
        {
            // Arrange
            var id = 0;
            var nome = "Maria Oliveira";
            var cpf = "12345678901";
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));
            var telefone = "11988887777";
            var email = "maria@email.com";
            var endereco = GetValidarLogradouro();
            var numero = "456";
            var complemento = "Bloco B";
            var senha = "Senha@123";
            var foto = GetValidarArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-2));
            var tipo = TipoUsuario.Atendente;
            var vinculo = TipoColaborador.CLT;

            // Act
            var colaborador = Colaborador.Criar(id, nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo);

            // Assert
            Assert.NotNull(colaborador);
        }
        [Fact]
        public void CriarColaborador_ComNomeVazio_DeveLancarExcecao()
        {
            var cpf = "12345678901";
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));
            var telefone = "11988887777";
            var email = "maria@email.com";
            var endereco = GetValidarLogradouro();
            var numero = "456";
            var complemento = "Bloco B";
            var senha = "Senha@123";
            var foto = GetValidarArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-2));
            var tipo = TipoUsuario.Atendente;
            var vinculo = TipoColaborador.CLT;

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                Colaborador.Criar(0, "", cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo)
            );

            // Assert
            Assert.Equal("NOME_OBRIGATORIO", ex.Message);
        }
    

        [Fact]
        public void CriarColaborador_ComCPFInvalido_DeveLancarExcecao()
        {
            // Arrange
            var id = 0;
            var nome = "Maria Oliveira";
            var cpf = "123"; // inválido
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));
            var telefone = "11988887777";
            var email = "maria@email.com";
            var endereco = GetValidarLogradouro();
            var numero = "456";
            var complemento = "Bloco B";
            var senha = "Senha@123";
            var foto = GetValidarArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddYears(-2));
            var tipo = TipoUsuario.Atendente;
            var vinculo = TipoColaborador.CLT;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => Colaborador.Criar(id, nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo));

            Assert.Equal("CPF_DIGITOS", ex.Message);
        }

    
        [Fact]
        public void CriarColaborador_ComDataAdmissaoFutura_DeveLancarExcecao()
        {
            // Arrange
            var id = 0;
            var nome = "Maria Oliveira";
            var cpf = "12345678901";
            var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));
            var telefone = "11988887777";
            var email = "maria@email.com";
            var endereco = GetValidarLogradouro();
            var numero = "456";
            var complemento = "Bloco B";
            var senha = "Senha@123";
            var foto = GetValidarArquivo();
            var dataAdmissao = DateOnly.FromDateTime(DateTime.Today.AddDays(5)); // futura
            var tipo = TipoUsuario.Atendente;
            var vinculo = TipoColaborador.CLT;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Colaborador.Criar(id, nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo));

            Assert.Equal("Data de admissão não pode ser no futuro.", ex.Message);
        }
    }
}