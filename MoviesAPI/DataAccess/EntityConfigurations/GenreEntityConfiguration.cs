using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesAPI.DataAccess.Entities;

namespace MoviesAPI.DataAccess.EntityConfigurations
{
    public class GenreEntityConfiguration : IEntityTypeConfiguration<GenreEntity>
    {
        public void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            builder.ToTable("Genres");

            builder.HasKey(x => x.GenreId);

            builder.Property(x => x.GenreId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.GenreName)
                .HasMaxLength(200)
                .IsRequired();

        }

    }
}
