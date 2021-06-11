using System.Threading.Tasks;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserEntity> GetUserByIdAsync(long userId);
    }
}
