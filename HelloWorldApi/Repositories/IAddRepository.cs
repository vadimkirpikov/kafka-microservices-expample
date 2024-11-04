namespace HelloWorldApi.Repositories;

public interface IAddRepository<in T>
{
    Task AddAsync(T entity);
}