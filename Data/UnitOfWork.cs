using Backend.Pattern.Interfaces;
using Backend.Pattern.Repository;

namespace Backend.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IUserRepository UserRepository => new UserRepository(_dataContext);
		public ICategoryRepository CategoryRepository => new CategoryRepository(_dataContext);
		public IProductRepository ProductRepository => new ProductRepository(_dataContext);
		public IOrderRepository OrderRepository => new OrderRepository(_dataContext);
		public async Task<bool> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
