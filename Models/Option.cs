namespace IBUAPI.Models;

public class Option
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public int? UserId { get; set; } = null;

}