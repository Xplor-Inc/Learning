using XploringMe.Core.Constants;

namespace XploringMe.SqlServer.Maps.Finance.Categories;

public class CategoryMap : Map<Category>
{
    public override void Configure(EntityTypeBuilder<Category> entity)
    {
        entity
            .ToTable("Finance_Categories");

        entity
            .Property(t => t.Type)
            .HasConversion<string>();

        entity
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(StaticConfiguration.COMMAN_LENGTH);

        entity
            .Property(e => e.Color)
            .IsRequired()
            .HasMaxLength(StaticConfiguration.NAME_LENGTH);

        entity
            .Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(StaticConfiguration.COMMAN_LENGTH);
    }
}
