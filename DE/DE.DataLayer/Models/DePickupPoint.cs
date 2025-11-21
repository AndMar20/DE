namespace DE.DataLayer.Models;

public partial class DePickupPoint
{
    public int Id { get; set; }

    public int Index { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public int StreetNumber { get; set; }

    public virtual ICollection<DeOrder> DeOrders { get; set; } = new List<DeOrder>();
}
