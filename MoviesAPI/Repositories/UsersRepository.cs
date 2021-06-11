using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DataAccess;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.Repositories.Interfaces;

namespace MoviesAPI.Repositories
{
    public class UsersRepository: IUsersRepository
    {
        private readonly MoviesDbContext _dbContext;

        public UsersRepository(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity> GetUserByIdAsync(long userId)
        {
            return await _dbContext
                .Users
                //Since there is no update method, for now, let's not track entity
                .AsNoTracking()
                .SingleOrDefaultAsync(user => user.UserId == userId);
        }
    }
}
