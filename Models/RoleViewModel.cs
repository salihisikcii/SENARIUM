public class RoleViewModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public List<string> Users { get; set; } = new List<string>();
}
