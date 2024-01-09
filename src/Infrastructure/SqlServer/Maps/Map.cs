﻿namespace XploringMe.SqlServer.Maps;
public abstract class Map<TEntity> where TEntity : class
{
    public abstract void Configure(EntityTypeBuilder<TEntity> entity);
}
