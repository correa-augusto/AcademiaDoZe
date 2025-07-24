using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.AcademiaDoZe;

namespace AcademiaDoZe
{
    public class Aluno : Pessoa
    {
        public Aluno
            (
            int id,
            string nome,
            string cpf,
            DateOnly dataNascimento,
            string telefone,
            string email,
            string senha,
            Endereco endereco,
            string? foto = null
        ) : base(id, nome, cpf, dataNascimento, telefone, email, senha, endereco, foto)
        {
        }
    }
}
