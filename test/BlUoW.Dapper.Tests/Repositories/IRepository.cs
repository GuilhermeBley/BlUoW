namespace BlUoW.Dapper.Tests.Repositories;

public interface IRepository<TEntity, TModel, TId>
    where TEntity : class
    where TModel : class
{
    Task<TModel> AddAsync(TEntity model);
    Task<TModel> UpdateAsync(TId id, TEntity model);
    Task<TModel?> DeleteAsync(TId id);
    Task<TModel?> GetByIdOrEmptyAsync(TId id);
    Task<IEnumerable<TModel>> GetAllAsync();
}