namespace DE.DataLayer.Models;

public partial class DeProduct
{
    public string Id { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public bool Category { get; set; }

    public double Discount { get; set; }

    public int StockQuantity { get; set; }

    public string Description { get; set; } = null!;

    public string? Photo { get; set; }

    public virtual ICollection<DeOrderHasDeProduct> DeOrderHasDeProducts { get; set; } = new List<DeOrderHasDeProduct>();

    public virtual DeManufacturer Manufacturer { get; set; } = null!;

    public virtual DeSupplier Supplier { get; set; } = null!;
}
