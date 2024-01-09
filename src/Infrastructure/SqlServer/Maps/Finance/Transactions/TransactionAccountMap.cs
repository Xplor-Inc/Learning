using XploringMe.Core.Constants;

namespace XploringMe.SqlServer.Maps.Finance.Transactions;

public class TransactionAccountMap : Map<TransactionAccount>
{
    public override void Configure(EntityTypeBuilder<TransactionAccount> entity)
    {
        entity
            .ToTable("Finance_TransactionAccounts");

        entity
            .Property(t => t.AccountType)
            .HasConversion<string>();

        entity
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(StaticConfiguration.COMMAN_LENGTH);

        entity
            .Property(e => e.AccountNumber)
            .IsRequired()
            .HasMaxLength(StaticConfiguration.NAME_LENGTH);

        entity
            .Property(e => e.OpeningBalance)
            .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
            .Property(e => e.Balance)
            .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);
    }
}
