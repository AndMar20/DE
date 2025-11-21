namespace DE.DataLayer.Models;

public partial class DeManufacturer
{
    public int Id { get; set; }

    public string Manufacturer { get; set; } = null!;

    public virtual ICollection<DeProduct> DeProducts { get; set; } = new List<DeProduct>();
}
