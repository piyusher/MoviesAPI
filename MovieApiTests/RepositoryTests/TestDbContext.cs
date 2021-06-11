using Microsoft.EntityFrameworkCore;
using System;
using MoviesAPI.DataAccess;
using MoviesAPI.Repositories;

namespace MovieApiTests.RepositoryTests
{
    public static class TestDbContext
    {
        public static MoviesDbContext GetInMemoryDbContext()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            //Every time we create a new database and destroy it
            dbContextOptionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dbContext = new MoviesDbContext(dbContextOptionsBuilder.Options);

            //This step will seed the tests data to in memory database
            //Refer to file DataAccess/DataSeed/SeedData.json in the API project
            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}
