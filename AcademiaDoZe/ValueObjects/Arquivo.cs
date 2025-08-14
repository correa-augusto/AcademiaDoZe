//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;

namespace AcademiaDoZe.Domain.ValueObject;
public record Arquivo
{
        public byte[] Conteudo { get; }
        private Arquivo(byte[] conteudo)
        {
            Conteudo = conteudo;
        }
        public static Arquivo Criar(byte[] conteudo)
        {
            const int tamanhoMaximoBytes = 5 * 1024 * 1024; // 5MB
            if (conteudo.Length > tamanhoMaximoBytes)
                throw new DomainException("ARQUIVO_TIPO_TAMANHO");
            // cria e retorna o objeto

            return new Arquivo(conteudo);

        }
}