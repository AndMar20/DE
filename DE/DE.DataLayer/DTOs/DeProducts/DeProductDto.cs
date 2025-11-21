namespace DE.DataLayer.DTOs.DeProducts
{
    public partial class DeProductDto
    {
        public string Id { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public int SupplierId { get; set; }

        public string? Supplier { get; set; }

        public int ManufacturerId { get; set; }

        public string? Manufacturer { get; set; }

        public bool Category { get; set; }

        public double Discount { get; set; }

        public int StockQuantity { get; set; }

        public string UnitOfMeasure { get; set; } = "шт.";

        public string Description { get; set; } = null!;

        public string? Photo { get; set; }
    }
}
