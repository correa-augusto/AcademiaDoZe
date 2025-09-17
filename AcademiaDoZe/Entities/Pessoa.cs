//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObject;

namespace AcademiaDoZe
{
    namespace AcademiaDoZe
    {
        public abstract class Pessoa : Entity
        {
            public int id { get; }
            public string Nome { get; }
            public string Cpf { get; }
            public DateOnly DataNascimento { get;  }
            public string Telefone { get; }
            public string Email { get; }
            public Logradouro Endereco { get; }
            public string Numero { get;  }
            public string Complemento { get; }
            public string Senha { get; }
            public Arquivo Foto { get; }


            public Pessoa( int id,
                           string nome,
                           string cpf,
                           DateOnly dataNascimento,
                           string telefone,
                           string email,
                           Logradouro endereco,
                           string numero,
                           string complemento,
                           string senha,
                           Arquivo foto) : base()

            {
                Id = id;
                Nome = nome;
                Cpf = cpf;
                DataNascimento = dataNascimento;
                Telefone = telefone;
                Email = email;
                Endereco = endereco;
                Numero = numero;
                Complemento = complemento;
                Senha = senha;
                Foto = foto;
            }
        } 
    }
}