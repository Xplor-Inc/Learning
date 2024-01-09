namespace XploringMe.SqlServer.Maps.Finance.Tags;

public class TransactionTaggingMap : Map<TransactionTagging>
{
    public override void Configure(EntityTypeBuilder<TransactionTagging> entity)
    {
        entity
            .ToTable("Finance_Taggings");
    }
}

