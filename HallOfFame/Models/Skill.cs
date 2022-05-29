using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HallOfFame.Models;

public class Skill
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    public byte Level { get; set; }

    [JsonIgnore]
    public ICollection<Person>? Persons { get; set; }
}