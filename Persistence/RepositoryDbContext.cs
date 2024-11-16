using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;

namespace Persistence;

public sealed class RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
    : DbContext(options)
{
    public DbSet<BookClub> BookClubs { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);

        ConfigureNamingConventions(modelBuilder);
    }

    private static void ConfigureNamingConventions(ModelBuilder modelBuilder)
    {
        var snakeCaseNameTranslator = new NpgsqlSnakeCaseNameTranslator();

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Set table name to snake_case
            entity.SetTableName(
                snakeCaseNameTranslator.TranslateMemberName(entity.GetTableName() ?? string.Empty)
            );

            // Set all property names to snake_case
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(snakeCaseNameTranslator.TranslateMemberName(property.Name));
            }

            // Set foreign key names to snake_case
            foreach (var key in entity.GetKeys())
            {
                var name = key.GetName();
                if (name is not null)
                    key.SetName(snakeCaseNameTranslator.TranslateMemberName(name));
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                var name = fk.GetConstraintName();
                if (name is not null)
                    fk.SetConstraintName(snakeCaseNameTranslator.TranslateMemberName(name));
            }

            foreach (var index in entity.GetIndexes())
            {
                var name = index.GetDatabaseName();
                if (name is not null)
                    index.SetDatabaseName(name);
            }
        }
    }
}
