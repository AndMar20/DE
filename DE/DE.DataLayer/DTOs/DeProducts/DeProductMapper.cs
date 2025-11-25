using DE.DataLayer.Models;
using System.Runtime.CompilerServices;

namespace DE.DataLayer.DTOs.DeProducts
{
    public static class DeProductMapper
    {
        public static DeProductDto ToDto(this DeProduct deProduct, string? baseUrl)
        {
            if (deProduct is null)
                throw new ArgumentNullException(nameof(deProduct));

            string? photoUrl = deProduct.Photo;
            if (!string.IsNullOrEmpty(deProduct.Photo) && !string.IsNullOrEmpty(baseUrl))
            {
                // Убираем слэш в конце baseUrl, если есть, и добавляем путь к фото
                photoUrl = $"{baseUrl.TrimEnd('/')}/images/products/{deProduct.Photo}";
            }

            return new DeProductDto
            {
                Id = deProduct.Id,
                ProductName = deProduct.ProductName,
                Price = deProduct.Price,
                SupplierId = deProduct.SupplierId,
                Supplier = deProduct.Supplier?.Supplier,
                ManufacturerId = deProduct.ManufacturerId,
                Manufacturer = deProduct.Manufacturer?.Manufacturer,
                Category = deProduct.Category,
                Discount = deProduct.Discount,
                StockQuantity = deProduct.StockQuantity,
                UnitOfMeasure = "шт.",
                Description = deProduct.Description,
                Photo = photoUrl,
                PriceWithDiscount = deProduct.Price - deProduct.Price * (decimal)deProduct.Discount / 100
            };
        }
    }
}
