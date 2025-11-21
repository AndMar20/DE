namespace DE.DataLayer.Models;

public partial class DeSupplier
{
    public int Id { get; set; }

    public string Supplier { get; set; } = null!;

    public virtual ICollection<DeProduct> DeProducts { get; set; } = new List<DeProduct>();
}
