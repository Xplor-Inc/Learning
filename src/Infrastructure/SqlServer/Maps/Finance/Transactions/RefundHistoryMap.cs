using XploringMe.Core.Constants;

namespace XploringMe.SqlServer.Maps.Finance.Transactions;

public class RefundHistoryMap : Map<RefundHistory>
{
    public override void Configure(EntityTypeBuilder<RefundHistory> entity)
    {
        entity
            .ToTable("Finance_RefundHistories");

        entity
            .Property(e => e.Remark)
            .IsRequired()
            .HasMaxLength(StaticConfiguration.COMMAN_LENGTH);

        entity
           .Property(e => e.Total)
           .IsRequired()
           .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
           .Property(e => e.Balance)
           .IsRequired()
           .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
           .Property(e => e.Refund)
           .IsRequired()
           .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);
    }
}
