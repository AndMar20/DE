namespace DE.DataLayer.Models;

public partial class DeOrderHasDeProduct
{
    public string ProductId { get; set; } = null!;

    public int OrderId { get; set; }

    public int Quantity { get; set; }

    public virtual DeOrder Order { get; set; } = null!;

    public virtual DeProduct Product { get; set; } = null!;
}
