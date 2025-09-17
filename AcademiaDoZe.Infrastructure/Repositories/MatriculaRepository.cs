//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Repositories;
using System.Runtime.ConstrainedExecution;
using System.Reflection.PortableExecutable;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe;
using AcademiaDoZe.Domain.ValueObject;

namespace AcedemiaDoZe.Infrastructure.Repositories
{
    public class MatriculaRepository : BaseRepository<Matricula>, IMatriculaRepository 
    {
        private readonly IAlunoRepository _alunoRepository;

        public MatriculaRepository(string connectionString, DatabaseType databaseType, IAlunoRepository alunoRepository)
            : base(connectionString, databaseType)
        {
            _alunoRepository = alunoRepository;
        }
        public async Task<IEnumerable<Matricula>> ObterPorAluno(int alunoId)
        {
            try
            {
                var matriculas = new List<Matricula>();

                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE aluno_id = @AlunoId";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", alunoId, DbType.Int32, _databaseType));

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var matricula = await MapAsync(reader);
                    if (matricula != null)
                        matriculas.Add(matricula);
                }

                return matriculas;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_OBTER_POR_ALUNO", ex);
            }
        }

        public async Task LimparTabela()
        {
            var sql = "DELETE FROM tb_matricula;";

            try
            {
                // 1. Abre a conexão com o banco de dados
                await using var connection = await GetOpenConnectionAsync();

                // 2. Cria um comando SQL com a instrução DELETE
                await using var command = connection.CreateCommand();
                command.CommandText = sql;

                // 3. Executa o comando para deletar todos os registros
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                // Se houver alguma falha, você pode tratar aqui
                throw new InvalidOperationException("Erro ao limpar a tabela de matriculas.", ex);
            }
        }


        public async Task<IEnumerable<Matricula>> ObterAtivas(int idAluno = 0)
        {
            var matriculas = new List<Matricula>();

            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = $@"
            SELECT * 
            FROM {TableName} 
            WHERE data_fim >= {(_databaseType == DatabaseType.SqlServer ? "GETDATE()" : "CURRENT_DATE()")} 
            {(idAluno > 0 ? "AND aluno_id = @id" : "")}";

                await using var command = DbProvider.CreateCommand(query, connection);

                if (idAluno > 0)
                {
                    var param = command.CreateParameter();
                    param.ParameterName = "@id";
                    param.Value = idAluno;
                    command.Parameters.Add(param);
                }

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var matricula = await MapAsync(reader);
                    matriculas.Add(matricula);
                }
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_OBTER_MATRICULAS_ATIVAS", ex);
            }

            return matriculas;
        }
        public async Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias)
        {
            var matriculas = new List<Matricula>();

            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query;

                if (_databaseType == DatabaseType.SqlServer)
                {
                    query = $@"
                SELECT * FROM {TableName} WHERE data_fim BETWEEN GETDATE() AND DATEADD(day, @dias, GETDATE())";
                }
                else 
                {
                    query = $@"
                SELECT * FROM {TableName} WHERE data_fim BETWEEN CURRENT_DATE AND CURRENT_DATE + INTERVAL @dias day";
                }

                await using var command = DbProvider.CreateCommand(query, connection);

                command.Parameters.Add(DbProvider.CreateParameter("@dias", dias, DbType.Int32, _databaseType));

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var matricula = await MapAsync(reader);
                    if (matricula != null)
                        matriculas.Add(matricula);
                }
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException(
                    $"ERRO_OBTER_MATRICULAS_VENCENDO_EM_{dias}_DIAS", ex
                );
            }

            return matriculas;
        }

        public override async Task<Matricula> Adicionar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                    ? $"INSERT INTO {TableName} (aluno_id, plano, data_inicio, data_fim, objetivo, restricao_medica, obs_restricao, laudo_medico) " +
                      "OUTPUT INSERTED.id_matricula " +
                      "VALUES (@AlunoId, @Plano, @DataInicio, @DataFim, @Objetivo, @RestricaoMedica, @ObsRestricao, @LaudoMedico);"
                    : $"INSERT INTO {TableName} (aluno_id, plano, data_inicio, data_fim, objetivo, restricao_medica, obs_restricao, laudo_medico) " +
                      "VALUES (@AlunoId, @Plano, @DataInicio, @DataFim, @Objetivo, @RestricaoMedica, @ObsRestricao, @LaudoMedico); " +
                      "SELECT LAST_INSERT_ID();";

                await using var command = DbProvider.CreateCommand(query, connection);

                command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", entity.AlunoMatricula.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataInicio", entity.DataInicio, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataFim", entity.DataFim, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@RestricaoMedica", (int)entity.RestricoesMedicas, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@ObsRestricao", entity.ObservacoesRestricoes ?? string.Empty, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@LaudoMedico", entity.LaudoMedico?.Conteudo ?? (object)DBNull.Value, DbType.Binary, _databaseType));

                var id = await command.ExecuteScalarAsync();

                if (id != null && id != DBNull.Value)
                {
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(id));
                }

                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_ADD_MATRICULA", ex);
            }
        }

        public override async Task<Matricula> Atualizar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = $"UPDATE {TableName} "
                             + "SET aluno_id = @AlunoId, "
                             + "plano = @Plano, "
                             + "data_inicio = @DataInicio, "
                             + "data_fim = @DataFim, "
                             + "objetivo = @Objetivo, "
                             + "restricao_medica = @RestricaoMedica, "
                             + "obs_restricao = @ObsRestricao, "
                             + "laudo_medico = @LaudoMedico "
                             + "WHERE id_matricula = @Id";

                await using var command = DbProvider.CreateCommand(query, connection);

                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", entity.AlunoMatricula.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataInicio", entity.DataInicio, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataFim", entity.DataFim, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@RestricaoMedica", (int)entity.RestricoesMedicas, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@ObsRestricao", entity.ObservacoesRestricoes ?? string.Empty, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@LaudoMedico", entity.LaudoMedico?.Conteudo ?? (object)DBNull.Value, DbType.Binary, _databaseType));

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"MATRICULA_NAO_ENCONTRADA_ID_{entity.Id}");
                }

                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_UPDATE_MATRICULA", ex);
            }
        }


        protected override async Task<Matricula> MapAsync(DbDataReader reader)
        {
            try
            {
                var alunoId = Convert.ToInt32(reader["aluno_id"]);
                var aluno = await _alunoRepository.ObterPorId(alunoId);

                var matricula = Matricula.Criar(
                    0,
                    aluno,
                    (TipoPlano)Convert.ToInt32(reader["plano"]),
                    DateOnly.FromDateTime(Convert.ToDateTime(reader["data_inicio"])),
                    DateOnly.FromDateTime(Convert.ToDateTime(reader["data_fim"])),
                    reader["objetivo"].ToString()!,
                    (Restricoes)Convert.ToInt32(reader["restricao_medica"]),
                    reader["obs_restricao"].ToString(),
                    reader["laudo_medico"] == DBNull.Value
                        ? null
                        : Arquivo.Criar((byte[])reader["laudo_medico"])
                );

                var idProperty = typeof(Entity).GetProperty("Id");
                idProperty?.SetValue(matricula, Convert.ToInt32(reader["id_matricula"]));

                return matricula;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao mapear dados da matrícula: {ex.Message}", ex);
            }
        }
    }
}