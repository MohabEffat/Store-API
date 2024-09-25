using Microsoft.Extensions.Logging;
using Store.Data.Contexts;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context, ILoggerFactory loggerFactory)
        {
			try
			{
				if (context.productBrands is not null && !context.productBrands.Any())
				{
					var brandsData = File.ReadAllText("../Store.Repository/Data/SeedData/brands.json");
					var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
					if (brands is not null)
						await context.productBrands.AddRangeAsync(brands);
                }
                if (context.productTypes is not null && !context.productTypes.Any())
                {
                    var typesData = File.ReadAllText("../Store.Repository/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types is not null)
                        await context.productTypes.AddRangeAsync(types);
                }
                if (context.products is not null && !context.products.Any())
                {
                    var productsData = File.ReadAllText("../Store.Repository/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products is not null)
                        await context.products.AddRangeAsync(products);
                }
                await context.SaveChangesAsync();
            }
			catch (Exception ex)
			{

                var logger = loggerFactory.CreateLogger<StoreDbContext>();
                logger.LogError(ex.Message);

            }
        }

    }
}
