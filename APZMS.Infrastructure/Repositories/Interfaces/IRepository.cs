namespace APZMS.Infrastructure.Repositories.Interfaces
{
    //IRepository: This is the name of the interface. The means it’s generic — it can work with any type(T) like Customer, Product, etc.
    //where T : class: This is a constraint — it says that T must be a reference type (i.e., a class, not a struct or primitive like int). This ensures we’re only using it with objects like entity classes.

    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        //Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T item);
        //void Update(T item);
        T Delete(T item);
        Task<int> SaveChangesAsync();
    }
}