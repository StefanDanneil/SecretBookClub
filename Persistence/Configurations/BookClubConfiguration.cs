using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class BookClubConfiguration : IEntityTypeConfiguration<BookClub>
{
    public void Configure(EntityTypeBuilder<BookClub> builder)
    {
        builder.HasKey(bookClub => bookClub.Id);
        builder.Property(bookClub => bookClub.Name).HasMaxLength(60);

        builder.HasMany(bookClub => bookClub.Members).WithMany(user => user.BookClubs);
    }
}
