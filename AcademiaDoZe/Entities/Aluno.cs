//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.AcademiaDoZe;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObject;

namespace AcademiaDoZe.Domain.Entities;
public sealed  class Aluno : Pessoa
{
    private Aluno(string nomeCompleto,
    string cpf,

    DateOnly dataNascimento,
    string telefone,
    string email,
    Endereco endereco,
    string numero,
    string complemento,
    string senha,
    Arquivo foto)

    : base(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto)
    {
    }

    public static Aluno Criar(string nome, string cpf, DateOnly dataNascimento, string telefone, string email, Endereco endereco, string numero, string complemento, string senha, Arquivo foto)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new DomainException("NOME_OBRIGATORIO");

        if (string.IsNullOrWhiteSpace(cpf)) throw new DomainException("CPF_OBRIGATORIO");

        cpf = TextoNormalizadoService.LimparEDigitos(cpf);

        if (cpf.Length != 11) throw new DomainException("CPF_DIGITOS");

        if (dataNascimento > DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("Data de nascimento não pode ser no futuro.");

        if (dataNascimento < DateOnly.FromDateTime(new DateTime(1900, 1, 1))) throw new ArgumentException("Data de nascimento muito antiga.");

        if (string.IsNullOrWhiteSpace(telefone)) throw new ArgumentException("Telefone não pode ser nulo ou vazio.");

        if (!string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email não pode ser nulo ou vazio.");

        if(endereco == null) throw new ArgumentNullException(nameof(endereco));

        numero = TextoNormalizadoService.LimparEDigitos(numero);

        if (string.IsNullOrWhiteSpace(senha)) throw new ArgumentException("senha não pode ser nulo ou vazio.");

        if (foto == null) throw new ArgumentNullException(nameof(foto));

        return new Aluno(nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto);
    }
}
