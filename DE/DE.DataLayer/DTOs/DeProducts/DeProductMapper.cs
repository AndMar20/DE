using DE.DataLayer.Models;

namespace DE.DataLayer.DTOs.DeProducts
{
    public static class DeProductMapper
    {
        public static DeProductDto ToDto(this DeProduct deProduct, string? baseUrl)
        {
            if (deProduct is null)
                throw new ArgumentNullException(nameof(deProduct));

            string? photoUrl = null;
            if (!string.IsNullOrWhiteSpace(deProduct.Photo))
            {
                var photoFileName = deProduct.Photo.Contains('.') ? deProduct.Photo : $"{deProduct.Photo}.jpg";

                // При наличии baseUrl формируем абсолютный путь для API, иначе используем локальный ресурс из каталога Photos
                photoUrl = string.IsNullOrEmpty(baseUrl)
                    ? $"Photos/{photoFileName}"
                    : $"{baseUrl.TrimEnd('/')}/images/products/{photoFileName}";
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
