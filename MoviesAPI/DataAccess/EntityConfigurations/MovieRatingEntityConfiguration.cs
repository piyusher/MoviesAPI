using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.DataAccess.EntityConfigurations
{
    public class MovieRatingEntityConfiguration : IEntityTypeConfiguration<MovieRatingEntity>
    {
        public void Configure(EntityTypeBuilder<MovieRatingEntity> builder)
        {
            builder.ToTable("MovieRatings");

            builder.HasKey(x=> new
            {
                x.UserId, x.MovieId
            });

            builder.Property(x => x.MovieId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.HasOne(x => x.Movie)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.MovieId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.MovieRatings)
                .HasForeignKey(x => x.UserId);
        }
    }
}
