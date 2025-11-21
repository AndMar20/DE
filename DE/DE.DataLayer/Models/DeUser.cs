namespace DE.DataLayer.Models;

public partial class DeUser
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<DeOrder> DeOrders { get; set; } = new List<DeOrder>();

    public virtual DeRole Role { get; set; } = null!;
}
