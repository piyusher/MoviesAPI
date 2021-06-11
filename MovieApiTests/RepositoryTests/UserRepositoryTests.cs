using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DataAccess;
using MoviesAPI.DataAccess.DataSeed;
using MoviesAPI.Repositories;
using MoviesAPI.Repositories.Interfaces;
using Xunit;

namespace MovieApiTests.RepositoryTests
{
    public class UserRepositoryTests:IDisposable
    {
        private readonly IUsersRepository _usersRepo;
        private readonly MoviesDbContext _dbContext;

        public UserRepositoryTests()
        {
            _dbContext = TestDbContext.GetInMemoryDbContext();
            _usersRepo = new UsersRepository(_dbContext);
        }

        [Fact]
        public async Task GetUserByIdReturnsNullWhenNotFound()
        {
            //user 55 is not in seeded data
            var userEntity = await _usersRepo.GetUserByIdAsync(55);
            userEntity.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByIdReturnsUser()
        {
            //User 1 is there in seeded data
            var userEntity = await _usersRepo.GetUserByIdAsync(1);
            userEntity.Should().NotBeNull();
            userEntity.Should().BeEquivalentTo(DataSeeder.Data
                .Users
                .Single(x=>x.UserId ==1));
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
