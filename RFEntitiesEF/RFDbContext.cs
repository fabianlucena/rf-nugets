using Microsoft.EntityFrameworkCore;

namespace RFEntitiesEF;

public class RFDbContext(DbContextOptions options, bool convertToSnakeCase = false) : DbContext(options)
{
    // public DbSet<EntityName> TableName { get; set; }

    public bool ConvertToSnakeCase { get; } = convertToSnakeCase;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName?.StartsWith("RF") ?? false)
            .Where(a => a.GetReferencedAssemblies()
                .Any(r => r.Name == "Microsoft.EntityFrameworkCore")
            )
            .ToArray();

        foreach (var assembly in assemblies)
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        if (ConvertToSnakeCase)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(ToSnakeCase(entity.GetTableName()!));

                foreach (var property in entity.GetProperties())
                    property.SetColumnName(ToSnakeCase(property.GetColumnName()!));

                foreach (var key in entity.GetKeys())
                    key.SetName(ToSnakeCase(key.GetName()!));

                foreach (var fk in entity.GetForeignKeys())
                    fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()!));

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()!));
            }
        }
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var chars = new List<char>(input.Length + 10);

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (char.IsUpper(c))
            {
                if (i > 0)
                    chars.Add('_');

                chars.Add(char.ToLowerInvariant(c));
            }
            else
            {
                chars.Add(c);
            }
        }

        return new string([.. chars]);
    }
}
