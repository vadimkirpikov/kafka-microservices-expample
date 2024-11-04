using System.Threading.Tasks;

namespace HelloWorldApi.Repositories;

public interface IAddRepository<in T>
{
    void CreateTable();
    Task AddAsync(T entity);
}