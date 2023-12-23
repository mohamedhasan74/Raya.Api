using Raya.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Raya.Repository.Data
{
    public class AppDbContextSeed
    {
        public static async Task ProductSeedAsync(AppDbContext context)
        {
            if (context.Products.Count() == 0)
            {
                var dataSeed = File.ReadAllText("../Raya.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(dataSeed);
                if (products is not null)
                {
                    foreach (var brand in products)
                    {
                        await context.Products.AddAsync(brand);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
