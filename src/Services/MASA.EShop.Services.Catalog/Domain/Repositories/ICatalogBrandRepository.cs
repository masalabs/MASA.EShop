﻿namespace MASA.EShop.Services.Catalog.Domain.Repositories
{
    public interface ICatalogBrandRepository
    {
        IQueryable<CatalogBrand> GetAll();
    }
}
