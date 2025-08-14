using AcademiaDoZe.Domain.Entities;
namespace AcademiaDoZe.Domain.Repositories
{
    public interface ILogradouroRepository : IRepository<Endereco>
    {
        // Métodos específicos do domínio

        Task<Endereco?> ObterPorCep(string cep);

        Task<IEnumerable<Endereco>> ObterPorCidade(string cidade);
    }
}