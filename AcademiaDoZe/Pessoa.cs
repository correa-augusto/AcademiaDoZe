using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe
{
    namespace AcademiaDoZe
    {
        public abstract class Pessoa
        {
            private int _id;
            private string _nome;
            private string _cpf;
            private DateOnly _dataNascimento;
            private string _telefone;
            private string _email;
            private string _senha;
            private string? _foto;
            private Endereco _endereco;

            public int Id
            {
                get => _id;
                set => _id = value;
            }

            public string Nome
            {
                get => _nome;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Nome não pode ser nulo ou vazio.");
                    _nome = value;
                }
            }

            public string Cpf
            {
                get => _cpf;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("CPF não pode ser nulo ou vazio.");
                    _cpf = value;
                }
            }

            public DateOnly DataNascimento
            {
                get => _dataNascimento;
                set
                {
                    if (value > DateOnly.FromDateTime(DateTime.Today))
                        throw new ArgumentException("Data de nascimento não pode ser no futuro.");
                    if (value < DateOnly.FromDateTime(new DateTime(1900, 1, 1)))
                        throw new ArgumentException("Data de nascimento muito antiga.");
                    _dataNascimento = value;
                }
            }

            public string Telefone
            {
                get => _telefone;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Telefone não pode ser nulo ou vazio.");
                    _telefone = value;
                }
            }

            public string Email
            {
                get => _email;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Email não pode ser nulo ou vazio.");
                    _email = value;
                }
            }

            public string Senha
            {
                get => _senha;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Senha não pode ser nula ou vazia.");
                    _senha = value;
                }
            }

            public string? Foto
            {
                get => _foto;
                set => _foto = value; 
            }

            public Endereco Endereco
            {
                get => _endereco;
                set => _endereco = value ?? throw new ArgumentNullException(nameof(value), "Endereço não pode ser nulo.");
            }

            public Pessoa(
           int id,
           string nome,
           string cpf,
           DateOnly dataNascimento,
           string telefone,
           string email,
           string senha,
           Endereco endereco,
           string? foto = null
       )
            {
                Id = id;
                Nome = nome;
                Cpf = cpf;
                DataNascimento = dataNascimento;
                Telefone = telefone;
                Email = email;
                Senha = senha;
                Endereco = endereco;
                Foto = foto; 
            }
        }
    }
}