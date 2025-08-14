using AcademiaDoZe.Domain.Entities;
namespace AcademiaDoZe.Domain.Repositories
{
    // Interface generic para reposit�rios, permite a cria��o de reposit�rios espec�ficos para cada entidade do dom�nio.
    // Define os contratos essenciais para a persist�ncia de dados.
    // Delega a implementa��o para a camada de infraestrutura, o que � correto.
    // Herda de Entity para garantir que TEntity seja uma entidade v�lida, e seu uso somente no domain.
    // M�todos ass�ncronos (Task), alinhados com pr�ticas modernas de acesso a dados.
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity?> ObterPorId(int id);
        Task<IEnumerable<TEntity>> ObterTodos();
        Task<TEntity> Adicionar(TEntity entity);
        Task<TEntity> Atualizar(TEntity entity);
        Task<bool> Remover(int id);
    }
}