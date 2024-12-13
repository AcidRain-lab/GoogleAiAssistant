
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace BLL
{

    // file example 

    /*
Name MeasureTypes EnergyValue Proteines Carbs Fat ProductCategory IsAllergyc
-------------------------------------------

Beef 2 250.0 26.0 0.0 15.0 3 False
Chicken Breast 2 165.0 31.0 0.0 3.6 3 False
Pork 2 242.0 27.0 0.0 14.0 3 False
Lamb 2 294.0 25.0 0.0 21.0 3 False
Bacon 2 541.0 37.0 1.4 42.0 3 True
Turkey 2 135.0 29.0 0.0 1.0 3 False
Duck 2 337.0 19.0 0.0 28.0 3 False
Sausage 2 301.0 12.0 1.9 27.0 3 True
Veal 2 172.0 24.0 0.0 8.0 3 False
Ham 2 145.0 20.0 1.5 7.5 3 True
Ham2 2 145.0 20.0 1.5 7.5 3 True
Bacon2 2 541.0 37.0 1.4 42.0 3 True
    */

    public class ProductImportService
    {
        private readonly CrmContext _context;
        private readonly EdamamService _edamamService;

        public ProductImportService(CrmContext context, EdamamService edamamService)
        {
            _context = context;
            _edamamService = edamamService;
        }

        public async Task ImportProductsFromFileAsync(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                var lines = await System.IO.File.ReadAllLinesAsync(filePath);
                var products = new List<Product>();
                int lineNumber = 0;

                foreach (var line in lines)
                {
 
                        var product = ParseProductFromLine(line);
                        if (product != null && !await ProductExists(product.Name))
                        {
                            products.Add(product);
                        }

                }

                _context.Products.AddRange(products);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new FileNotFoundException($"The file at path {filePath} was not found.");
            }
        }


        public async Task ImportProductsFromEdamamAsync(string query)
        {
            JObject foodData = await _edamamService.SearchFoodAsync(query);
            var products = new List<Product>();

            foreach (var food in foodData["hints"])
            {
                var product = ParseProductFromEdamamData(food);
                if (product != null && !await ProductExists(product.Name))
                {
                    products.Add(product);
                }
            }

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();
        }


        private Product? ParseProductFromEdamamData(JToken food)
        {
            try
            {
                return new Product
                {
                    Id = Guid.NewGuid(),
                    Name = food["food"]["label"].ToString(),
                    //MeasurementTypesId = 2, // Assuming grams for this example
                    CaloriesPer100 = food["food"]["nutrients"]["ENERC_KCAL"].Value<decimal>(),
                    ProteinsPer100 = food["food"]["nutrients"]["PROCNT"].Value<decimal>(),
                    CarbsPer100 = food["food"]["nutrients"]["CHOCDF"].Value<decimal>(),
                    FatsPer100 = food["food"]["nutrients"]["FAT"].Value<decimal>(),
                    //ProductTypeId = 1, // Assuming a generic type for this example
                    IsAllergyc = false // Assuming default value; adjust based on real data
                };
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                Console.WriteLine($"Error parsing food data: {food}. Exception: {ex.Message}");
                return null;
            }
        }

        private async Task<bool> ProductExists(string name)
        {
            return await _context.Products.AnyAsync(p => p.Name == name);
        }

        private Product? ParseProductFromLine(string line)
        {
            var parts = line.Split(' ');

            if (parts.Length == 8)
            {
                try
                {
                    return new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = parts[0],
                        //MeasuresId = int.Parse(parts[1]),
                        CaloriesPer100 = decimal.Parse(parts[2]),
                        ProteinsPer100 = decimal.Parse(parts[3]),
                        CarbsPer100 = decimal.Parse(parts[4]),
                        FatsPer100 = decimal.Parse(parts[5]),
                        //ProductTypeId = int.Parse(parts[6]),
                        IsAllergyc = bool.Parse(parts[7])
                    };
                }
                catch (Exception ex)
                {
                    // Log or handle the exception if needed
                    Console.WriteLine($"Error parsing line: {line}. Exception: {ex.Message}");
                }
            }

            return null;
        }
    }
}
