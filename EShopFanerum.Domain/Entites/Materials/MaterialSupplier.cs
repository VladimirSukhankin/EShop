﻿namespace EShopFanerum.Domain.Entites.Materials;

public class MaterialSupplier : IEntity<long>
{
    public long Id { get; set; }
    public long SupplierId { get; set; }
    
    public long MaterialId { get; set; }
}