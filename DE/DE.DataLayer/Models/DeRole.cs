namespace DE.DataLayer.Models;

public partial class DeRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DeUser> DeUsers { get; set; } = new List<DeUser>();
}
