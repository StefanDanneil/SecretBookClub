using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;

namespace Data;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BookClub> BookClubs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureNamingConventions(modelBuilder);
    }

    private static void ConfigureNamingConventions(ModelBuilder modelBuilder)
    {
        var snakeCaseNameTranslator = new NpgsqlSnakeCaseNameTranslator();

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Set table name to snake_case
            entity.SetTableName(snakeCaseNameTranslator.TranslateMemberName(entity.GetTableName() ?? string.Empty));

            // Set all property names to snake_case
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(snakeCaseNameTranslator.TranslateMemberName(property.Name));
            }

            // Set foreign key names to snake_case
            foreach (var key in entity.GetKeys())
            {
                key.SetName(snakeCaseNameTranslator.TranslateMemberName(key.GetName()));
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(snakeCaseNameTranslator.TranslateMemberName(fk.GetConstraintName()));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(snakeCaseNameTranslator.TranslateMemberName(index.GetDatabaseName()));
            }
        }
    }
}