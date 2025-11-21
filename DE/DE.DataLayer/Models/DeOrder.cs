namespace DE.DataLayer.Models;

public partial class DeOrder
{
    public int Id { get; set; }

    public DateTime DateOrder { get; set; }

    public DateTime DateDelivery { get; set; }

    public int PickupPointId { get; set; }

    public int CustomerId { get; set; }

    public int CodeReceipt { get; set; }

    public bool StatusOrder { get; set; }

    public virtual DeUser Customer { get; set; } = null!;

    public virtual ICollection<DeOrderHasDeProduct> DeOrderHasDeProducts { get; set; } = new List<DeOrderHasDeProduct>();

    public virtual DePickupPoint PickupPoint { get; set; } = null!;
}
