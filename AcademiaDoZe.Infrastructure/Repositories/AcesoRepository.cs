//AUGUSTO DOS SANTOS CORREA
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AcademiaDoZe;
using AcademiaDoZe.AcademiaDoZe;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Domain.ValueObject;
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Repositories;

namespace AcedemiaDoZe.Infrastructure.Repositories
{
    public class AcessoRepository : BaseRepository<Acesso>, IAcessoRepository
    {
        public AcessoRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType) { }

        public override async Task<Acesso> Adicionar(Acesso entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                    ? $"INSERT INTO tb_acesso (pessoa_tipo, pessoa_id, data_hora) " +
                      "OUTPUT INSERTED.id_acesso " +
                      "VALUES (@PessoaTipo, @PessoaId, @DataHora);"
                    : $"INSERT INTO tb_acesso (pessoa_tipo, pessoa_id, data_hora) " +
                      "VALUES (@PessoaTipo, @PessoaId, @DataHora); " +
                      "SELECT LAST_INSERT_ID();";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@PessoaId", entity.AlunoColaborador.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataHoraEntrada", entity.DataHoraEntrada, DbType.DateTime, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataHoraSaida", entity.DataHoraSaida, DbType.DateTime, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));

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
                throw new InvalidOperationException("ERRO_ADD_ACESSO", ex);
            }
        }

        public override async Task<Acesso> Atualizar(Acesso entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = $"UPDATE {TableName} " +
                               "SET pessoa_id = @PessoaId, " +
                               "data_hora_entrada = @DataHoraEntrada, " +
                               "data_hora_saida = @DataHoraSaida " +
                               "WHERE id_acesso = @Id;";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@PessoaId", entity.AlunoColaborador.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataHoraEntrada", entity.DataHoraEntrada, DbType.DateTime, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@DataHoraSaida", entity.DataHoraSaida, DbType.DateTime, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));

                await command.ExecuteNonQueryAsync();

                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_ATUALIZAR_ACESSO", ex);
            }
        }

        public override async Task<bool> Remover(int id)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"DELETE FROM tb_acesso WHERE id_acesso = @Id";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Id", id, DbType.Int32, _databaseType));

                var rows = await command.ExecuteNonQueryAsync();
                return rows > 0;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_REMOVER_ACESSO", ex);
            }
        }

        public async Task<IEnumerable<Acesso>> GetAcessosPorAlunoPeriodo(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE pessoa_tipo = 0";
                if (alunoId.HasValue) query += " AND pessoa_id = @AlunoId";
                if (inicio.HasValue) query += " AND data_hora >= @Inicio";
                if (fim.HasValue) query += " AND data_hora <= @Fim";

                await using var command = DbProvider.CreateCommand(query, connection);
                if (alunoId.HasValue) command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", alunoId.Value, DbType.Int32, _databaseType));
                if (inicio.HasValue) command.Parameters.Add(DbProvider.CreateParameter("@Inicio", inicio.Value.ToDateTime(TimeOnly.MinValue), DbType.DateTime, _databaseType));
                if (fim.HasValue) command.Parameters.Add(DbProvider.CreateParameter("@Fim", fim.Value.ToDateTime(TimeOnly.MaxValue), DbType.DateTime, _databaseType));

                var result = new List<Acesso>();
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    MapAsync(reader);
                }
                return result;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_GET_ACESSOS_ALUNO", ex);
            }
        }

        public async Task<IEnumerable<Acesso>> GetAcessosPorColaboradorPeriodo(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE pessoa_tipo = 1";
                if (colaboradorId.HasValue) query += " AND pessoa_id = @ColaboradorId";
                if (inicio.HasValue) query += " AND data_hora >= @Inicio";
                if (fim.HasValue) query += " AND data_hora <= @Fim";

                await using var command = DbProvider.CreateCommand(query, connection);
                if (colaboradorId.HasValue) command.Parameters.Add(DbProvider.CreateParameter("@ColaboradorId", colaboradorId.Value, DbType.Int32, _databaseType));
                if (inicio.HasValue) command.Parameters.Add(DbProvider.CreateParameter("@Inicio", inicio.Value.ToDateTime(TimeOnly.MinValue), DbType.DateTime, _databaseType));
                if (fim.HasValue) command.Parameters.Add(DbProvider.CreateParameter("@Fim", fim.Value.ToDateTime(TimeOnly.MaxValue), DbType.DateTime, _databaseType));

                var result = new List<Acesso>();
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(await MapAsync(reader));
                }
                return result;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_GET_ACESSOS_COLABORADOR", ex);
            }
        }

        public async Task<Dictionary<TimeOnly, int>> GetHorarioMaisProcuradoPorMes(int mes)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query = _databaseType == DatabaseType.SqlServer
                    ? $@"
                SELECT CAST(data_hora AS TIME) AS horario, COUNT(*) AS quantidade
                FROM {TableName}
                WHERE MONTH(data_hora) = @Mes
                GROUP BY CAST(data_hora AS TIME)
                ORDER BY quantidade DESC"
                    : $@"
                SELECT TIME(data_hora) AS horario, COUNT(*) AS quantidade
                FROM {TableName}
                WHERE MONTH(data_hora) = @Mes
                GROUP BY TIME(data_hora)
                ORDER BY quantidade DESC";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Mes", mes, DbType.Int32, _databaseType));

                var result = new Dictionary<TimeOnly, int>();

                await using var reader = await command.ExecuteReaderAsync();
                int horarioIndex = reader.GetOrdinal("horario");
                int quantidadeIndex = reader.GetOrdinal("quantidade");

                while (await reader.ReadAsync())
                {
                    var horario = TimeOnly.FromTimeSpan(reader.GetFieldValue<TimeSpan>(horarioIndex));
                    var quantidade = reader.GetInt32(quantidadeIndex);
                    result[horario] = quantidade;
                }

                return result;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_GET_HORARIO_MAIS_PROCURADO", ex);
            }
        }

        public async Task<Dictionary<int, TimeSpan>> GetPermanenciaMediaPorMes(int mes)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = "/* Aqui depende de como você armazena a saída dos alunos para calcular permanência */";
                throw new NotImplementedException("Cálculo de permanência precisa dos dados de saída.");
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_GET_PERMANENCIA_MEDIA", ex);
            }
        }
        public async Task<IEnumerable<Aluno>> GetAlunosSemAcessoNosUltimosDias(int dias)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT a.* FROM tb_aluno a " +
                               $"WHERE NOT EXISTS (SELECT 1 FROM {TableName} ac WHERE ac.pessoa_tipo = 0 AND ac.pessoa_id = a.id_aluno " +
                               "AND ac.data_hora >= DATE_SUB(NOW(), INTERVAL @Dias DAY))";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Dias", dias, DbType.Int32, _databaseType));

                var result = new List<Aluno>();
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // Mapeamento de Aluno direto no método
                    int id = reader.GetInt32(reader.GetOrdinal("id_aluno"));
                    string nome = reader.GetString(reader.GetOrdinal("nome"));
                    string cpf = reader.GetString(reader.GetOrdinal("cpf"));
                    DateOnly dataNascimento = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data_nascimento")));
                    string telefone = reader.GetString(reader.GetOrdinal("telefone"));
                    string email = reader.GetString(reader.GetOrdinal("email"));

                    string logradouro = reader.GetString(reader.GetOrdinal("logradouro"));
                    string bairro = reader.GetString(reader.GetOrdinal("bairro"));
                    string cidade = reader.GetString(reader.GetOrdinal("cidade"));
                    string estado = reader.GetString(reader.GetOrdinal("estado"));
                    string cep = reader.GetString(reader.GetOrdinal("cep"));
                    string pais = reader.GetString(reader.GetOrdinal("pis"));

                    var endereco = Logradouro.Criar(id, cep, nome, bairro, cidade, estado, pais);

                    string numero = reader.GetString(reader.GetOrdinal("numero"));
                    string complemento = reader.IsDBNull(reader.GetOrdinal("complemento"))
                        ? string.Empty
                        : reader.GetString(reader.GetOrdinal("complemento"));

                    string senha = reader.GetString(reader.GetOrdinal("senha"));
                    string objetivo = reader.GetString(reader.GetOrdinal("objetivo"));

                  
                    byte[] conteudo = (byte[])reader["foto_bytes"];


                    var foto = Arquivo.Criar(conteudo);

                    var aluno = Aluno.Criar(id, nome, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha,  foto);

                    result.Add(aluno);
                }

                return result;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException("ERRO_GET_ALUNOS_SEM_ACESSO", ex);
            }
        }

        protected override async Task<Acesso> MapAsync(DbDataReader reader)
        {
            try
            {
                var pessoaTipo = reader.GetInt32(reader.GetOrdinal("pessoa_tipo"));
                var pessoaId = reader.GetInt32(reader.GetOrdinal("pessoa_id"));

                // Montar endereço
                var endereco = Logradouro.Criar(
                    id: pessoaId,
                    cep: reader["cep"].ToString()!,
                    nome: reader["logradouro_nome"].ToString()!,
                    bairro: reader["bairro"].ToString()!,
                    cidade: reader["cidade"].ToString()!,
                    estado: reader["estado"].ToString()!,
                    pais: reader["pais"].ToString()!
                );

                Pessoa pessoa;

                if (pessoaTipo == 0) // ALUNO
                {
                    pessoa = Aluno.Criar(
                        id: pessoaId,
                        nome: reader["nome"].ToString()!,
                        cpf: reader["cpf"].ToString()!,
                        dataNascimento: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data_nascimento"))),
                        telefone: reader["telefone"].ToString()!,
                        email: reader["email"].ToString()!,
                        endereco: endereco,
                        numero: reader["numero"].ToString()!,
                        complemento: reader["complemento"].ToString()!,
                        senha: reader["senha"].ToString()!,
                        foto: Arquivo.Criar((byte[])reader["foto"]) 
                    );
                }
                else 
                {
                    pessoa = AcademiaDoZe.Domain.Entities.Colaborador.Criar(
                        id: pessoaId,
                        nome: reader["nome"].ToString()!,
                        cpf: reader["cpf"].ToString()!,
                        dataNascimento: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data_nascimento"))),
                        telefone: reader["telefone"].ToString()!,
                        email: reader["email"].ToString()!,
                        endereco: endereco,
                        numero: reader["numero"].ToString()!,
                        complemento: reader["complemento"].ToString()!,
                        senha: reader["senha"].ToString()!,
                        foto: Arquivo.Criar((byte[])reader["foto"]),
                        dataAdmissao: DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data_admissao"))),
                        tipo: (TipoUsuario)reader.GetInt32(reader.GetOrdinal("tipo_usuario")),
                        vinculo: (TipoColaborador)reader.GetInt32(reader.GetOrdinal("tipo_colaborador"))
                    );
                }

                // Datas de acesso
                var dataHoraEntrada = reader.GetDateTime(reader.GetOrdinal("data_hora_entrada"));
                var dataHoraSaida = reader.IsDBNull(reader.GetOrdinal("data_hora_saida"))
                    ? (DateTime?)null
                    : reader.GetDateTime(reader.GetOrdinal("data_hora_saida"));

                // Criar acesso
                var acesso = Acesso.Criar(pessoa, dataHoraEntrada, dataHoraSaida);

                // Definir ID (via reflexão porque é protegido)
                var idProperty = typeof(Entity).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                idProperty?.SetValue(acesso, reader.GetInt32(reader.GetOrdinal("id_acesso")));

                return acesso;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao mapear dados de acesso: {ex.Message}", ex);
            }
        }
    }
}
