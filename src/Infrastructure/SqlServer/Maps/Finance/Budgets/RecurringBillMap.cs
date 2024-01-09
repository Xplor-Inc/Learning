using XploringMe.Core.Constants;

namespace XploringMe.SqlServer.Maps.Finance.Budgets;

public class RecurringBillMap : Map<RecurringBill>
{
    public override void Configure(EntityTypeBuilder<RecurringBill> entity)
    {
        entity
            .ToTable("Finance_RecurringBills");

        entity
            .Property(e => e.BillName)
            .HasMaxLength(StaticConfiguration.COMMAN_LENGTH);

        entity
           .Property(e => e.Amount)
           .IsRequired()
           .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);
    }
}

