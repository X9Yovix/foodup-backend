namespace Backend.Pattern.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task<bool> SaveChangesAsync();
    }
}
