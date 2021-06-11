using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DataAccess.DataSeed;
using MoviesAPI.DataAccess.Entities;
using MoviesAPI.DataAccess.EntityConfigurations;

namespace MoviesAPI.DataAccess
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<MovieEntity> Movies { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<GenreEntity> Genres { get; set; }

        public DbSet<MovieRatingEntity> MovieRatings { get; set; }

        public DbSet<MovieGenreEntity> MovieGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MovieEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GenreEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MovieRatingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MovieGenreEntityConfiguration());

            //Seed data using extension method
            modelBuilder.Seed();
        }


    }
}
