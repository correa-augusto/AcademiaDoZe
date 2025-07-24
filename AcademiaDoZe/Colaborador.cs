using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.AcademiaDoZe;

namespace AcademiaDoZe
{
    public class Colaborador : Pessoa
    {
        private DateOnly _dataAdmissao;
        public TipoColaborador Tipo { get; set; }


        public DateOnly DataAdmissao
        {
            get => _dataAdmissao;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Today))
                    throw new ArgumentException("Data de admissão não pode ser no futuro.");
                _dataAdmissao = value;
            }
        }

        public string TipoColaborador
        {
            get => TipoColaborador;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tipo do colaborador não pode ser nulo ou vazio.");
                   TipoColaborador = value;
            }
        }

        public Colaborador(
            int id,
            string nome,
            string cpf,
            DateOnly dataNascimento,
            string telefone,
            string email,
            string senha,
            Endereco endereco,
            DateOnly dataAdmissao,
            string tipoColaborador,
            string vinculoColaborador,
            string? foto = null
            ) : base(id, nome, cpf, dataNascimento, telefone, email, senha, endereco, foto)
                {
                     DataAdmissao = dataAdmissao;
                     TipoColaborador = tipoColaborador;
                }
    }
}
