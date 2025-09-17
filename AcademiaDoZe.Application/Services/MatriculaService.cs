//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Application.Security;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;

namespace AcademiaDoZe.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly Func<IMatriculaRepository> _repoFactory;

        public MatriculaService(Func<IMatriculaRepository> repoFactory)
        {
            _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
        }

        public async Task<MatriculaDTO> ObterPorIdAsync(int id)
        {
            var matricula = await _repoFactory().ObterPorId(id);
            return (matricula != null) ? matricula.ToDto() : null!;

        }

        public async Task<IEnumerable<MatriculaDTO>> ObterTodasAsync()
        {
            var matriculas = await _repoFactory().ObterTodos();
            return [.. matriculas.Select(c => c.ToDto())];
        }

        public async Task<MatriculaDTO> AdicionarAsync(MatriculaDTO matriculaDto)
        {

            var matriculasAtivas = await _repoFactory().ObterAtivas(matriculaDto.AlunoMatricula.Id);

            if (matriculasAtivas.Any())

            {
                throw new InvalidOperationException($"Já existe uma matricula ativa para o aluno");
            }

            // Cria a entidade de domínio a partir do DTO
            var matricula = matriculaDto.ToEntity();
            // Salva no repositório
            await _repoFactory().Adicionar(matricula);
            // Retorna o DTO atualizado com o ID gerado
            return matricula.ToDto();

        }

        public async Task<MatriculaDTO> AtualizarAsync(MatriculaDTO matriculaDto)
        {

            var matriculasAtivas = await _repoFactory().ObterAtivas(matriculaDto.AlunoMatricula.Id);

            var matriculaExistente = matriculasAtivas.FirstOrDefault(m => m.Id == matriculaDto.Id);

            if (matriculaExistente == null)
                throw new InvalidOperationException("Nenhuma matrícula ativa encontrada para este aluno.");

            var matriculaAtualizada = matriculaExistente.UpdateFromDto(matriculaDto);

            // Atualiza no repositório
            await _repoFactory().Atualizar(matriculaAtualizada);

            return matriculaAtualizada.ToDto();

        }

        public async Task<bool> RemoverAsync(int id)
        {
            // Busca matrículas ativas do aluno pela matrícula em questão
            var matriculasAtivas = await _repoFactory().ObterAtivas();

            // Localiza a matrícula específica
            var matriculaExistente = matriculasAtivas.FirstOrDefault(m => m.Id == id);

            if (matriculaExistente == null)
                throw new InvalidOperationException("Nenhuma matrícula ativa encontrada com este ID.");

            // Remove do repositório
            await _repoFactory().Remover(matriculaExistente.Id);

            return true;
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterPorAlunoIdAsync(int alunoId)
        {
            
            var matriculas = await _repoFactory().ObterPorAluno(alunoId);

           
            return matriculas.Select(m => m.ToDto());
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterAtivasAsync(int alunoId)
        {
            // Obtém todas as matrículas ativas do repositorio
            var matriculasAtivas = await _repoFactory().ObterAtivas(alunoId);

            // Converte cada entidade para DTO
            return matriculasAtivas.Select(m => m.ToDto());
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterVencendoEmDiasAsync(int dias)
        {
            var matriculas = await _repoFactory().ObterVencendoEmDias(dias);

            // Converte cada entidade para DTO
            return matriculas.Select(m => m.ToDto());
        }
    }
}
