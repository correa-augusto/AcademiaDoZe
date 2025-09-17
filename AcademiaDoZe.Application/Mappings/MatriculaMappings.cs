using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.ValueObject;
namespace AcademiaDoZe.Application.Mappings
{
    public static class MatriculaMappings
    {
        public static MatriculaDTO ToDto(this Matricula matricula)
        {
            return new MatriculaDTO
            {
                Id = matricula.Id,
                AlunoMatricula = matricula.AlunoMatricula.ToDto(),
                Plano = matricula.Plano.ToApp(),
                DataInicio = matricula.DataInicio,
                DataFim = matricula.DataFim,
                Objetivo = matricula.Objetivo,
                RestricoesMedicas = matricula.RestricoesMedicas.ToApp(),
                ObservacoesRestricoes = matricula.ObservacoesRestricoes,
                LaudoMedico = matricula.LaudoMedico != null ? new ArquivoDTO { Conteudo = matricula.LaudoMedico.Conteudo } : null, // Mapeia laudo para DTO
            };
        }
        public static Matricula ToEntity(this MatriculaDTO matriculaDto)
        {
            return Matricula.Criar(
                matriculaDto.Id,
                matriculaDto.AlunoMatricula.ToEntityMatricula(),
                matriculaDto.Plano.ToDomain(),
                matriculaDto.DataInicio,
                matriculaDto.DataFim,
                matriculaDto.Objetivo,
                matriculaDto.RestricoesMedicas.ToDomain(),
                matriculaDto.ObservacoesRestricoes!,
                (matriculaDto.LaudoMedico?.Conteudo != null)
                    ? Arquivo.Criar(matriculaDto.LaudoMedico.Conteudo)
                    : null
            );
        }
        public static Matricula UpdateFromDto(this Matricula matricula, MatriculaDTO matriculaDto)
        {
            return Matricula.Criar(
                matricula.Id, // mantém o ID original
                matriculaDto.AlunoMatricula?.ToEntityMatricula() ?? matricula.AlunoMatricula,
                matriculaDto.Plano != default ? matriculaDto.Plano.ToDomain() : matricula.Plano,
                matriculaDto.DataInicio != default ? matriculaDto.DataInicio : matricula.DataInicio,
                matriculaDto.DataFim != default ? matriculaDto.DataFim : matricula.DataFim,
                matriculaDto.Objetivo ?? matricula.Objetivo,
                matriculaDto.RestricoesMedicas.ToDomain(),
                matriculaDto.ObservacoesRestricoes ?? matricula.ObservacoesRestricoes,   
                (matriculaDto.LaudoMedico?.Conteudo != null)
                    ? Arquivo.Criar(matriculaDto.LaudoMedico.Conteudo)
                    : matricula.LaudoMedico                                          
            );
        }
    }
}