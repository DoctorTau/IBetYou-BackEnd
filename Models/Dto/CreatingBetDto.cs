using IBUAPI.Models;

namespace BackEnd.Models.Dto;

public class CreatingBetDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new List<string>();
    public DateTime? EndDate { get; set; } = null;

    public CreatingBetDto(string Name, string Description, List<string> Options, DateTime? EndDate = null)
    {
        this.Name = Name;
        this.Description = Description;
        this.Options = Options;
        this.EndDate = EndDate;
    }
}


