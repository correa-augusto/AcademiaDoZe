//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.AcademiaDoZe;

using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObject;
namespace AcademiaDoZe.Domain.Entities;
public sealed class Colaborador : Pessoa
{
    public DateOnly DataAdmissao {get;}
    public PessoaTipo Tipo { get; }
    public TipoColaborador Vinculo { get; }
    private Colaborador(string nomeCompleto,
    string cpf,

    DateOnly dataNascimento,
    string telefone,
    string email,
    Endereco endereco,
    string numero,
    string complemento,
    string senha,
    Arquivo foto,
    DateOnly dataAdmissao,
    PessoaTipo tipo,
    TipoColaborador vinculo)

    : base(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto)
    {
        DataAdmissao = dataAdmissao;
        Tipo = tipo;
        Vinculo = vinculo;
    }

    public static Colaborador Criar(string nome, string cpf, DateOnly dataNascimento, string telefone, string email, Endereco endereco, string numero, string complemento, string senha, Arquivo foto, DateOnly dataAdmissao, PessoaTipo tipo, TipoColaborador vinculo)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new DomainException("NOME_OBRIGATORIO");

        if (string.IsNullOrWhiteSpace(cpf)) throw new DomainException("CPF_OBRIGATORIO");

        cpf = TextoNormalizadoService.LimparEDigitos(cpf);

        if (cpf.Length != 11) throw new DomainException("CPF_DIGITOS");

        if (dataNascimento > DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("Data de nascimento não pode ser no futuro.");

        if (dataNascimento < DateOnly.FromDateTime(new DateTime(1900, 1, 1))) throw new ArgumentException("Data de nascimento muito antiga.");

        if (string.IsNullOrWhiteSpace(telefone)) throw new ArgumentException("Telefone não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email não pode ser nulo ou vazio.");

        numero = TextoNormalizadoService.LimparEDigitos(numero);

        if (string.IsNullOrWhiteSpace(senha)) throw new ArgumentException("senha não pode ser nulo ou vazio.");

        if(foto == null) throw new ArgumentNullException(nameof(foto));

        if (dataAdmissao > DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("Data de admissão não pode ser no futuro.");

        if (!Enum.IsDefined(typeof(PessoaTipo), tipo))  throw new ArgumentException("Tipo de usuário inválido.");

        if (!Enum.IsDefined(typeof(TipoColaborador), vinculo)) throw new ArgumentException("Tipo de usuário inválido.");


        return new Colaborador(nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo);
    }
}