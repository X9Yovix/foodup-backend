using Backend.Pattern.Interfaces;

namespace Backend.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
		ICategoryRepository CategoryRepository { get; }
		IProductRepository ProductRepository { get; }
		Task<bool> SaveChangesAsync();
    }
}
