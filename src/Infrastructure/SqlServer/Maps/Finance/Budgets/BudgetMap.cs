using XploringMe.Core.Constants;

namespace XploringMe.SqlServer.Maps.Finance.Budgets;

public class BudgetMap : Map<Budget>
{
    public override void Configure(EntityTypeBuilder<Budget> entity)
    {
        entity
            .ToTable("Finance_Budgets");

        entity
            .Property(e => e.Actual)
            .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
            .Property(e => e.Expected)
            .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
            .Property(e => e.Description)
            .HasMaxLength(StaticConfiguration.MAX_LENGTH);
    }
}

