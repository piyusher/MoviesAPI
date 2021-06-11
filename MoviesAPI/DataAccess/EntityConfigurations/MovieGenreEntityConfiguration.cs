using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.DataAccess.EntityConfigurations
{
    public class MovieGenreEntityConfiguration : IEntityTypeConfiguration<MovieGenreEntity>
    {
        public void Configure(EntityTypeBuilder<MovieGenreEntity> builder)
        {
            builder.ToTable("MovieGenres");

            builder.HasKey(x => new
            {
                x.GenreId,
                x.MovieId
            });

            builder.Property(x => x.MovieId)
                .IsRequired();

            builder.Property(x => x.GenreId)
                .IsRequired();

            builder.HasOne(x => x.Movie)
                .WithMany(x => x.MovieGenres)
                .HasForeignKey(x => x.MovieId);

            builder.HasOne(x => x.Genre)
                .WithMany(x => x.MovieGenres)
                .HasForeignKey(x => x.GenreId);
        }
    }
}
