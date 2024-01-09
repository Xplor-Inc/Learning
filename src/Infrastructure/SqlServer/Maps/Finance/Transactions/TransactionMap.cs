using Microsoft.EntityFrameworkCore;
using XploringMe.Core.Constants;

namespace XploringMe.SqlServer.Maps.Finance.Transactions;

public class TransactionMap : Map<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> entity)
    {
        entity
            .ToTable("Finance_Transactions");

        entity
            .Property(t => t.TransactionType)
            .HasConversion<string>();

        entity
            .Property(t => t.Source)
            .HasConversion<string>();

        entity
            .Property(e => e.Particular)
            .IsRequired()
            .HasMaxLength(StaticConfiguration.COMMAN_LENGTH);

        entity
            .Property(e => e.InvoicePath)
            .HasMaxLength(StaticConfiguration.COMMAN_LENGTH);

        entity
           .Property(e => e.Debit)
           .IsRequired()
           .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
           .Property(e => e.Credit)
           .IsRequired()
           .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
           .Property(e => e.CurrentBalance)
           .IsRequired()
           .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
          .Property(e => e.PreviousBalance)
          .IsRequired()
          .HasPrecision(StaticConfiguration.DECIMAL_PRECISION, StaticConfiguration.DECIMAL_SCALE_2);

        entity
            .HasOne(d => d.Parent)
            .WithMany(p => p.ChildTransactions)
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
