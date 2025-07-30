//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Domain.Enums;

namespace AcademiaDoZe
{
    public class Usuario
    {
        public int Id { get; }
        public string Nome { get; }
        public TipoUsuario Tipo { get; }

        public List<string> ObterPermissoes()
        {
            switch (Tipo)
            {
                case TipoUsuario.Administrador:
                    return new List<string>
                    {
                        "Acesso total"
                    };

                case TipoUsuario.Atendente:
                    return new List<string>
                    {
                        "Cadastrar alunos",
                        "Realizar matrícula",
                        "Registrar entrada/saída de aluno",
                        "Registrar entrada/saída própria"
                    };

                case TipoUsuario.Instrutor:
                    return new List<string>
                    {
                        "Registrar entrada/saída de aluno",
                        "Registrar entrada/saída própria"
                    };

                case TipoUsuario.Aluno:
                    return new List<string>
                    {
                        "Consultar dados estatísticos",
                        "Registrar entrada/saída própria"
                    };

                default:
                    return new List<string>();
            }
        }

        public bool PodeExecutar(string acao)
        {
            return ObterPermissoes().Contains(acao);
        }

        public void VerificarPermissao(string acao)
        {
            if (PodeExecutar(acao))
            {
                Console.WriteLine($"{Nome} tem permissão para: {acao}");
            }
            else
            {
                Console.WriteLine($"{Nome} NÃO tem permissão para: {acao}");
            }
        }
    }
}
