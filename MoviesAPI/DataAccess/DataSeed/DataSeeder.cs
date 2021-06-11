using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DataAccess.Entities;
using Serilog;

namespace MoviesAPI.DataAccess.DataSeed
{
    [ExcludeFromCodeCoverage]
    public static class DataSeeder
    {
        //The data that needs to be seeded is in the JSON file. 
        //We are deserializing the JSON to form collection of entities that will be added to DB.

        private static readonly string JsonFeed =
            File.ReadAllText($"{AppContext.BaseDirectory}/DataAccess/DataSeed/SeedData.json",Encoding.UTF8);

        public static readonly SeedDataModel Data = JsonSerializer
            .Deserialize<SeedDataModel>(JsonFeed, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            });
       
        //The public extension method to be used to seed data
        public static void Seed(this ModelBuilder modelBuilder)
        {
            try
            {
                Log.Information("Starting to seed data");

                SeedGenres(modelBuilder);
                SeedUsers(modelBuilder);
                SeedMovies(modelBuilder);
                SeedMovieGenres(modelBuilder);
                SeedRatings(modelBuilder);

                Log.Information("Data seeded successfully");

            }
            catch (Exception ex)
            {
                Log.Fatal("Exception while seeding the data : {@Exception}", ex);
            }
        }

        private static void SeedMovies(ModelBuilder modelBuilder)
        {

            Log.Information("Seeding Movies");

            //Cast to core entity before seeding
            var movies = Data.Movies.Select(movie => new MovieEntity()
            {
                MovieId = movie.MovieId,
                Runtime = movie.Runtime,
                Title = movie.Title,
                Year = movie.Year,
                //Calculate average rating based on current data
                AvgRating = Data.Ratings
                        .Where(rating => rating.MovieId == movie.MovieId)
                        .Select(x => (decimal)x.Rating)
                        .DefaultIfEmpty()
                        .Average()

            }).ToList();
            modelBuilder.Entity<MovieEntity>().HasData(movies.ToArray());
        }

        private static void SeedGenres(ModelBuilder modelBuilder)
        {
            Log.Information("Seeding Genres");
            modelBuilder.Entity<GenreEntity>().HasData(Data.Genres.ToArray());
        }

        private static void SeedUsers(ModelBuilder modelBuilder)
        {

            Log.Information("Seeding Users");
            modelBuilder.Entity<UserEntity>().HasData(Data.Users.ToArray());
        }

        private static void SeedRatings(ModelBuilder modelBuilder)
        {
            Log.Information("Seeding Ratings");
            modelBuilder.Entity<MovieRatingEntity>().HasData(Data.Ratings.ToArray());
        }

        private static void SeedMovieGenres(ModelBuilder modelBuilder)
        {

            Log.Information("Assigning genres to movies");
            //generate MovieGenre entities using JSON data

            var movieGenresCollection = (
                from movie in Data.Movies 
                from genre in movie.Genres
                let genreId = Data.Genres
                                  .SingleOrDefault(x => x.GenreName == genre)?
                                  .GenreId ?? 0 where genreId != 0 
                select new MovieGenreEntity() 
                    {GenreId = genreId, MovieId = movie.MovieId})
                .ToList();

            modelBuilder.Entity<MovieGenreEntity>().HasData(movieGenresCollection.ToArray());
        }

    }
}
