using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe
{
    public class Endereco
    {
        private string _cep;
        private string _pais;
        private string _estado;
        private string _cidade;
        private string _bairro;
        private string _nomeLogradouro;
        private string _numero;
        private string? _complemento;

        public string CEP
        {
            get => _cep;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CEP não pode ser nulo ou vazio.");
                _cep = value;
            }
        }

        public string Pais
        {
            get => _pais;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("País não pode ser nulo ou vazio.");
                _pais = value;
            }
        }

        public string Estado
        {
            get => _estado;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Estado não pode ser nulo ou vazio.");
                _estado = value;
            }
        }

        public string Cidade
        {
            get => _cidade;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Cidade não pode ser nula ou vazia.");
                _cidade = value;
            }
        }

        public string Bairro
        {
            get => _bairro;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Bairro não pode ser nulo ou vazio.");
                _bairro = value;
            }
        }

        public string NomeLogradouro
        {
            get => _nomeLogradouro;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome do logradouro não pode ser nulo ou vazio.");
                _nomeLogradouro = value;
            }
        }

        public string Numero
        {
            get => _numero;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Número não pode ser nulo ou vazio.");
                _numero = value;
            }
        }

        public string? Complemento
        {
            get => _complemento;
            set => _complemento = value; // opcional
        }

        public Endereco(string cep, string pais, string estado, string cidade, string bairro, string nomeLogradouro, string numero, string? complemento = null)
        {
            CEP = cep;
            Pais = pais;
            Estado = estado;
            Cidade = cidade;
            Bairro = bairro;
            NomeLogradouro = nomeLogradouro;
            Numero = numero;
            Complemento = complemento;
        }

    }
}
