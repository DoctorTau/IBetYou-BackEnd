public class PostBetDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? EndDate { get; set; } = null;

    public PostBetDto(string Name, string Description, DateTime? EndDate = null)
    {
        this.Name = Name;
        this.Description = Description;
        this.EndDate = EndDate;
    }
}