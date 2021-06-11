using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.DataAccess.EntityConfigurations
{
    public class MovieEntityConfiguration: IEntityTypeConfiguration<MovieEntity>
    {
        public void Configure(EntityTypeBuilder<MovieEntity> builder)
        {
            builder.ToTable("Movies");

            builder.HasKey(x => x.MovieId);

            builder.Property(x => x.MovieId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Title)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(500);

            builder.Property(x => x.AvgRating)
                .HasDefaultValue(0)
                .HasPrecision(1, 3)
                //For SQLLite
                .HasConversion<double>();

            builder.Property(x => x.Runtime)
                .IsRequired();

            builder.Property(x => x.Year)
                .IsRequired();

        }
    }
}
